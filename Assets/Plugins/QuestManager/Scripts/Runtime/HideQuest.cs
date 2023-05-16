using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLabGraz.QuestManager
{
    public class HideQuest : MonoBehaviour
    {
        private List<GameObject> _disabledQuests = new List<GameObject>();
        private void OnTriggerEnter(Collider trigger)
        {
            if (CompareTag("AdditionalInformationBody"))
                _disabledQuests.Add(trigger.gameObject);
            
            if(trigger.CompareTag("QuestBody"))
                HideObjects(trigger.gameObject);
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.CompareTag("QuestBody"))
                RevealObjects(trigger.gameObject);
        }

        private void OnDisable()
        {
            foreach (var go in _disabledQuests)
                RevealObjects(go);
        }

        private static void HideObjects(GameObject obj)
        {
            obj.GetComponentInParent<Quest>().IsHidden = true;
            foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
                renderer.enabled = false;
        }

        private static void RevealObjects(GameObject obj)
        {
            var quest = obj.GetComponentInParent<Quest>();
            if (quest != null)
                quest.IsHidden = false;
            foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
                renderer.enabled = true;
        }
    }
}
