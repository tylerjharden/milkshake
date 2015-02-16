using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MilkshakeTester
{
    public abstract class XMLCatalogParser : CatalogParser
    {                
        public XMLCatalogParser()
        {
            _format = "*.xml";            
        }
        
        public override void ParseFile(string filename)
        {
            Console.Write("Found " + _name + " Catalog: " + filename + "...");

            using (XmlSanitizingStream clean = new XmlSanitizingStream(File.OpenRead(filename)))
            {
                using (XmlReader reader = XmlReader.Create(clean, new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore }))
                {
                    Parallel.ForEach(reader.Products(), node =>
                    {
                        RawProduct p = Parse(node);
                        AddProduct(p);
                    });

                    Console.WriteLine("done.");
                }
            }

            try
            {
                File.Move(filename, filename.Replace(".xml", ".done"));
            }
            catch { }
        }

        public abstract RawProduct Parse(XElement node);
    }
}
