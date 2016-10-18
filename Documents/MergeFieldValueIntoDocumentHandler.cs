namespace AppliedSystems.Documents
{
    using System.Data;
    using AppliedSystems.Collections;
    using AppliedSystems.Data.Connections;
    using AppliedSystems.Documents.Messages;
    using AppliedSystems.Messaging.Infrastructure.Commands;
    using Dapper;

    public class MergeFieldValueIntoDocumentHandler : ICommandHandler<MergeFieldValueIntoDocument>
    {
        private readonly IConnectionFactory connectionFactory;

        public MergeFieldValueIntoDocumentHandler(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public void Handle(MergeFieldValueIntoDocument message)
        {
            const string InsertSql = @"
INSERT INTO [Document](DataCaptureId, Text) 
SELECT @DataCaptureId, [Text] FROM Template 
WHERE NOT EXISTS(SELECT * FROM [Document] WHERE DataCaptureId = @DataCaptureId)";

            using (IDbConnection connection = connectionFactory.Create())
            {
                connection.Open();

                connection.Execute(InsertSql, new { message.DataCaptureId });

                var documents = connection
                    .Query<Document>("SELECT * from Document WHERE DataCaptureId = @DataCaptureId", new { message.DataCaptureId });

                documents.ForEach(document =>
                {
                    document.Merge(message.FieldName, message.FieldValue);
                    connection.Execute("UPDATE [Document] SET Text = @Text WHERE DataCaptureId = @DataCaptureId", document);
                });
            }
        }
    }
}