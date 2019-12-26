using System;
using FloodGate.SDK;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FloodGateClient.Test
{
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public void CreateInstanceOfConsoleLogger()
        {
            // Act
            ILogger consoleLogger = new ConsoleLogger();

            // Assert
            Assert.IsInstanceOfType(consoleLogger, typeof(ILogger));

            consoleLogger.Debug(string.Empty);

            consoleLogger.Info(string.Empty);

            consoleLogger.Warning(string.Empty);

            consoleLogger.Error(string.Empty);

            consoleLogger.Error(new Exception());
        }

        [TestMethod]
        public void CreateInstanceOfFileLogger()
        {
            // Act
            ILogger fileLogger = new FileLogger(null);

            // Assert
            Assert.IsInstanceOfType(fileLogger, typeof(ILogger));

            fileLogger.Debug(string.Empty);

            fileLogger.Info(string.Empty);

            fileLogger.Warning(string.Empty);

            fileLogger.Error(string.Empty);

            fileLogger.Error(new Exception());
        }
    }
}
