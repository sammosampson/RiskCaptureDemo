namespace AppliedSystems.Documents
{
    using System.Data;
    using System.Linq;
    using AppliedSystems.Collections;
    using AppliedSystems.Data.Connections;
    using AppliedSystems.Documents.Messages;
    using AppliedSystems.Infrastucture.Data;
    using AppliedSystems.Messaging.Infrastructure.Commands;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;
    using Dapper;

    public class MergeFieldValueIntoDocumentHandler : ICommandHandler<MergeFieldValueIntoDocument>
    {
        private readonly SqlRunner runner;
        private readonly IProjectionStore projectionStore;

        public MergeFieldValueIntoDocumentHandler(SqlRunner runner, IProjectionStore projectionStore)
        {
            this.runner = runner;
            this.projectionStore = projectionStore;
        }

        public void Handle(MergeFieldValueIntoDocument message)
        {
            const string InsertSql = @"
INSERT INTO [Document](DataCaptureId, Text) 
SELECT @DataCaptureId, [Text] FROM Template 
WHERE NOT EXISTS(SELECT * FROM [Document] WHERE DataCaptureId = @DataCaptureId)";

            var mapping = projectionStore
               .GetProjection<RiskCaptureItemToDocumentFieldMapping>(RiskCaptureItemToDocumentFieldMappingId.Parse(message.ProductLine))
               .Single(i => i.ItemId == message.DataCaptureId);
            
            runner.ExecuteConnectionAction(connection =>
            {
                connection.Execute(InsertSql, new { message.DataCaptureId });

                var documents = connection
                    .Query<Document>("SELECT * from Document WHERE DataCaptureId = @DataCaptureId", new { message.DataCaptureId });

                documents.ForEach(document =>
                {
                    document.Merge(mapping.FieldName, message.FieldValue);
                    connection.Execute("UPDATE [Document] SET Text = @Text WHERE DataCaptureId = @DataCaptureId", document);
                });
            });
        }
    }
}