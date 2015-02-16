using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schloss.IO.Csv;
using System.IO;

namespace MilkshakeTester
{
    public abstract class CSVCatalogParser : CatalogParser
    {
        public CSVCatalogParser()
        {
            _format = "*.csv";            
        }

        public override void ParseFile(string filename)
        {
            Console.Write("Found " + _name + " Catalog: " + filename + "...");

            using (CsvReader csv = new CsvReader(new StreamReader(filename), true, ',') { MissingFieldAction = MissingFieldAction.ReplaceByNull, SkipEmptyLines = true })
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();

                while (csv.ReadNextRecord())
                {
                    RawProduct p = Parse(csv);
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