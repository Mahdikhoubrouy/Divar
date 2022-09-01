using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DivarTools.Scraper
{
    public class HTMLParser
    {
        public List<string> ParsAndGetPostCode(string source)
        {
            var html = new HtmlDocument();
            html.LoadHtml(source);
            var AllPost = html.DocumentNode.SelectNodes("//*[@id=\"app\"]/div[1]/main/div/div/div/div[*]");

            var PostCode = new List<string>();
            foreach (var Post in AllPost)
            {
                PostCode.Add(Post.FirstChild.Attributes["href"].Value.Split("/")[3]);
            }
            return PostCode;
        }
    }
}
