using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public class LogEntry
    {
        public int EventId { get; internal set; }
        public string Message { get; internal set; }
        public string LogLevel { get; internal set; }
        public DateTime CreatedTime { get; internal set; }
    }
}
