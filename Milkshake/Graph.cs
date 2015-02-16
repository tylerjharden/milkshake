using Schloss.Data.Neo4j;
using System;
using System.Net;

namespace Milkshake
{
    internal class Graph
    {
        private static GraphClient client;
        
        public static GraphClient Instance
        {
            get 
            {
                if (client == null)
                    Initialize();

                return client; 
            }
        }

        static Graph()
        {
            Initialize();
        }
        
        static void Initialize()
        {
            try
            {
                Logger.Log("Initializing Milkshake database connection...");

                ServicePointManager.Expect100Continue = false;
                ServicePointManager.DefaultConnectionLimit = 200;
                ServicePointManager.UseNagleAlgorithm = false;

                HttpClientWrapper clienthttp = new HttpClientWrapper();
                                
                client = new GraphClient(new Uri("http://10.0.0.40:7474/db/data"), clienthttp);
                client.Connect();

                client.OperationCompleted += client_OperationCompleted;

                Logger.Log("Connected to Milkshake database!");                                
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());

                client = null;                
            }
        }

        static void client_OperationCompleted(object sender, OperationCompletedEventArgs e)
        {
            Logger.Log("Graph Operation Completed\n" + e.QueryText + "\nResources Returned: " + e.ResourcesReturned + "\nTime: " + e.TimeTaken.Milliseconds + "ms");
        }
    }
}
