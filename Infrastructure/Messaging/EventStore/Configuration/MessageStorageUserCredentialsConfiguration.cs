namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Configuration
{
    public class MessageStorageUserCredentialsConfiguration
    {
        public string User { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(Password);
        }
    }
}
