using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpiderWebChecker : MonoBehaviour
{
    private List<SpiderWebNode> spiderWebNodes = new List<SpiderWebNode>();
    private List<SpiderWebSystem> spiderWebSystems = new List<SpiderWebSystem>();
    public float distanceThreshold = 100;
    public GameObject webSystemVisualPrefab;
    public static SpiderWebChecker Instance;
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

    private void Update()
    {
        foreach (SpiderWebSystem system in spiderWebSystems)
        {
            system.UpdateSystem();
        }
    }

    public void RemoveAllNodes(float wewa)
    {
        Debug.Log("Remove All Nodes");
        Debug.Log(spiderWebNodes.Count);
        foreach(SpiderWebNode node in spiderWebNodes.ToList())
        {
            Debug.Log("Hitting Node");
            node.OnTakeDamage(1000, Color.white, DamageType.Normal);
        }
    }

    public void NewWebNodeAdded(SpiderWebNode node)
    {
        spiderWebNodes.Add(node);

        SearchForNewWebSystems(node);
    }

    public void NodeRemoved(SpiderWebNode node)
    {
        Debug.Log("Node Removed");
        List<SpiderWebNode> affectedNodes = CheckForAffectedNodes(node);
        CheckForAffectedSystems(node);
        spiderWebNodes.Remove(node);
        foreach (SpiderWebNode affectedNode in affectedNodes)
        {
            SearchForNewWebSystems(affectedNode);
        }
    }

    public void CheckForAffectedSystems(SpiderWebNode removedNode)
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

    public List<SpiderWebNode> CheckForAffectedNodes(SpiderWebNode removedNode)
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

    public void SearchForNewWebSystems(SpiderWebNode newNode)
    {
        List<SpiderWebNode> potentialWebNodes = new List<SpiderWebNode>();

        foreach (SpiderWebNode node in spiderWebNodes)
        {
            if (node == newNode) { continue; }

            if (AreNodesInRange(newNode, node))
            {
                potentialWebNodes.Add(node);
            }
        }

        for (int i = 0; i < potentialWebNodes.Count; i++)
        {
            for (int j = i + 1; j < potentialWebNodes.Count; j++)
            {
                SpiderWebNode nodeA = potentialWebNodes[i];
                SpiderWebNode nodeB = potentialWebNodes[j];

                if (nodeA == null || nodeB == null) { continue; }

                if (AreNodesInRange(nodeA, nodeB))
                {
                    CreateNewWebSystem(newNode, nodeA, nodeB);
                    //return;
                }
            }
        }
    }

    public void CreateNewWebSystem(SpiderWebNode A, SpiderWebNode B, SpiderWebNode C)
    {
        if (DoesSystemAlreadyExist(A, B, C)) { return; }

        WebSystemVisual visual = ObjectPoolManager.SpawnObject(webSystemVisualPrefab, Vector3.zero, Quaternion.identity).GetComponent<WebSystemVisual>();
        SpiderWebSystem system = new SpiderWebSystem(A, B, C, visual);

        A.AddToNewSystem(system);
        B.AddToNewSystem(system);
        C.AddToNewSystem(system);

        spiderWebSystems.Add(system);
    }

    public bool DoesSystemAlreadyExist(SpiderWebNode A, SpiderWebNode B, SpiderWebNode C)
    {
        foreach(SpiderWebSystem system in spiderWebSystems)
        {
            if (system.nodes.Contains(A) && system.nodes.Contains(B) && system.nodes.Contains(C)) { return true; }           
        }
        return false;
    }

    public bool AreNodesInRange(SpiderWebNode nodeA, SpiderWebNode nodeB)
    {
        float nodeDist = Vector3.Distance(nodeA.transform.position, nodeB.transform.position);
        return nodeDist <= distanceThreshold;
    }
}
