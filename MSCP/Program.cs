using Milkshake;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCP
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please drag a taxonomy TXT file delimited by '>' and seperated by newlines on this program to parse.");
                Console.Read();
                return;
            }

            List<Category> categories = new List<Category>();
            foreach (string cat in File.ReadAllLines(args[0]))
            {
                // category hierarchy line                
                if (cat.Contains(">")) // multi-level hierarchy
                {
                    List<string> cats = cat.Split('>').ToList();

                    Category parent = null;
                    foreach (string cc in cats)
                    {
                        string s = cc.Trim();
                        Category c = new Category();
                        
                        if (Category.Exists(s))
                        {
                            c = Category.GetCategory(s);
                            parent = c;
                            Console.WriteLine("Found existing root category: " + c.Name);
                            continue;
                        }
                        else
                        {
                            c.Id = Guid.NewGuid();
                            c.Name = s;

                            if (parent == null) // first category in order is root
                            {
                                c.IsRootCategory = true;
                                c.ParentId = Guid.Empty;
                                parent = c;
                                Category.Create(c);
                                Console.WriteLine("Created root category: " + c.Name);
                                continue;
                            }
                            else
                            {
                                c.ParentId = parent.Id;
                                c.IsRootCategory = false;
                                parent = c;
                                Category.Create(c);
                                Console.WriteLine("Created child category: " + c.Name + " of " + parent.Name);
                                continue;
                            }
                        }
                    }                    
                }
                // Potentially Root Category
                else
                {
                    string s = cat.Trim();
                    Category c = new Category();                    
                    if (Category.Exists(s))
                    {                       
                        continue; // do nothing, no children to add
                    }
                    else
                    {
                        c.Id = Guid.NewGuid();
                        c.Name = s;
                        c.IsRootCategory = true;
                        c.ParentId = Guid.Empty;
                        Category.Create(c);
                        Console.WriteLine("Created one-line root category: " + c.Name);
                    }
                }
            }
        }
    }
}
