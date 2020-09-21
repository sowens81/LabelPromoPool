using System;
namespace PromoPool.audioConverterAPI.Settings
{
    public interface IMessageQueueSettings
    {
        string HostName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        int Port { get; set; }
        int RequestConnectionTimeout { get; set; }
    }
}
