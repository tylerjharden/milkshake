using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;

namespace Truffle.Common
{

    public
    static
     class Logger
    {
        private static TcpClient clientSocket;

        static Logger()
        {
            try
            {
                clientSocket = new TcpClient();
                clientSocket.Connect("127.0.0.1", 13000);
            }
            catch
            {
                // If we can't connect to Bubble, oh well.
            }
        }

        private static void blowBubble(string msg)
        {
            try
            {
                if (clientSocket.Connected == false || clientSocket.GetStream() == NetworkStream.Null)
                {
                    clientSocket.Connect("10.0.0.5", 13000);
                }

                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = Encoding.ASCII.GetBytes(msg + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.FlushAsync();
            }
            catch
            {
                try
                {
                    clientSocket = new TcpClient();
                    clientSocket.Connect("10.0.0.5", 13000);

                    NetworkStream serverStream = clientSocket.GetStream();
                    byte[] outStream = Encoding.ASCII.GetBytes(msg + "$");
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.FlushAsync();
                }
                catch { }
            }
        }

        private static void Log(LogType type, string message)
        {
            try
            {
                string final = "(Truffle) ";

                switch (type)
                {
                    case LogType.Error:
                        final = "(Truffle) (Error) " + message;
                        break;

                    case LogType.Message:
                        final = "(Truffle) " + message;
                        break;
                }

                //if (GenericPrincipal.Current.Identity.Name != String.Empty)
                //    final = "(Context:" + System.Security.Principal.GenericPrincipal.Current.Identity.Name + ") " + final;

                Action<object> action = (object msg) =>
                    {
                        blowBubble(msg.ToString());
                    };

                System.Threading.Tasks.Task t = new System.Threading.Tasks.Task(action, final);
                t.Start();
            }
            catch { }
        }

        public static void Log(string message)
        {
            Log(LogType.Message, message);
        }

        public static void Error(string message)
        {
            Log(LogType.Error, message);
        }
        //---------------------------------------------------------------------------------------------//
    }

    public enum LogType
    {
        Message = 1,
        Error
    }
    //================================================================================================//
}

