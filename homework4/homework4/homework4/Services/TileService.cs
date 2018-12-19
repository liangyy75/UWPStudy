using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System.Xml.Linq;
using homework4.Model;

namespace homework4.Services
{
    class TileService
    {
        // 定制磁贴
        public static XmlDocument CreateTiles(Memo memo)
        {
            XDocument xDoc = new XDocument(
                new XElement("tile", new XAttribute("version", 3),
                    new XElement("visual",
                        // small tile
                        new XElement("binding"/*, new XAttribute("branding", "TodoList")*/, new XAttribute("displayName", "Memo"), new XAttribute("template", "TileSmall"),
                            new XElement("image", new XAttribute("placement", "background"), new XAttribute("src", memo.imgFile.Path)),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", memo.Time.ToString(), new XAttribute("hint-style", "caption")),
                                    new XElement("text", memo.Title, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", memo.Description, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3))
                                )
                            )
                        ),
                        // medium tile
                        new XElement("binding"/*, new XAttribute("branding", "TodoList")*/, new XAttribute("displayName", "Memo"), new XAttribute("template", "TileMedium"),
                            new XElement("image", new XAttribute("placement", "background"), new XAttribute("src", memo.imgFile.Path)),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", memo.Time.ToString(), new XAttribute("hint-style", "caption")),
                                    new XElement("text", memo.Title, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", memo.Description, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3))
                                )
                            )
                        ),
                        // wide tile
                        new XElement("binding"/*, new XAttribute("branding", "TodoList")*/, new XAttribute("displayName", "Memo"), new XAttribute("template", "TileWide"),
                            new XElement("image", new XAttribute("placement", "background"), new XAttribute("src", memo.imgFile.Path)),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", memo.Time.ToString(), new XAttribute("hint-style", "caption")),
                                    new XElement("text", memo.Title, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", memo.Description, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3))
                                )
                            )
                        ),
                        // large tile
                        new XElement("binding"/*, new XAttribute("branding", "TodoList")*/, new XAttribute("displayName", "Memo"), new XAttribute("template", "TileLarge"),
                            new XElement("image", new XAttribute("placement", "background"), new XAttribute("src", memo.imgFile.Path)),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", memo.Time.ToString(), new XAttribute("hint-style", "caption")),
                                    new XElement("text", memo.Title, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", memo.Description, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3))
                                )
                            )
                        )
                    )
                )
            );
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xDoc.ToString()/*, new XmlLoadSettings {
                ProhibitDtd = false,
                ValidateOnParse = false,
                ElementContentWhiteSpace = false,
                ResolveExternals = false
            }*/);
            return xmlDoc;
        }
    }
}
