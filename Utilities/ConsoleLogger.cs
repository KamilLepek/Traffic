using System;
using System.IO;

namespace Traffic.Utilities
{
    /// <summary>
    ///     Class used for logging results to file/console output
    /// </summary>
    public static class ConsoleLogger
    {
        private static int row;

        /// <summary>
        ///     Log to file and console
        /// </summary>
        /// <param name="message"> message to log </param>
        public static void Log(string message)
        {
            //ConsoleLogger.LogOnConsole(message);
            LogToFile(message);
        }

        /// <summary>
        ///     Logs to console
        /// </summary>
        /// <param name="message"> message to log </param>
        private static void LogOnConsole(string message)
        {
            Console.SetCursorPosition(40, row);
            row++;
            Console.Write(message);
        }

        /// <summary>
        ///     Logs to file 
        /// </summary>
        /// <param name="message"> message to log </param>
        private static void LogToFile(string message)
        {
            using (var file = new StreamWriter(Constants.LogFile, true))
            {
                file.WriteLine(message);
            }
        }

        /// <summary>
        ///     Deletes file with logs
        /// </summary>
        public static void DeleteLogs()
        {
            File.Delete(Constants.LogFile);
        }
    }
}
