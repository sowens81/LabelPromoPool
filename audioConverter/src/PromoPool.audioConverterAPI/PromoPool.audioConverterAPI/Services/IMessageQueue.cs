using System;
using PromoPool.audioConverterAPI.Models;

namespace PromoPool.audioConverterAPI.Services
{
    public interface IMessageQueue
    {
        string AddQueueMessage(AudioSchema audioSchema);
    }
}
