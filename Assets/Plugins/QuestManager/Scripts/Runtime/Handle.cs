using UnityEngine;

namespace GameLabGraz.QuestManager
{
    public class Handle : MonoBehaviour
    {
        [SerializeField] private QuestManager _questManager;
        [SerializeField] private GameObject _dataObjectRoot;
        [SerializeField] private float _movementMultiplier = 1.0f;

        private Vector3 _iniPosition;
        private Vector3 _iniRootPosition;

        private void Start()
        {
            _iniPosition = transform.position;
            _iniRootPosition = _dataObjectRoot.transform.position;

            _questManager.OnQuestsReset.AddListener(() =>
            {
                transform.position = _iniPosition;
            });

        }

        private void Update()
        {
            _dataObjectRoot.transform.position = _iniRootPosition - (transform.position - _iniPosition) * _movementMultiplier;
        }
    }
}

