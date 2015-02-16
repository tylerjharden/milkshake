using Topshelf;

namespace Milkshake.Service
{
    class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
                {
                    x.Service<IndexingService>(s =>
                        {
                            s.ConstructUsing(name => new IndexingService());
                            s.WhenStarted(tc => tc.Start());
                            s.WhenStopped(tc => tc.Stop());
                        });
                    x.RunAsLocalSystem();

                    x.SetDescription("Milkshake.Service (Indexing)");
                    x.SetDisplayName("Milkshake.Service (Indexing)");
                    x.SetServiceName("Milkshake.Service.Indexing");
                });
        }
    }
}
