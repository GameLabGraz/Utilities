using System;
using GEAR.QuestManager.Data;
using GEAR.QuestManager.Extensions;
using GEAR.QuestManager.NodeGraph;
using GEAR.QuestManager.Reader;
using TMPro;
using UnityEngine;

namespace GEAR.QuestManager
{

    public struct QM_ColliderParam
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
        public Transform parent;
    }

    [ExecuteInEditMode]
    public class QMController : MonoBehaviour
    {
        [SerializeField] private GameObject mainQuestBodyPrefab;
        [SerializeField] private Vector3 MQBodyOffset = new Vector3 (0, -2.85f, 0);
        [SerializeField] private GameObject subQuestBodyPrefab;
        [SerializeField] private Vector3 SQBodyOffset = new Vector3 (0f, -2f, -0.1f);
        [SerializeField] private bool autoPositionQuests = true;

        [Header ("Cover")][SerializeField] private GameObject coverPrefab;
        [SerializeField] private float coverOffset;

        [Header ("Collider")][SerializeField] private GameObject colliderPlaceHolderPrefab;
        [SerializeField] private float colliderOffset = 0.0f;

        [Header ("Initializer")][SerializeField]
        private GameObject initializerPrefab;

        [Header ("Handle")][SerializeField] private bool handleAutoPosition = true;
        [SerializeField] private GameObject handlePrefab;
        [SerializeField] private Vector3 handleOffset;

        [Header ("Quest Data Source")][SerializeField]
        private QMNodeGraph nodeGraph;

        [SerializeField] private TextAsset xmlFile;
        private QMReader _qmReader;

        public float CoverOffset
        {
            get => coverOffset;
            set => coverOffset = value;
        }

        private QuestManager questManager;
        private Vector3 CoverSize => coverPrefab.GetRendererSize();

        private Vector3 coverPosition =>
            coverPrefab.transform.position +
            new Vector3 (0, CoverOffset, 0); //TODO check whether coverPrefab position in necessary

        private void Start ()
        {
            _qmReader = GetComponent<QMReader> ();
        }

        public void GenerateQuestManager ()
        {
            if (questManager)
            {
                DestroyImmediate (questManager.gameObject);
            }

            Debug.Log ("Quest Manager Generated");
            questManager = new GameObject ("QuestManager").AddComponent<QuestManager> ();

            SetupCollider ();

            SetupPlaceHolder ();
        }

        private void SetupCollider ()
        {
            var colliderCollection = new GameObject ("ColliderCollection");
            colliderCollection.transform.parent = questManager.transform;
            GenerateCollider (new QM_ColliderParam
            {
                name = "topCollider",
                    position = coverPosition - new Vector3 (0, CoverSize.y / 2f, 0),
                    rotation = Quaternion.Euler (0, 0, 0),
                    parent = colliderCollection.transform
            });
            GenerateCollider (new QM_ColliderParam
            {
                name = "bottomCollider",
                    position = coverPosition - new Vector3 (0, coverPosition.y - colliderOffset, 0),
                    rotation = Quaternion.Euler (180, 0, 0),
                    parent = colliderCollection.transform
            });
        }

        private void GenerateCollider (QM_ColliderParam colliderParam)
        {
            var qmCollider = Instantiate (colliderPlaceHolderPrefab, colliderParam.parent);
            qmCollider.name = colliderParam.name;
            qmCollider.transform.position = colliderParam.position;
            qmCollider.transform.rotation = colliderParam.rotation;
            qmCollider.transform.localScale = new Vector3 (
                qmCollider.transform.localScale.x,
                qmCollider.transform.localScale.y * CoverSize.y * 100f,
                qmCollider.transform.localScale.z * CoverSize.z);
        }

        private void SetupPlaceHolder ()
        {
            var placeHolder = new GameObject ("PlaceHolder");
            placeHolder.transform.parent = questManager.transform;
            var origin = new GameObject ("Origin");
            origin.transform.parent = placeHolder.transform;
            var initializer = Instantiate (initializerPrefab, origin.transform);

            SetupData (placeHolder);
        }

        private void SetupData (GameObject parent)
        {
            var data = new GameObject ("Data");
            data.transform.parent = parent.transform;

            var cover = Instantiate (coverPrefab, data.transform);
            cover.transform.position = coverPosition;

            SetupHandle (data, cover);
            SetupRoot (data);

        }

        private void SetupHandle (GameObject parent, GameObject cover)
        {
            var handle = Instantiate (handlePrefab, parent.transform);

            handle.transform.position = handleAutoPosition ?
                coverPosition - new Vector3 (0, CoverSize.y * 0.2f, CoverSize.z * 0.7f) :
                handleOffset;
        }

        private void SetupRoot (GameObject parent)
        {
            var root = new GameObject ("Root");
            root.transform.parent = parent.transform;

            if (_qmReader == null)
                _qmReader = new QMNodeGraphReader (nodeGraph);

            var mainQuestInfos = _qmReader.ReadData ();

            //------------------------------
            var mainQuestSize = mainQuestBodyPrefab.GetRendererSize();
            var subQuestSize = subQuestBodyPrefab.GetRendererSize();

            var mainQuestPosition = coverPosition + MQBodyOffset;

            if (autoPositionQuests)
            {
                mainQuestPosition = new Vector3 (
                    coverPosition.x - 0.2f * mainQuestSize.x,
                    coverPosition.y - 0.5f * CoverSize.y - 0.7f * mainQuestSize.y,
                    coverPosition.z - 0.05f * mainQuestSize.z);
            }

            foreach (var mainQuestInfo in mainQuestInfos)
            {
                var mainQuest = InstantiateQuestObject<MainQuest> (
                    mainQuestInfo, mainQuestBodyPrefab, mainQuestPosition, root);

                var subQuestOffsetCount = 0;
                var nextOffset = Vector3.zero;

                foreach (var subQuestInfo in mainQuestInfo.GetSubQuestInfos ())
                {
                    //var subQuest = Instantiate(subQuestBodyPrefab, mainQuest.transform);

                    //var subQuest = InstantiateQuestObject<SubQuest>(subQuestInfo, subQuestBodyPrefab, mainQuestPosition, mainQuest);


                    var subQuestPosition = subQuestOffsetCount * SQBodyOffset;
                    nextOffset = SQBodyOffset * subQuestOffsetCount;

                    if (autoPositionQuests)
                    {
                        subQuestPosition = new Vector3 (
                            mainQuestPosition.x - 0.2f * mainQuestSize.x,
                            mainQuestPosition.y - 0.5f * mainQuestSize.y - 0.7f * subQuestSize.y - 1.2f * subQuestSize.y * subQuestOffsetCount,
                            mainQuestPosition.z - 0.05f * mainQuestSize.z);

                        nextOffset = new Vector3 (
                            coverPosition.x - 0.2f * mainQuestSize.x,
                            subQuestPosition.y - 1.2f * mainQuestSize.y,
                            coverPosition.z - 0.05f * mainQuestSize.z);
                    }

                    var subQuest = InstantiateQuestObject<SubQuest>(
                        subQuestInfo, subQuestBodyPrefab, subQuestPosition, mainQuest);

                    //AddTextComponent(subQuest, subQuestInfo.Name);
                    //subQuest.transform.position = subQuestPosition;

                    // TODO: add all scale as unit
                    subQuest.transform.localScale = Vector3.one;
                    subQuest.transform.localRotation = Quaternion.Euler (Vector3.zero);

                    subQuestOffsetCount++;
                }

                mainQuestPosition = nextOffset;
            }

            //-------------------------------

        }

        private GameObject InstantiateQuestObject<T> (QuestInfo questInfo, GameObject questPrefab, Vector3 position, GameObject parent)
        {
            var questObject = Instantiate (questPrefab, parent.transform);
            questObject.transform.position = position;

            AddTextComponent(questObject, questInfo.Name);
            questObject.AddComponent (typeof (T));
            return questObject;
        }

        private void AddTextComponent (GameObject quest, string text)
        {
            var questSize = quest.GetRendererSize();

            var textObj = new GameObject ("Text");
            textObj.transform.parent = quest.transform;
            textObj.transform.localPosition = Vector3.zero;
            textObj.transform.forward = quest.transform.right;

            var textMeshComp = textObj.AddComponent<TextMeshPro> ();
            textMeshComp.text = text;
            textMeshComp.alignment = TextAlignmentOptions.Left;
            textMeshComp.enableAutoSizing = true;
            textMeshComp.fontSizeMin = 0.0f;

            var rt = textMeshComp.GetComponent<RectTransform> ();
            rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, questSize.z * 0.8f);
            rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, questSize.y * 0.7f);
        }
    }
}