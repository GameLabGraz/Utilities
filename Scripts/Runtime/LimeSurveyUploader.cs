using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using GEAR.LimeSurvey.Extensions;
using GEAR.Gadgets.Coroutine;
using Debug = UnityEngine.Debug;

namespace GEAR.LimeSurvey
{
    public class LimeSurveyUploader : MonoBehaviour
    {
        [Header("Login")] [SerializeField] private string user;

        [SerializeField] private string password;

        [SerializeField] private string surveyId;

        [Header("Upload Settings")]

        [SerializeField] private bool excludeRecordIds = true;

        [SerializeField] private LimeSurveyInsertIdType insert = LimeSurveyInsertIdType.Ignore;

        [SerializeField] private bool importAsNotFinalized;

        [SerializeField] private LimeSurveyCharset charset = LimeSurveyCharset.Utf8;

        private string LoginUri => "http://wlvi.iicm.edu/limesurvey/index.php/admin/authentication/sa/login";

        private string UploadUri => $"http://wlvi.iicm.edu/limesurvey/index.php/admin/dataentry/sa/vvimport/surveyid/{surveyId}";

        private readonly string[] _errorMessages =
        {
            "Incorrect username and/or password!",
            "You have exceeded the number of maximum login attempts.",
            "Please log in first."
        };

        public bool LoggedIn { get; private set; }

        private void OnEnable()
        {
            StartCoroutine(Login());
        }

        private void OnDisable()
        {
            UnityWebRequest.ClearCookieCache();
        }

        public void StartUpload(TextAsset data)

        {
            StartCoroutine(UploadData(data.bytes));
        }

        public void StartUpload(byte[] data)

    {
            StartCoroutine(UploadData(data));
        }

        private IEnumerator Login()
        {
            Debug.Log("LimeSurveyUploader::Login: Start Login ...");

            var form = new WWWForm();
            form.AddField(LimeSurveyField.User, user);
            form.AddField(LimeSurveyField.Password, password);
            form.AddField(LimeSurveyField.Language, LimeSurveyLanguage.Default);
            form.AddField(LimeSurveyField.Action, LimeSurveyAction.Login);

            var cd = new CoroutineWithData(this, SendData(LoginUri, form));
            yield return cd.Coroutine;

            var error = _errorMessages.FirstOrDefault(e => ((string)cd.Result).Contains(e));
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"LimeSurveyUploader::Login: {error}");
                LoggedIn = false;
            }
            else
            {
                Debug.Log($"LimeSurveyUploader::Login: Success");
                LoggedIn = true;
            }
        }

        private IEnumerator UploadData(byte[] data)
        {
            if (LoggedIn)
            {
                Debug.Log("LimeSurveyUploader::UploadData: Start Uploading data ...");

                var form = new WWWForm();
                form.AddField(LimeSurveyField.Action, LimeSurveyAction.Import);
                form.AddField(LimeSurveyField.SubAction, LimeSurveyAction.Upload);
                form.AddField(LimeSurveyField.Sid, surveyId);

                form.AddField(LimeSurveyField.File, "file");

                if(excludeRecordIds)
                    form.AddField(LimeSurveyField.NoId, LimeSurveyInsertIdType.NoId);
                else
                    form.AddField(LimeSurveyField.Insert, insert);
                
                if(importAsNotFinalized)
                    form.AddField(LimeSurveyField.Finalized, "notfinalized");

                form.AddField(LimeSurveyField.Charset, charset);

                form.AddBinaryData(LimeSurveyField.File, data);

                var cd = new CoroutineWithData(this, SendData(UploadUri, form));
                yield return cd.Coroutine;

                var error = _errorMessages.FirstOrDefault(e => ((string) cd.Result).Contains(e));
                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError($"LimeSurveyUploader::UploadData: {error}");
                }
                else
                {
                    Debug.Log("LimeSurveyUploader::UploadData: Success");
                }
            }
            else
            {
                Debug.Log("LimeSurveyUploader:UploadData: You are not logged in");
            }
        }

        private IEnumerator SendData(string uri, WWWForm form)
        {
            using (var w = UnityWebRequest.Post(uri, form))
            {
                yield return w.SendWebRequest();
                if (w.isNetworkError || w.isHttpError)
                {
                    Debug.LogError($"LimeSurveyUploader::SendData: Error: {w.error}");
                    yield return w.error;
                }
                else
                {
                    Debug.Log("LimeSurveyUploader::SendData: Sent data successfully");
                    yield return w.downloadHandler.text;
                }
            }
        }
    }
}
