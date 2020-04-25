using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Client.Models
{
    public class SasTokenResponse
    {
        public string Audience { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
        public virtual string TokenValue { get; set; }
        public virtual string TokenType { get; set; }
        public string Endpoint { get; set; }
    }
}
