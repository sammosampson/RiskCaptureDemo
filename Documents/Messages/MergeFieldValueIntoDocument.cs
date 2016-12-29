namespace AppliedSystems.Documents.Messages
{
    using System;
    using AppliedSystems.Messaging.Messages;

    public class MergeFieldValueIntoDocument : ICommand
    {
        public Guid DataCaptureId { get; private set; }
        public string ProductLine { get; private set; }
        public string FieldValue { get; private set; }

        public MergeFieldValueIntoDocument(Guid dataCaptureId, string productLine, string fieldValue)
        {
            DataCaptureId = dataCaptureId;
            ProductLine = productLine;
            FieldValue = fieldValue;
        }
    }
}