using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace EksamenM2E2017.Services
{
    public class ApiAccess
    {
        private string url;

        public ApiAccess()
        {
            url = @"https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro=&explaintext=&titles=";
        }

        public string GetSummary(string ingredientName)
        {
            string summary = "";
            using (WebClient client = new WebClient())
            {
                string content = client.DownloadString(url + ingredientName);

                var res = JObject.Parse(content);

                var entitites = (JObject)res["query"]["pages"];

                var entity = entitites.Properties().First();

                var pageValues = (JObject)entity.Value;

                var pageValue = pageValues["extract"];

                summary += pageValue;
            }
            return summary;
        }
    }
}
