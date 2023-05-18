using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace everything_webapi11.Controllers
{
    public class HtmlParserController : ApiController
    {
        public class Result
        {
            public int numresults;
            public List<Dictionary<string, object>> data;
        }

        [HttpGet]
        public IHttpActionResult Search(string html)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            var client = new RestClient($"http://localhost:8432/?search={html}");
            client.Timeout = -1;
            var requestEverything = new RestRequest(Method.GET);
            requestEverything.AddHeader("Authorization", "Basic c2E6MTIzNDU2Nzg=");
            //request.AddHeader("Cookie", "user=id=9130");
            IRestResponse responseEverything = client.Execute(requestEverything);


            var doc = new HtmlDocument();
            doc.LoadHtml(responseEverything.Content);

            var numresults = doc.DocumentNode.SelectSingleNode("//p[@class='numresults']");
            var rows = doc.DocumentNode.SelectNodes("//tr[contains(@class, 'trdata')]");
            string pattern = @"\d{1,3}(,\d{3})*\b(?=\s*(?:结果|个))";
            Match match2 = Regex.Match(numresults.InnerText, pattern);

            var result = new Result();
            //result.numresults = int.Parse(numresults.InnerText.Split(' ')[0]);
            result.numresults = int.Parse(match2.Value.Replace(",",""));
            if (result.numresults > 0)
            {
                result.data = new List<Dictionary<string, object>>();

                foreach (var rowTB in rows)
                {
                    var cells = rowTB.SelectNodes("td");
                    var item = new Dictionary<string, object>();
                    item["name"] = cells[0].InnerText.Trim();
                    item["path"] = cells[1].InnerText.Trim();
                    item["size"] = cells[2].InnerText.Trim();
                    result.data.Add(item);
                }
            }

            // var json = JsonConvert.SerializeObject(result);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            response.Headers.Add("ackCode", "100.1");
            return ResponseMessage(response);
            //return Ok(json);
        }
    }
}
