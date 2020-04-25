namespace ServiceBus.Client.Models
{
    public class TopicSharedAccessPolicy
    {
        public string KeyName { get; }
        public string PrimaryKey { get; }

        public TopicSharedAccessPolicy(string keyName, string primaryKey)
        {
            KeyName = keyName;
            PrimaryKey = primaryKey;
        }
    }
}
