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
    List<GameObject> buidlings = new List<GameObject>();

    List<Transform> transforms;
    List<GameObject> createdObjects = new List<GameObject>();
    List<Connection> connections;
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
                for(float i = widthRoad / 2; i < distance; i += compaction)
                {
                    Vector3 center = c.self.position + direction * i;
                    Vector3 normal = new Vector3(-direction.z, 0, direction.x);
                    SpawnRandomBuilding(center + normal * widthRoad / 2, Quaternion.Euler(0,Vector3.Angle(Vector3.right, normal),0));
                    normal = -normal;
                    SpawnRandomBuilding(center + normal * widthRoad / 2, Quaternion.Euler(0,Vector3.Angle(Vector3.right, normal),0));
                }
            }
        }
    }

    void SpawnRandomBuilding(Vector3 position, Quaternion roation)
    {
        int r = Random.Range(0,buidlings.Count);
        createdObjects.Add(Instantiate(buidlings[r], position, roation, parentBuidlings) as GameObject);
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
