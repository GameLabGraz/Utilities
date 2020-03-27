using GEAR.QuestManager;
using UnityEngine;

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
    [SerializeField] private TextAsset streamingAsset;
    [SerializeField] private GameObject MainQuestBody;
    [SerializeField] private Vector3 MQBodyiniOffset = new Vector3 (0, -5, 0);
    [SerializeField] private Vector3 MQBodyOffset = new Vector3 (0, -2.85f, 0);
    [SerializeField] private GameObject SubQuestBody;
    [SerializeField] private Vector3 SQBodyOffset = new Vector3 (0f, -2f, -0.1f);
    [SerializeField] private GameObject DataObjectRoot;
    [SerializeField] private GameObject Cover;
    [SerializeField] private GameObject ColliderPlaceHolder;
    [SerializeField] private float _colliderOffset = 0.0f;

    private QuestManager questManager;
    private Vector3 coverSize => Cover.GetComponent<MeshRenderer> ().bounds.size;
    private Vector3 coverPosition => Cover.transform.position;

    public void GenerateQM ()
    {
        if (questManager)
        {
            DestroyImmediate (questManager.gameObject);
        }

        Debug.Log ("Quest Manager Generated");
        questManager = new GameObject ("QuestManager").AddComponent<QuestManager> ();

        var colliderCollection = new GameObject ("ColliderCollection");
        colliderCollection.transform.parent = questManager.transform;
        SetupCollider (new QM_ColliderParam()
        {
            name = "topCollider",
            position = Cover.transform.position - new Vector3(0, coverSize.y / 2f, 0),
            rotation = Quaternion.Euler(0,0,0),
            parent = colliderCollection.transform
        });
        SetupCollider(new QM_ColliderParam()
        {
            name = "bottomCollider",
            position = Cover.transform.position - new Vector3(0, coverPosition.y - _colliderOffset, 0),
            rotation = Quaternion.Euler(180, 0, 0),
            parent = colliderCollection.transform
        });

        var placeHolder = new GameObject ("PlaceHolder");
        placeHolder.transform.parent = questManager.transform;

    }

    private void SetupCollider (QM_ColliderParam colliderParam)
    {
        var qmCollider = Instantiate (ColliderPlaceHolder, colliderParam.parent);
        qmCollider.name = colliderParam.name;
        qmCollider.transform.position = colliderParam.position;
        qmCollider.transform.rotation = colliderParam.rotation;
        qmCollider.transform.localScale = new Vector3 (
            qmCollider.transform.localScale.x,
            qmCollider.transform.localScale.y * coverSize.y * 100f,
            qmCollider.transform.localScale.z * coverSize.z);
    }

}