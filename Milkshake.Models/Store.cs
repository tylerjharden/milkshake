using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Models
{
    public class Store
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LegalName { get; set; }

        public string NAICS { get; set; }
        public string ISIC { get; set; }
        public string DUNS { get; set; }
        public string GlobalLocationNumber { get; set; }
    }
}
