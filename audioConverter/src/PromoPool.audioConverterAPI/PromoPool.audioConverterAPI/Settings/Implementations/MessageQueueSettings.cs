using System;
namespace PromoPool.audioConverterAPI.Settings.Implementations
{
    public class MessageQueueSettings : IMessageQueueSettings
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public int RequestConnectionTimeout { get; set; } 
    }
}
