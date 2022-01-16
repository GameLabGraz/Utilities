using System;
using System.IO;
using System.Net;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;

namespace GameLabGraz.LimeSurvey
{
    public class JsonRpcClient
    {
        private int id = 0;
        public string URL { set; get; }
        public string Method { set; get; }
        public JObject Parameters { set; get; }
        public JsonRpcResponse Response { set; get; }

        public JsonRpcClient()
        {
            Parameters = new JObject();
            Response = null;
        }

        public JsonRpcClient(string URL)
        {
            this.URL = URL;
            Parameters = new JObject();
            Response = null;
        }

        public string Post()
        {
            try
            {
                var jobject = new JObject();
                jobject.Add(new JProperty("jsonrpc", "2.0"));
                jobject.Add(new JProperty("id", ++id));
                jobject.Add(new JProperty("method", Method));
                jobject.Add(new JProperty("params", Parameters));

                var postData = JsonConvert.SerializeObject(jobject);
                var encoding = new UTF8Encoding();
                var bytes = encoding.GetBytes(postData);

                var request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.KeepAlive = true;
                request.ContentLength = bytes.Length;

                var writeStream = request.GetRequestStream();
                writeStream.Write(bytes, 0, bytes.Length);
                writeStream.Close();

                var response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                var readStream = new StreamReader(responseStream, Encoding.UTF8);

                Response = new JsonRpcResponse();
                Response = JsonConvert.DeserializeObject<JsonRpcResponse>(readStream.ReadToEnd());
                Response.StatusCode = response.StatusCode;

                return Response.ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public void ClearParameters()
        {
            Parameters = new JObject();
        }
    }

    public class JsonRpcResponse
    {
        public int Id { set; get; }
        public object Result { set; get; }
        public string Error { set; get; }
        public HttpStatusCode StatusCode { set; get; }

        public JsonRpcResponse() { }

        public override string ToString()
        {
            return "{\"id\":" + Id + ",\"result\":\"" + Result + "\",\"error\":" + Error + ((string.IsNullOrEmpty(Error)) ? "null" : "\"" + Error + "\"") + "}";
        }
    }
}