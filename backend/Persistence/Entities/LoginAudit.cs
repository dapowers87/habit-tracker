using System;

namespace Persistence.Entities
{
    public class LoginAudit
    {
        public int LoginAuditId { get; set; }
        public string IpAddress { get; set; }
        public string UserName { get; set; }
        public DateTime LoginAttemptDate { get; set; }
        public int FailCount { get; set; }
    }
}