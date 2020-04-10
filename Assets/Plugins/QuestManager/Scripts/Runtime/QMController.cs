using GEAR.QuestManager.NodeGraph;
using GEAR.QuestManager.Reader;
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
        private Vector3 coverSize => coverPrefab.GetComponent<MeshRenderer> ().bounds.size;

        private Vector3 coverPosition =>
            coverPrefab.transform.position +
            new Vector3 (0, CoverOffset, 0); //TODO check whether coverPrefab position in necessary

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
                    position = coverPosition - new Vector3 (0, coverSize.y / 2f, 0),
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
                qmCollider.transform.localScale.y * coverSize.y * 100f,
                qmCollider.transform.localScale.z * coverSize.z);
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
                coverPosition - new Vector3 (0, coverSize.y * 0.2f, coverSize.z * 0.7f) :
                handleOffset;
        }

        private void SetupRoot (GameObject parent)
        {
            var root = new GameObject ("Root");
            root.transform.parent = parent.transform;

            var reader = gameObject.AddComponent<QMNodeGraphReader>();
            reader.Root = root;
            reader.NodeGraph = nodeGraph;
            reader.MainQuestPrefab = mainQuestBodyPrefab;
            reader.MainQuestIniOffset = coverPosition;
            reader.MainQuestBodyOffset = MQBodyOffset;
            reader.SubQuestPrefab = subQuestBodyPrefab;
            reader.SubQuestBodyOffset = SQBodyOffset;

            reader.ReadData ();

        }
    }
}