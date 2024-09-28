using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace Application.Services
{
    public class LoggerService : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, ConsoleLogger> _loggers = new ConcurrentDictionary<string, ConsoleLogger>();
        private readonly LogLevel _minLevel;

        public LoggerService(LogLevel minLevel = LogLevel.Information)
        {
            _minLevel = minLevel;
        }

        public ILogger CreateLogger(string categoryName)
            => _loggers.GetOrAdd(categoryName, name => new ConsoleLogger(name, _minLevel));

        public void Dispose()
        {
            _loggers.Clear();
        }

        private class ConsoleLogger : ILogger
        {
            private readonly string _categoryName;
            private readonly LogLevel _minLevel;

            public ConsoleLogger(string categoryName, LogLevel minLevel)
            {
                _categoryName = categoryName;
                _minLevel = minLevel;
            }

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception exception,
                Func<TState, Exception, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                var message = formatter(state, exception);
                var logEntry = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} [{logLevel}] {_categoryName}: {message}";

                if (exception != null)
                {
                    logEntry += $"\n{exception}";
                }

                Console.WriteLine(logEntry);
            }

            public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLevel;

            public IDisposable BeginScope<TState>(TState state) => null;
        }
    }
}