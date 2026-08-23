using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpiderWebChecker : MonoBehaviour
{
    private List<SpiderWebNode> spiderWebNodes = new List<SpiderWebNode>();
    private List<SpiderWebSystem> spiderWebSystems = new List<SpiderWebSystem>();
    private List<SpiderWebConnection> spiderWebConnections = new List<SpiderWebConnection>();
    private List<SpiderWebNode> newNodes = new List<SpiderWebNode>();
    [Header("Connection Requirements")]
    public float distanceThreshold = 35;
    public float maxSystemAngle = 170;
    //public bool isSystemCurrentlyPlanar;
    [Header("Web Visual Prefabs")]
    public GameObject webSystemVisualPrefab;
    public GameObject webConnectionVisualPrefab;
    public static SpiderWebChecker Instance;

    private float timer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        WaveManager.WaveOver += RemoveAllNodes;
    }

    private void OnDisable()
    {
        WaveManager.WaveOver -= RemoveAllNodes;
    }
    // Systems aren't monobehaviours so supply their update through this
    private void Update()
    {
        timer += Time.deltaTime;
        foreach (SpiderWebSystem system in spiderWebSystems)
        {
            system.UpdateSystem();
        }

        // This was a neat kind of testing method but only works if all nodes are connected as a part of one web & there aren't any faces missed due to the angle exception
        //isSystemCurrentlyPlanar = 2 == spiderWebNodes.Count - spiderWebConnections.Count + spiderWebSystems.Count + 1;

        if (timer > 0.5f && newNodes.Count > 0)
        {
            
            foreach (SpiderWebNode node in newNodes)
            {
                if (node == null || spiderWebNodes.Contains(node)) { continue; }

                spiderWebNodes.Add(node);
                SearchForNewWebSystems(node);
            }
            newNodes.Clear();
            //Debug.Log("Nodes(vertices): " + spiderWebNodes.Count + " Connections(edges): " + spiderWebConnections.Count + " Systems(faces): " + spiderWebSystems.Count);
            CheckGraphForDuplicates();
            timer = 0;
        }
    }
    // Exclusively called via end of wave event
    public void RemoveAllNodes(float wewa)
    {
        //Debug.Log("Remove All Nodes");
        //Debug.Log(spiderWebNodes.Count);
        foreach(SpiderWebNode node in spiderWebNodes.ToList())
        {
            //Debug.Log("Hitting Node");
            node.OnTakeDamage(1000, Color.white, DamageType.Normal);
        }
    }
    // All Node additions MUST occur through this, as its a singleton just call this from anything spawning a node
    public void NewWebNodeAdded(SpiderWebNode node)
    {
        if (node  == null) { return; }
        if (!newNodes.Contains(node) && !spiderWebNodes.Contains(node)) { newNodes.Add(node); }
           
    }
    // All Node removals MUST occur through this to correctly update all systems and connections to prevent random removals and stale data
    public void NodeRemoved(SpiderWebNode node)
    {
        //Debug.Log("Node Removed");
       
        
        CheckForAffectedSystems(node);
        CheckForAffectedConnections(node);
        spiderWebNodes.Remove(node);
        
        // Affected Nodes is now unused

        // List<SpiderWebNode> affectedNodes = CheckForAffectedNodes(node);
        //foreach (SpiderWebNode affectedNode in affectedNodes)
        //{
        //    SearchForNewWebSystems(affectedNode);
        //}
    }

    // When A node is removed find any connections it was a part of and remove them
    private void CheckForAffectedConnections(SpiderWebNode removedNode)
    {
        List<SpiderWebConnection> affectedConnections = new List<SpiderWebConnection>();
        foreach (SpiderWebConnection connection in spiderWebConnections)
        {
            if (connection.NodeA == removedNode || connection.NodeB == removedNode)
            {
                affectedConnections.Add(connection);
            }
        }

        foreach (SpiderWebConnection connection in affectedConnections)
        {
            spiderWebConnections.Remove(connection);
            connection.NodeA.RemovedFromConnection(connection);
            connection.NodeB.RemovedFromConnection(connection);
            //connection.visual.DestroyMe();
        }
    }

    // When A node is removed there can be multiple affected systems, you need to update the node's internal system counter and destroy the relevant visual
    private void CheckForAffectedSystems(SpiderWebNode removedNode)
    {
        List<SpiderWebSystem> affectedSystems = new List<SpiderWebSystem>();
        foreach (SpiderWebSystem system in spiderWebSystems)
        {
            if (system.nodes.Contains(removedNode))
            {
                affectedSystems.Add(system);
            }
        }

        foreach (SpiderWebSystem system in affectedSystems)
        {
            spiderWebSystems.Remove(system);
            system.visual.DestroyMe();
             
            foreach (SpiderWebNode node in system.nodes)
            {
                //node.RemoveFromSystem();
                node.RemoveFromSystem(system);
            }
        }

    }
    // No Longer needed as nodes aren't limited to 1 system, originally if a system broke the other nodes would need prompting to re-evaluate
    private List<SpiderWebNode> CheckForAffectedNodes(SpiderWebNode removedNode)
    {
        HashSet<SpiderWebNode> affectedNodes = new HashSet<SpiderWebNode>();

        foreach (SpiderWebSystem system in spiderWebSystems)
        {
            if (!system.nodes.Contains(removedNode)) { continue; }

            foreach (SpiderWebNode node in system.nodes)
            {
                if (node != removedNode)
                {
                    affectedNodes.Add(node);
                }
            }  
        }

        foreach (SpiderWebNode node in spiderWebNodes)
        {
            if (node == removedNode) { continue; }

            if (AreNodesInRange(node, removedNode))
            {
                affectedNodes.Add(node);
            }
        }

        return affectedNodes.ToList();
    }
    // Where all re-evaluation and system attempts start
    private void SearchForNewWebSystems(SpiderWebNode newNode)
    {
        // If the new node is placed in an existing system you need to split it to avoid overlaps, this is surprisingly easy. Also don't forget to return (totally didn't take me a while to realise I forgot that)
        SpiderWebSystem overlappingSystem = IsNodeInExistingSystem(newNode);
        if (overlappingSystem != null) { SplitExistingSystem(overlappingSystem, newNode); return; }

        // There is no need to perform the whole process on a node that isn't in range to begin with, collect some possible nodes then continue the process
        List<SpiderWebNode> potentialWebNodes = new List<SpiderWebNode>();

        foreach (SpiderWebNode node in spiderWebNodes)
        {
            if (node == newNode) { continue; }

            if (AreNodesInRange(newNode, node))
            {
                potentialWebNodes.Add(node);
            }
        }

        // Cycle through the potential nodes searching for pairs that are also in range of each other to begin the attempt at making a system.
        for (int i = 0; i < potentialWebNodes.Count; i++)
        {
            if (CanConnectionBeMade(newNode, potentialWebNodes[i]))
            {
                GetOrCreateConnection(newNode, potentialWebNodes[i]);
            }

            for (int j = i + 1; j < potentialWebNodes.Count; j++)
            {
                SpiderWebNode nodeA = potentialWebNodes[i];
                SpiderWebNode nodeB = potentialWebNodes[j];

                if (nodeA == null || nodeB == null) { continue; }

                if (AreNodesInRange(nodeA, nodeB))
                {
                    CreateNewWebSystem(newNode, nodeA, nodeB, false);
                    
                    //return;   Was here for when nodes were limited in connections
                }
            }
        }
    }
    // Used to determine when a system needs splitting, makes use of the Barycentric Coordinate check built into every system
    private SpiderWebSystem IsNodeInExistingSystem(SpiderWebNode node)
    {
        foreach (SpiderWebSystem system in spiderWebSystems)
        {
            if (system.CheckTargetInTriangle(node.transform.position))
            {
                return system;
            }
        }
        return null;

    }
    // Take all 4 nodes involved, remove the system in question and create 3 new systems including each combination of nodes, also don't forgot to destroy the visual
    private void SplitExistingSystem(SpiderWebSystem system, SpiderWebNode newNode)
    {
        Debug.Log("Splitting existing System");

        SpiderWebNode A = system.nodes[0];
        SpiderWebNode B = system.nodes[1];
        SpiderWebNode C = system.nodes[2];

        spiderWebSystems.Remove(system);
        system.visual.DestroyMe();

        foreach (SpiderWebNode node in system.nodes)
        {
            node.RemoveFromSystem(system);
        }

        CreateNewWebSystem(A, B, newNode, true);
        CreateNewWebSystem(B, C, newNode, true);
        CreateNewWebSystem(C, A, newNode, true);
    }

    // To get to this point there must be 3 nodes within range of each other, from here there are further checks before a system can be made
    //  - Check if it already exists, we don't need direct duplicates
    //  - Check for the maxiumum angle threshold to prevent linear / stretched triangles
    //  - For each new connection that would be made, ensure it is a valid connection and doesn't intersect any exisitng connections
    // Once all checks are passed the actual system is created, including its visual, as Connections can be shared a function exists to allow re-using of an exisiing connection rather than duplicating
    private void CreateNewWebSystem(SpiderWebNode A, SpiderWebNode B, SpiderWebNode C, bool fromSplit)
    {
        if (DoesSystemAlreadyExist(A, B, C)) { return; }

        if (!fromSplit)
        {
            if (!DoesTriangleHaveValidAngles(A, B, C)) { return; }
        }      

        if (!CanConnectionBeMade(A, B)) { return; }
        if (!CanConnectionBeMade(B, C)) { return; }
        if (!CanConnectionBeMade(C, A)) { return; }

        WebSystemVisual visual = ObjectPoolManager.SpawnObject(webSystemVisualPrefab, Vector3.zero, Quaternion.identity).GetComponent<WebSystemVisual>();
        SpiderWebSystem system = new SpiderWebSystem(A, B, C,
            GetOrCreateConnection(A, B),
            GetOrCreateConnection(B, C),
            GetOrCreateConnection(A, C),         
            visual);

        A.AddToNewSystem(system);
        B.AddToNewSystem(system);
        C.AddToNewSystem(system);

        spiderWebSystems.Add(system);
    }
    // Does what it says, if all 3 new nodes are present in an existing system than that system already exists
    private bool DoesSystemAlreadyExist(SpiderWebNode A, SpiderWebNode B, SpiderWebNode C)
    {
        foreach(SpiderWebSystem system in spiderWebSystems)
        {
            if (system.nodes.Contains(A) && system.nodes.Contains(B) && system.nodes.Contains(C)) { return true; }           
        }
        return false;
    }
    // Search the current list of connections to see if the one needed already exists, otherwise create a new connection
    private SpiderWebConnection GetOrCreateConnection(SpiderWebNode A, SpiderWebNode B)
    {
        foreach (SpiderWebConnection connection in spiderWebConnections)
        {
            if ((connection.NodeA == A && connection.NodeB == B) || (connection.NodeA == B && connection.NodeB == A))
            {
                A.AddedToConnection(connection);
                B.AddedToConnection(connection);
                return connection;
            }
        }
        //WebConnectionVisual visual = ObjectPoolManager.SpawnObject(webConnectionVisualPrefab, Vector3.zero, Quaternion.identity).GetComponent<WebConnectionVisual>();
        SpiderWebConnection newConnection = new SpiderWebConnection(A, B);
        spiderWebConnections.Add(newConnection);
        A.AddedToConnection(newConnection);
        B.AddedToConnection(newConnection);
        return newConnection;
    }
    // Loops through every connection to see if the new one would intersect it in any way
    private bool CanConnectionBeMade(SpiderWebNode A, SpiderWebNode B)
    {
        foreach (SpiderWebConnection connection in spiderWebConnections)
        {
            if (DoConnectionsIntersect(A, B, connection.NodeA, connection.NodeB))
            {
                return false;
            }
        }
        return true;
    }
    // Convert all nodes into their Vector2 positions for checks, also determine if the new Connection shares a Node with another connection, in this case the intersection check would be returning a true intersection
    private bool DoConnectionsIntersect(SpiderWebNode A, SpiderWebNode B, SpiderWebNode connectedA, SpiderWebNode connectedB)
    {
        Vector2 a = new Vector2(A.transform.position.x, A.transform.position.z);
        Vector2 b = new Vector2(B.transform.position.x, B.transform.position.z);
        Vector2 cA = new Vector2(connectedA.transform.position.x, connectedA.transform.position.z);
        Vector2 cB = new Vector2(connectedB.transform.position.x, connectedB.transform.position.z);

        if (!DoLineSegmentsIntersect(a, b, cA, cB))
        {
            return false;
        }

        if (A == connectedA || A == connectedB || B == connectedA || B == connectedB) { return false; }

        return true;
    }
    // 2D Cross product to determine if points c & d are on opposite sides of AB, then determine if points a & b are on opposite sides of CD, if so an intersection has happened
    private bool DoLineSegmentsIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        float ABtoC = Vec2Cross(b - a, c - a);
        float ABtoD = Vec2Cross(b - a, d - a);

        float CDtoA = Vec2Cross(d - c, a - c);
        float CDtoB = Vec2Cross(d - c, b - c);

        bool CDresult = (ABtoC > 0f && ABtoD < 0f) || (ABtoC < 0f && ABtoD > 0f);
        bool ABresult = (CDtoA > 0f &&  CDtoB < 0f) || (CDtoA < 0f && CDtoB > 0f);
    
        return CDresult && ABresult;
    }
    // Apparently this function doesn't exist or I am being dense, but I made my own :3c
    private float Vec2Cross(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }
    // Does what it says, check the node positions against a distance threshold
    private bool AreNodesInRange(SpiderWebNode nodeA, SpiderWebNode nodeB)
    {
        float nodeDist = Vector3.Distance(nodeA.transform.position, nodeB.transform.position);
        return nodeDist <= distanceThreshold;
    }
    // Determine the angle of each edge (Apparently Unity just has a function for that so thats nice), then check they all have angles less than a set max
    private bool DoesTriangleHaveValidAngles(SpiderWebNode A, SpiderWebNode B, SpiderWebNode C)
    {
        Vector2 a = new Vector2(A.transform.position.x, A.transform.position.z);
        Vector2 b = new Vector2(B.transform.position.x, B.transform.position.z);
        Vector2 c = new Vector2(C.transform.position.x, C.transform.position.z);

        float angleA = Vector2.Angle(b - a, c - a);
        float angleB = Vector2.Angle(a - b, c - b);
        float angleC = Vector2.Angle(a - c, b - c);

        return angleA <= maxSystemAngle && angleB <= maxSystemAngle && angleC <= maxSystemAngle;

    }
    // public function for external checks against a system's area, currently unused
    public bool IsPointInsideASystem(Vector2 point)
    {
        foreach (SpiderWebSystem system in spiderWebSystems)
        {
            if (system.CheckTargetInTriangle(point))
            {
                return true;
            }
        }
        return false;
    }

    private void CheckGraphForDuplicates()
    {
        for (int i = 0; i < spiderWebNodes.Count; i++)
        {
            for (int j = i + 1; j < spiderWebNodes.Count; j++)
            {
                if (spiderWebNodes[i] == spiderWebNodes[j])
                {
                    Debug.LogWarning("DUPLICATE NODE IN GRAPH");
                }
            }
        }

        for (int i = 0;i < spiderWebConnections.Count; i++)
        {
            for (int j = i + 1;j < spiderWebConnections.Count; j++)
            {
                if ((spiderWebConnections[i].NodeA == spiderWebConnections[j].NodeA && spiderWebConnections[i].NodeB == spiderWebConnections[j].NodeB) || (spiderWebConnections[i].NodeA == spiderWebConnections[j].NodeB && spiderWebConnections[i].NodeB == spiderWebConnections[j].NodeA))
                {
                    Debug.LogWarning("DUPLICATE CONNECTION IN GRAPH");
                }
            }
        }
    }
}
