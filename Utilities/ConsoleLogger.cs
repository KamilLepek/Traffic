using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Traffic.Utilities
{
    //Robocza klasa do logowania rzeczy na konsole/do pliku
    public static class ConsoleLogger
    {
        static int row = 0;

        public static void Log(string message)
        {
            //ConsoleLogger.LogOnConsole(message);//nie chce mi się tego ustawiać względem ekranu więc wolę logi w pliku, ale zostawiam, wystarczy odkomentować
            ConsoleLogger.LogToFile(message);
        }

        private static void LogOnConsole(string message)
        {
            Console.SetCursorPosition(40, row);
            row++;
            Console.Write(message);
        }

        private static void LogToFile(string message)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(Constants.LogFile, true))//prawilnie
            {
                file.WriteLine(message);
            }
        }

        public static void DeleteLogs()
        {
            File.Delete(Constants.LogFile);
        }
    }
}
