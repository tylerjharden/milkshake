using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schloss.IO.Csv;
using System.IO;

namespace MilkshakeTester
{
    public abstract class PIPECatalogParser : CatalogParser
    {
        public PIPECatalogParser()
        {
            _format = "*.txt";            
        }

        public override void ParseFile(string filename)
        {
            Console.Write("Found " + _name + " Catalog: " + filename + "...");

            using (CsvReader pipe = new CsvReader(new StreamReader(filename), true, '|') { MissingFieldAction = MissingFieldAction.ReplaceByNull, SkipEmptyLines = true })
            {
                int fieldCount = pipe.FieldCount;
                string[] headers = pipe.GetFieldHeaders();

                while (pipe.ReadNextRecord())
                {
                    RawProduct p = Parse(pipe);
                    AddProduct(p);
                }

                Console.WriteLine("done.");
            }
                        
            try
            {
                File.Move(filename, filename.Replace(_format, _doneFormat));
            }
            catch { }
        }

        public abstract RawProduct Parse(CsvReader node);
    }
}