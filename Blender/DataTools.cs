using Milkshake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blender
{
    public static class DataTools
    {
        public static void GetProductStoreErrors()
        {
            int count = (int)Graph.Instance.Cypher
                .Match("(n:Product),(s:Store)")
                .Where("NOT (n)-->(s)")
                .ReturnDistinct(n => n.CountDistinct())
                .Results.Single();

            Program.g_Main.PSEComplete(count);
            
            /*while (count > 0)
            {
                Guid newid = Guid.NewGuid();
                
                Graph.Instance.Cypher
                    .Match("(n:Product)")
                    .Where("n.Id = 'null'")
                    .With("n")
                    .Limit(1)
                    .Set("n.Id = {guid}")
                    .WithParam("guid", newid)
                    .ExecuteWithoutResults();

                count--;

                Console.WriteLine(count + " issues remaining.");           
            }*/
        }
    }
}
