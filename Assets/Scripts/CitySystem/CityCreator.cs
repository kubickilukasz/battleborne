using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CityCreator : MonoBehaviour
{
    [System.Serializable]
    public class Connection{
        public Transform self;
        public List<Transform> children;
        public float levelCompaction = 1f;
    }

    [System.Serializable]
    public class Building{
        public GameObject prefab;
        public Vector3 offsetRotation;
        public Vector3 offsetPosition;
        public Vector3 size;
    }

    [SerializeField]
    int seed;

    [SerializeField]
    float widthRoad = 5f;

    [SerializeField]
    [Range(0,45)]
    float maxAngle = 15f;

    [SerializeField]
    float compaction = 1f;

    [SerializeField]
    float maxDistance = 30f;

    [SerializeField]
    Transform parentBuidlings;

    [SerializeField]
    List<Building> buidlings = new List<Building>();

    [SerializeField]
    Building way;

    [SerializeField] [HideInInspector]
    List<Transform> transforms;
    [SerializeField] [HideInInspector]
    List<GameObject> createdObjects = new List<GameObject>();
    [SerializeField] [HideInInspector]
    List<Connection> connections = new List<Connection>();
    [SerializeField] [HideInInspector]
    Vector3 referenceVector;
 
    // Start is called before the first frame update
    void Awake()
    {
      // CreateCity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Delete City")]
    void deleteCreatedObjects()
    {
        foreach(GameObject ob in createdObjects)
        {
            if(ob != null)
            {
                DestroyImmediate(ob);
            }
        }
        createdObjects.Clear();
    }

    [ContextMenu("Create City")]
    void CreateCity()
    {

        deleteCreatedObjects();

        Random.InitState(seed);
        Transform [] tempTransforms = GetComponentsInChildren<Transform>();
        transforms = new List<Transform>(tempTransforms);
        if(transforms.Count > 1)
        {
            referenceVector = (transforms[1].position - transforms[0].position).normalized;
            CreateConnections();
            CreateRoads();
        }
    }

    void CreateConnections()
    {
        connections = new List<Connection>();

        for(int i =0; i < transforms.Count;)
        {
            Transform current = transforms[i];
            transforms.Remove(current);      
            connections.Add(CreateConnection(transforms,current));
        }
    }

    Connection CreateConnection(List<Transform> transforms, Transform current)
    {
        List<Transform> children = new List<Transform>();
        foreach(Transform tr in transforms)
        {
            if(Vector3.Distance(tr.position, current.position) < maxDistance && Vector3.Angle(referenceVector, tr.position - current.position) % 45 < maxAngle)
            {
                children.Add(tr);
            }
        }
        Connection connection = new Connection();
        connection.children = children;
        connection.self = current;
        connection.levelCompaction = children.Count;
        return connection;
    }

    void CreateRoads()
    {
        foreach(Connection c in connections)
        {
            foreach(Transform t in c.children)
            {
                float distance = Vector3.Distance(c.self.position, t.position) - (widthRoad/2);
                Vector3 direction = (t.position - c.self.position).normalized;
                Vector3 normalLeft = new Vector3(-direction.z, 0, direction.x);
                Vector3 normalRight = new Vector3(direction.z, 0, -direction.x);
                float angleLeft = Vector3.Angle(Vector3.forward, normalLeft);
                float angleRight = -Vector3.Angle(Vector3.forward, normalRight);
                float buidlingSizeX = 0;
                for(float i = widthRoad / 2; i < distance; i += compaction+buidlingSizeX)
                {
                    Vector3 center = c.self.position + direction * i;
                    
                    buidlingSizeX = SpawnRandomBuilding(center + normalLeft * widthRoad * 0.75f, Quaternion.Euler(0,angleLeft,0), normalLeft).x;
                    buidlingSizeX = Mathf.Max(SpawnRandomBuilding(center + normalRight * widthRoad * 0.75f, Quaternion.Euler(0,angleRight,0), normalRight).x,buidlingSizeX);
                }

                float angleWay = Vector3.Angle(Vector3.forward, direction);
                for(float i = 0; i < distance; i += way.size.z){
                    Vector3 center = c.self.position + direction * i;
                    SpawnWay(center - new Vector3(0f,0.001f * i,0f), Quaternion.Euler(0,angleWay,0), direction);
                }

            }
        }
    }

    Vector3 SpawnRandomBuilding(Vector3 position, Quaternion rotation, Vector3 normal)
    {
        int r = Random.Range(0,buidlings.Count);
        Vector3 offset = buidlings[r].offsetPosition;
        offset.x *= normal.x;
        offset.z *= normal.z;   
        createdObjects.Add(Instantiate(buidlings[r].prefab, position + offset, Quaternion.Euler(rotation.eulerAngles + buidlings[r].offsetRotation), parentBuidlings) as GameObject);
        return buidlings[r].size;
    }

    void SpawnWay(Vector3 position, Quaternion rotation, Vector3 normal){
        Vector3 offset = way.offsetPosition;
        offset.x *= normal.x;
        offset.z *= normal.z; 
        createdObjects.Add(Instantiate(way.prefab, position + offset, rotation, parentBuidlings) as GameObject);
    }

    void OnDrawGizmos()
    {
        foreach(Connection c in connections){
            foreach(Transform t in c.children){
                Gizmos.DrawLine(c.self.position, t.position);
            }
        }

    }

}
