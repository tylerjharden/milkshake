using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake
{
    internal delegate void LogPrint(string msg);

    internal static class Logger
    {        
        public static LogPrint LogExtension;
                                        
        private static async Task Log(LogType type, string message)
        {
            try
            {
                string final = "";

                switch (type)
                {
                    case LogType.Error:
                        final = "(Error) " + message;
                        break;

                    case LogType.Message:
                        final = message;
                        break;
                }

                final = "(Milkshake) " + final;

                await Redis.PushLogMessage(final);

                try
                {
                    if (LogExtension != null)
                        LogExtension.Invoke(final);
                }
                catch { }
            }
            catch { }
        }

        public static void Log(string message)
        {
            Log(LogType.Message, message).ConfigureAwait(false);
        }

        public static async Task LogAsync(string message)
        {
            await Log(LogType.Message, message);
        }

        public static void Error(string message)
        {
            Log(LogType.Error, message).ConfigureAwait(false);
        }

        public static async Task ErrorAsync(string message)
        {
            await Log(LogType.Error, message);
        }

        public static void Error(Exception ex)
        {
            Log(LogType.Error, "Exception: " + ex.Message + " Source: " + ex.Source + " Stack Trace: " + ex.StackTrace).ConfigureAwait(false);            
        }

        public static async Task ErrorAsync(Exception ex)
        {
            await Log(LogType.Error, "Exception: " + ex.Message + " Source: " + ex.Source + " Stack Trace: " + ex.StackTrace);
        }
    }

    internal enum LogType
    {
        Message,
        Error
    }
}