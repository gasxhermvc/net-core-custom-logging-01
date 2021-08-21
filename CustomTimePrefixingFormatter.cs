using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingSample
{
    public sealed class CustomTimePrefixingFormatter : ConsoleFormatter, IDisposable
    {
        private readonly IDisposable _optionsReloadToken;
        private CustomWrappingConsoleFormatterOptions _formatterOptions;

        public CustomTimePrefixingFormatter(IOptionsMonitor<CustomWrappingConsoleFormatterOptions> options)
            // Case insensitive
            : base(nameof(CustomTimePrefixingFormatter)) =>
            (_optionsReloadToken, _formatterOptions) =
                (options.OnChange(ReloadLoggerOptions), options.CurrentValue);

        private void ReloadLoggerOptions(CustomWrappingConsoleFormatterOptions options) =>
            _formatterOptions = options;

        public override void Write<TState>(
            in LogEntry<TState> logEntry,
            IExternalScopeProvider scopeProvider,
            TextWriter textWriter)
        {
            string message =
                logEntry.Formatter(
                    logEntry.State, logEntry.Exception);

            if (message == null)
            {
                return;
            }

            WritePrefix(textWriter);
            switch (logEntry.LogLevel)
            {
                case LogLevel.Information:
                    textWriter.Write($"\x1B[32m{logEntry.LogLevel}\x1B[39m\x1B[22m");
                    break;
                case LogLevel.Debug:
                    textWriter.Write($"\x1B[35m{logEntry.LogLevel}\x1B[39m\x1B[22m");
                    break;
                case LogLevel.Trace:
                    textWriter.Write($"\x1B[36m{logEntry.LogLevel}\x1B[39m\x1B[22m");
                    break;
                case LogLevel.Error:
                    textWriter.Write($"\x1B[1m\x1B[31m{logEntry.LogLevel}\x1B[39m\x1B[22m");
                    break;
                case LogLevel.Warning:
                    textWriter.Write($"\x1B[1m\x1B[33m{logEntry.LogLevel}\x1B[39m\x1B[22m");
                    break;
                case LogLevel.Critical:
                    textWriter.Write($"\x1B[33m{logEntry.LogLevel}\x1B[39m\x1B[22m");
                    break;
                default:
                    textWriter.Write($"\x1B[39m\x1B[22m{logEntry.LogLevel}\x1B[39m\x1B[22m");
                    break;

            }
            textWriter.Write(" ");
            textWriter.Write($" {logEntry.Category}[{logEntry.EventId.Id}] ");
            textWriter.Write(message);
            if (logEntry.Exception != null)
            {
                textWriter.Write($" {logEntry.Exception.ToString()}");

            }
            WriteSuffix(textWriter);
        }

        private void WritePrefix(TextWriter textWriter)
        {
            DateTime now = _formatterOptions.UseUtcTimestamp
                ? DateTime.UtcNow
                : DateTime.Now;

            textWriter.Write($"{_formatterOptions.CustomPrefix} {now.ToString(_formatterOptions.TimestampFormat)}");
        }

        private void WriteSuffix(TextWriter textWriter) => textWriter.WriteLine($" {_formatterOptions.CustomSuffix}");

        public void Dispose() => _optionsReloadToken?.Dispose();
    }
}
