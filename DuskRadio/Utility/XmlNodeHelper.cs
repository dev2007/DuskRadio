using DuskRadio.Models.Play;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;

namespace DuskRadio.Utility
{
    class XmlNodeHelper
    {
        private static XmlDocument document = null;
        private static IList<RadioNode> playNodeList = null;

        public static Task<IList<RadioNode>> GetNodeList(string fileName)
        {
            return Task.Run(async () =>
            {
                if (playNodeList != null)
                {
                    return playNodeList;
                }

                playNodeList = new List<RadioNode>();
                try
                {
                    StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                    StorageFolder assets = await appInstalledFolder.GetFolderAsync("Assets");
                    var file = await assets.GetFileAsync(fileName);
                    string xmlContent = await FileIO.ReadTextAsync(file);

                    document = new XmlDocument();
                    document.LoadXml(xmlContent);
                    XmlNodeList nodeList = document.ChildNodes;
                    foreach (XmlNode node in nodeList[1].ChildNodes)
                    {
                        var playNode = GetNode(node);
                        if (playNode != null)
                        {
                            playNodeList.Add(playNode);
                        }
                    }
                }
                catch (Exception e)
                {

                }

                return playNodeList;
            });
        }

        private static RadioNode GetNode(XmlNode xmlNode)
        {
            RadioNode node = new RadioNode();
            if (xmlNode == null)
                return null;

            XmlElement element = (XmlElement)xmlNode;
            try
            {
                XmlNodeList list = element.ChildNodes;
                if (list.Count < 2)
                    return null;

                node.Label = list[0].InnerText;
                node.PlayURL = list[1].InnerText;
            }
            catch (Exception e)
            {

            }
            return node;
        }
    }
}
