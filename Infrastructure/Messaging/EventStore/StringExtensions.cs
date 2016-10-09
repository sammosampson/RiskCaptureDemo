namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    using System.Text;

    public static class StringExtensions
    {
        public static byte[] ToUtf8(this string toConvert)
        {
            return Encoding.UTF8.GetBytes(toConvert);
        }
    }
}
