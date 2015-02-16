using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Models.GS1
{
    // GS1 Company Prefix
    public class GCP
    {
        public static readonly Dictionary<string, string> GS1PrefixList = new Dictionary<string, string>
        {
            // GS1 US
            {"000", "GS1 US"},
            {"001", "GS1 US"},
            {"002", "GS1 US"},
            {"003", "GS1 US"},
            {"004", "GS1 US"},
            {"005", "GS1 US"},
            {"006", "GS1 US"},
            {"007", "GS1 US"},
            {"008", "GS1 US"},
            {"009", "GS1 US"},
            {"010", "GS1 US"},
            {"011", "GS1 US"},
            {"012", "GS1 US"},
            {"013", "GS1 US"},
            {"014", "GS1 US"},
            {"015", "GS1 US"},
            {"016", "GS1 US"},
            {"017", "GS1 US"},
            {"018", "GS1 US"},
            {"019", "GS1 US"},
            // Restricted Distribution
            {"020 - 029", "Restricted distribution"},
            // GS1 US
            {"030 - 039", "GS1 US"}
        };
    }
}
