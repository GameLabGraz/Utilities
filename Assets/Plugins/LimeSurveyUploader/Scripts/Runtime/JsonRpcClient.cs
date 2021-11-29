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
        /// <summary>
        /// Set JSON-RPC webservice URL
        /// </summary>
        public string URL { set; get; }
        /// <summary>
        /// Set JSON-RPC method
        /// </summary>
        public string Method { set; get; }
        /// <summary>
        /// Add JSON-RPC params
        /// </summary>
        public JObject Parameters { set; get; }

        /// <summary>
        /// Results of the request
        /// </summary>
        public JsonRpcResponse Response { set; get; }

        /// <summary>
        /// Create a new object of RPCclient 
        /// </summary>
        public JsonRpcClient()
        {
            Parameters = new JObject();
            Response = null;
        }

        /// <summary>
        /// Create a new object of RPCclient
        /// </summary>
        /// <param name="URL"></param>
        public JsonRpcClient(string URL)
        {
            this.URL = URL;
            Parameters = new JObject();
            Response = null;
        }

        /// <summary>
        /// POST the request and returns server response
        /// </summary>
        /// <returns></returns>
        public string Post()
        {
            try
            {
                var jobject = new JObject();
                jobject.Add(new JProperty("jsonrpc", "2.0"));
                jobject.Add(new JProperty("id", ++id));
                jobject.Add(new JProperty("method", Method));
                jobject.Add(new JProperty("params", Parameters));

                var PostData = JsonConvert.SerializeObject(jobject);
                var encoding = new UTF8Encoding();
                var bytes = encoding.GetBytes(PostData);

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
            this.Parameters = new JObject();
        }


    }

    public class JsonRpcResponse
    {
        public int id { set; get; }
        public object result { set; get; }
        public string error { set; get; }
        public HttpStatusCode StatusCode { set; get; }

        public JsonRpcResponse() { }

        public override string ToString()
        {
            return "{\"id\":" + id.ToString() + ",\"result\":\"" + result.ToString() + "\",\"error\":" + error + ((String.IsNullOrEmpty(error)) ? "null" : "\"" + error + "\"") + "}";
        }
    }

}