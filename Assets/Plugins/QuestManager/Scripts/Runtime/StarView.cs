using System.Collections.Generic;
using UnityEngine;

namespace GameLabGraz.QuestManager
{
    public class StarView : MonoBehaviour
    {
        [SerializeField] protected List<GameObject> stars;
        private void Start()
        {
            foreach (var star in stars)
            {
                star.SetActive(true);
            }
            
            var starCount = 0;
            
            QuestManager.OnStarEarned.AddListener(() =>
            {
                var starRenderer = stars[starCount++].gameObject.GetComponent<Renderer>();
                starRenderer.material.SetColor("_Color", Color.yellow);
            });
        }
    }
}
