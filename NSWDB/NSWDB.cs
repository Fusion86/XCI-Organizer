using NSWDB.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NSWDB
{
    public class NSWDB
    {
        public IReadOnlyList<object> Releases => _releases.Release.AsReadOnly();

        private ReleasesRoot _releases { get; set; } = new ReleasesRoot();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Local path to NSWreleases.xml</param>
        /// <returns>True if has new entries</returns>
        public void LoadLocal(string path)
        {
            string txt = File.ReadAllText(path);
            LoadFromXmlDocument(txt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if has new entries</returns>
        public async Task LoadRemote(string url = "http://nswdb.com/xml.php")
        {
            string txt = await new HttpClient().GetStringAsync(url);
            LoadFromXmlDocument(txt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns>True if has new entries</returns>
        private void LoadFromXmlDocument(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ReleasesRoot));
            _releases = (ReleasesRoot)serializer.Deserialize(new StringReader(xml));
        }
    }
}
