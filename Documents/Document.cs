namespace AppliedSystems.Documents
{
    using System;
    using AppliedSystems.Infrastucture;

    public class Document
    {
        public Guid DataCaptureId { get; set; }
        public string Text { get; set; }

        public void Merge(string fieldName, string fieldValue)
        {
            GreenLogger.Log("Merging {0} into document with value {1}", fieldName, fieldValue);
            Text = Text.Replace("{{" + fieldName + "}}", fieldValue);
        }
    }
}