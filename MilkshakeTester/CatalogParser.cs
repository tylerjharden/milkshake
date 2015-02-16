using Newtonsoft.Json;
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
    public abstract class CatalogParser
    {
        protected static string _name;
        protected static string _folder;

        public static string _format = "*.catalog";
        public static string _doneFormat = "*.done";

        public static string _storeName;
        public static string _storeId;

        public virtual void Reset()
        {
            foreach (string f in Directory.GetFiles("C:\\milkshake\\" + _folder + "\\", _doneFormat))
            {
                File.Move(f, f.Replace(_doneFormat, _format));
            }
        }

        public virtual void ParseFile(string filename)
        {
            Console.Write("Found " + _name + " Catalog: " + filename + "...");

            // Null parser, overridden in implementations
            // Do nothing.
            
            try
            {
                File.Move(filename, filename.Replace(_format, _doneFormat));
            }
            catch { }                        
        }

        public virtual void ParseAll()
        {            
            foreach (string f in Directory.GetFiles("C:\\milkshake\\" + _folder + "\\", _format))
            {                
                ParseFile(f);
            }
        }
                        
        protected internal bool AddProduct(RawProduct p)
        {
            try
            {
                dynamic json = JsonConvert.SerializeObject(p);

                Redis.PushJSON(json);

                return true;
            }
            catch { return false; }
        }
                
        // TODO: Automate with file system watcher
    }
}