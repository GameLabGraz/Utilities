using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimeSurveyEventDebugger : MonoBehaviour
{
    public void OnError(string error, string errorDetail)
    {
        Debug.Log($"[LimeSurveyEvent] OnError: '{error}' (detail: '{errorDetail}')");
    }

    public void OnWarning(string warning)
    {
        Debug.Log($"[LimeSurveyEvent] OnWarning: '{warning}'");
    }

    public void OnStartLogin()
    {
        Debug.Log("[LimeSurveyEvent] OnStartLogin");
    }
    
    public void OnLoggedIn(string sessionKey)
    {
        Debug.Log($"[LimeSurveyEvent] OnLoggedIn: sessionKey='{sessionKey}'");
    }
    
    public void OnStartLoadQuestions()
    {
        Debug.Log("[LimeSurveyEvent] OnStartLoadQuestions");
    }
    
    public void OnQuestionsLoaded()
    {
        Debug.Log("[LimeSurveyEvent] OnQuestionsLoaded");
    }
    
    public void OnStartSubmission()
    {
        Debug.Log("[LimeSurveyEvent] OnStartSubmission");
    }
    
    public void OnSubmissionFinished(int responseID)
    {
        Debug.Log($"[LimeSurveyEvent] OnSubmissionFinished: responseID='{responseID}'");
    }
}
