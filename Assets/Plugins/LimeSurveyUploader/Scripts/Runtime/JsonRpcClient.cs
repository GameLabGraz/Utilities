using System.Collections;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

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

        public IEnumerator Post()
        {
            var jObject = new JObject
            {
                new JProperty("jsonrpc", "2.0"),
                new JProperty("id", ++id),
                new JProperty("method", Method),
                new JProperty("params", Parameters)
            };

            var postData = JsonConvert.SerializeObject(jObject);

            using (var request = UnityWebRequest.Post(URL, string.Empty))
            {
                request.method = UnityWebRequest.kHttpVerbPOST;
                request.downloadHandler = new DownloadHandlerBuffer();
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(postData));
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                {
                    yield return request.error;
                }
                else
                {
                    Response = new JsonRpcResponse();
                    Response = JsonConvert.DeserializeObject<JsonRpcResponse>(request.downloadHandler.text);
                    Response.StatusCode = request.responseCode;
                }
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
        public long StatusCode { set; get; }

        public JsonRpcResponse() { }

        public override string ToString()
        {
            return "{\"id\":" + Id + ",\"result\":\"" + Result + "\",\"error\":" + Error + ((string.IsNullOrEmpty(Error)) ? "null" : "\"" + Error + "\"") + "}";
        }
    }
}