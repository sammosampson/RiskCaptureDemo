namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using System.Diagnostics.CodeAnalysis;
    using Core;

    public class MessageStorageMode : Equatable<MessageStorageMode>
    {
        [ExcludeFromCodeCoverage]
        public static MessageStorageMode Parse(string mode)
        {
            return new MessageStorageMode(mode);
        }

        public static MessageStorageMode Persistent => new MessageStorageMode("Persistent");

        public static MessageStorageMode NonPersistent => new MessageStorageMode("NonPersistent");

        private readonly string mode;

        private MessageStorageMode(string mode)
        {
            this.mode = mode;
        }

        protected override bool IsEquatable(MessageStorageMode other)
        {
            return other.mode == mode;
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return mode.GetHashCode();
        }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return mode;
        }
    }
}