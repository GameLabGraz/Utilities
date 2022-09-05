using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameLabGraz.LimeSurvey
{
    public class LimeSurveyEvents : MonoBehaviour
    {
        public LimeSurveyView view;
        
        public ErrorEvent OnError;
        public WarningEvent OnWarning;
        public UnityEvent OnStartLogin;
        public LoginEvent OnLoggedIn;
        public UnityEvent OnStartLoadQuestions;
        public UnityEvent OnQuestionsLoaded;
        public UnityEvent OnStartSubmission;
        public SubmissionEvent OnSubmissionFinished;

        private int cnt = 0;
        private void Awake()
        {
            if (view)
            {
                view.OnError.AddListener(OnError.Invoke);
                view.OnWarning.AddListener(OnWarning.Invoke);
                view.OnStartLoadQuestions.AddListener(OnStartLoadQuestions.Invoke);
                view.OnQuestionsLoaded.AddListener(OnQuestionsLoaded.Invoke);
                view.OnStartSubmission.AddListener(OnStartSubmission.Invoke);
                OnSubmissionFinished.AddListener(OnSubmissionFinished.Invoke);
            }

            var manager = LimeSurveyManager.Instance;
            if (manager)
            {
                manager.OnError.AddListener(OnError.Invoke);
                manager.OnWarning.AddListener(OnWarning.Invoke);
                manager.OnStartLogin.AddListener(OnStartLogin.Invoke);
                manager.OnLoggedIn.AddListener(OnLoggedIn.Invoke);
            }
        }
    }
}