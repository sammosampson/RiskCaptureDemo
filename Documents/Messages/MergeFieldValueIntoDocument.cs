namespace AppliedSystems.Documents.Messages
{
    using System;
    using AppliedSystems.Messaging.Messages;

    public class MergeFieldValueIntoDocument : ICommand
    {
        public Guid DataCaptureId { get; set; }
        public string ProductLine { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }

        public MergeFieldValueIntoDocument(Guid dataCaptureId, string productLine, string fieldName, string fieldValue)
        {
            DataCaptureId = dataCaptureId;
            ProductLine = productLine;
            FieldName = fieldName;
            FieldValue = fieldValue;
        }
    }
}