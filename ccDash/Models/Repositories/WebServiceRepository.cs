using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using NLog;

namespace ccDash.Models.Repositories
{
    public enum HttpVerb
    {
        Get,
        Post,
        Put,
        Delete
    };

    public class WebServiceRepository
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public string wsCall(string wsURL, HttpVerb verb, object data)
        {
            return wsCall(wsURL, verb, data, new NameValueCollection());
        }

        public string wsCall(string wsURL, HttpVerb verb, object data, NameValueCollection headerData)
        {
            var result = string.Empty;

            HttpResponseMessage jsonData = new HttpResponseMessage();
            ObjectContent _content = null;

            using (var client = new HttpClient())
            {
                if (headerData != null)
                {
                    for (int i = 0; i < headerData.Count; i++)
                    {
                        client.DefaultRequestHeaders.Add(headerData.GetKey(i), headerData.Get(i));
                    }
                }

                var _uri = new Uri(wsURL);

                if (data != null && verb != HttpVerb.Get)
                {
                    _content = new ObjectContent(data.GetType(), data, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                }

                switch (verb)
                {
                    case HttpVerb.Get:
                        jsonData = client.GetAsync(_uri).Result;
                        break;
                    case HttpVerb.Post:
                        jsonData = client.PostAsync(_uri, _content).Result;
                        break;
                    case HttpVerb.Put:
                        jsonData = client.PutAsync(_uri, _content).Result;
                        break;
                    case HttpVerb.Delete:
                        jsonData = client.DeleteAsync(_uri).Result;
                        break;
                    default:
                        jsonData = client.GetAsync(_uri).Result;
                        break;
                }

                var json = jsonData.Content.ReadAsStringAsync().Result;

                //log only if config set
                if (Properties.Settings.Default.LogInfo)
                {
                    var logID = Guid.NewGuid().ToString();

                    logger.Info(string.Format("web service call {0} -- Request: {1}", logID, jsonData.RequestMessage.ToString()));
                    logger.Info(string.Format("web service call {0} -- Response: StatusCode: {1}, Content: {2}", logID, jsonData.StatusCode, json));
                }

                if (!jsonData.IsSuccessStatusCode)
                    throw new HttpException((int)jsonData.StatusCode, json);

                result = json;
            }

            return result;
        }
    }
}