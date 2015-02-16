using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MilkshakeTester
{
    public static class XmlReaderExtensions
    {
        // Stream product elements in LinkShare/CJ Catalog XML to an XElement that we can enumerate
        public static IEnumerable<XElement> Products(this XmlReader source)
        {
            while (source.Read())
            {
                source.MoveToContent();

                if (source.NodeType == XmlNodeType.Element && source.Name == "product")
                {
                    yield return (XElement)XElement.ReadFrom(source);
                }
            }
        }

        public static IEnumerable<XElement> Header(this XmlReader source)
        {
            while (source.Read())
            {
                source.MoveToContent();

                if (source.NodeType == XmlNodeType.Element && source.Name == "header")
                {
                    yield return (XElement)XElement.ReadFrom(source);
                }
                if (source.EOF)
                {
                    yield break;
                }
            }
        }

        public static IEnumerable<XElement> Trailer(this XmlReader source)
        {
            while (source.Read())
            {
                source.MoveToContent();

                if (source.NodeType == XmlNodeType.Element && source.Name == "trailer")
                {
                    yield return (XElement)XElement.ReadFrom(source);
                }
                if (source.EOF)
                {
                    yield break;
                }
            }
        }
    }
}
