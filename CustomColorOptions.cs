using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingSample
{
    public class CustomColorOptions : SimpleConsoleFormatterOptions
    {
        public string CustomPrefix { get; set; }
    }
}
