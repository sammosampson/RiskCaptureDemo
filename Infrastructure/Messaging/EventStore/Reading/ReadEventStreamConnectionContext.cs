namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Reading
{
    public static class ReadEventStreamConnectionContext
    {
        public static IReadEventStreamConnection Current { get; set; }

        public static void CloseCurrentConnection()
        {
            Current.Close();
            Current.Dispose();
            Current = null;
        }
    }
}