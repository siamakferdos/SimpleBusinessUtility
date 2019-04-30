using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Shoniz.Common.Core;

namespace Shoniz.Common.Data.Xml
{
    public class ConfigurationFile
    {
        private readonly string _filePath;
        public ConfigurationFile()
        {
            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (File.Exists(Path.Combine(assemblyFolder, "Web.Config")))
                _filePath = "Web.Config";
            else if (File.Exists(Path.Combine(assemblyFolder, "app.Config")))
                _filePath = "app.Config";
            else
                _filePath = "";
        }

        /// <summary>
        /// Gets the value of element.
        /// </summary>
        /// <param name="elementSeries">The element series. if you want title value
        ///  It should be like: /book/title </param>
        /// <returns></returns>
        public string GetValueOfElement(string elementSeries)
        {
            var doc = new XmlDocument();
            doc.Load(_filePath);
            if (doc.DocumentElement != null)
            {
                var node = doc.DocumentElement.SelectSingleNode(elementSeries);
                if (node != null) 
                    return node.InnerText;
            }
            return "";
        }

        /// <summary>
        /// Gets the value of element.
        /// </summary>
        /// <param name="elementSeries">The element series. if you want title value
        ///  It should be like: /book/title </param>
        /// <param name="attribute">attrubute of node element</param>
        /// <returns></returns>
        public string GetValueOfElement(string elementSeries, KeyValuePair<string, string> attribute)
        {
            var doc = new XmlDocument();
            doc.Load(_filePath);
            if (doc.DocumentElement != null)
            {
                var node = doc.DocumentElement.SelectSingleNode
                    (
                        string.Format(elementSeries + "[@{0}='{1}']", attribute.Key, attribute.Value)
                    );
                if (node != null) return node.InnerText;
            }
            return "";
        }

        public string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_filePath))
                return "Data Source=FROSH;Initial Catalog=UM;" +
                       "User ID=saleadmin;Password=H2389*x;MultipleActiveResultSets=True;" +
                       "Application Name=EntityFramework";
            return GetValueOfElement
                (
                    "\\configuration\\connectionStrings\\add",
                    new KeyValuePair<string, string>("name", "UMConnectionString")
                );
        }
    }
}
