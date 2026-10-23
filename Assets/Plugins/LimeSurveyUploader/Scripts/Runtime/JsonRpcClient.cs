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
            // Never reuse an earlier successful response after a failed request.
            Response = new JsonRpcResponse();
            var jObject = new JObject
            {
                new JProperty("jsonrpc", "2.0"),
                new JProperty("id", ++id),
                new JProperty("method", Method),
                new JProperty("params", Parameters)
            };

            var postData = JsonConvert.SerializeObject(jObject);

#if UNITY_2022_1_OR_NEWER
            using (var request = UnityWebRequest.PostWwwForm(URL, string.Empty))
#else
            using (var request = UnityWebRequest.Post(URL, string.Empty))
#endif
            {
                request.method = UnityWebRequest.kHttpVerbPOST;
                request.downloadHandler = new DownloadHandlerBuffer();
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(postData));
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                Response.StatusCode = request.responseCode;
                if (request.isNetworkError || request.isHttpError)
                {
                    Response.Error = request.error;
                    yield break;
                }
                else
                {
                    try
                    {
                        Response = JsonConvert.DeserializeObject<JsonRpcResponse>(request.downloadHandler.text)
                            ?? new JsonRpcResponse { Error = "Empty JSON-RPC response." };
                    }
                    catch (JsonException)
                    {
                        Response = new JsonRpcResponse { Error = "Invalid JSON-RPC response." };
                    }
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
        public JToken Error { set; get; }
        public long StatusCode { set; get; }

        public JsonRpcResponse() { }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}