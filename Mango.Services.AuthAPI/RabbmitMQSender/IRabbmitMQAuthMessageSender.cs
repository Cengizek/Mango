﻿namespace Mango.Services.AuthAPI.RabbitMQSender
{
    public interface IRabbmitMQAuthMessageSender
    {
        void SendMessage(Object message, string queueName);
    }
}