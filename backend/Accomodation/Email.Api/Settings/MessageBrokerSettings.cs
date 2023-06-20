﻿namespace Email.Api.Settings
{
    public class MessageBrokerSettings
    {
        public const string SectionName = "MessageBroker";
        public string Host { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
