using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingSample
{
    public class CustomWrappingConsoleFormatterOptions : ConsoleFormatterOptions
    {
        public string CustomPrefix { get; set; }

        public string CustomSuffix { get; set; }
    }
}
