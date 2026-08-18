using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderWebSystem
{
    private Vector2 A, B, C;
    public List<SpiderWebNode> nodes = new List<SpiderWebNode>();

    public SpiderWebConnection AB;
    public SpiderWebConnection BC;
    public SpiderWebConnection CA;

    private Vector3 centrePoint;
    private float broadSystemRadius;
    public WebSystemVisual visual;

    public SpiderWebSystem(SpiderWebNode nodeA, SpiderWebNode nodeB, SpiderWebNode nodeC, SpiderWebConnection newAB, SpiderWebConnection newBC, SpiderWebConnection newCA, WebSystemVisual visual)
    {
        // Debug.Log("I AHVE BEEN CREATED WE HAVE A SYSTEM YUPPIUE");

        // Store Nodes
        nodes.Add(nodeA); nodes.Add(nodeB); nodes.Add(nodeC);

        // Store Node 2D Positions
        A = new Vector2(nodeA.transform.position.x, nodeA.transform.position.z);
        B = new Vector2(nodeB.transform.position.x, nodeB.transform.position.z);
        C = new Vector2(nodeC.transform.position.x, nodeC.transform.position.z);

        // Store Node Connections
        AB = newAB;
        BC = newBC;
        CA = newCA;

        // Determine check values including broad phase radius and origin
        broadSystemRadius = DetermineLongestEdge();
        centrePoint = FindCentrePoint();

        // Store and Setup Visual
        this.visual = visual;
        visual.transform.position = centrePoint;
        visual.SetVisualsByPosition(nodeA.transform, nodeB.transform, nodeC.transform);
    }

    public void UpdateSystem()
    {
        CheckForTargetsInSystem();

        // Re-Enable for Debug Drawing
        //Debug.DrawLine(nodes[0].transform.position, nodes[1].transform.position, Color.white);
        //Debug.DrawLine(nodes[1].transform.position, nodes[2].transform.position, Color.white);
        //Debug.DrawLine(nodes[2].transform.position, nodes[0].transform.position, Color.white);
    }
    // Does what it says, compare all the distances between ndoes
    public float DetermineLongestEdge()
    {
        float currentLongest = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = i + 1; j < nodes.Count; j++)
            {
                float dist = Vector3.Distance(nodes[i].transform.position, nodes[j].transform.position);
                if (dist > currentLongest)
                {
                    currentLongest = dist;
                }
            }
        }
        return currentLongest;
    }

    public Vector3 FindCentrePoint()
    {
        return ((nodes[0].transform.position + nodes[1].transform.position + nodes[2].transform.position) / 3);
    }
    // Perform a broad phase check using the triangle centre point a radius of longest edge
    public void CheckForTargetsInSystem()
    {
        Collider[] colliders = Physics.OverlapSphere(centrePoint, broadSystemRadius, nodes[0].hostileMask);
        List<Entity> hitEntities = new List<Entity>();
        foreach (Collider collider in colliders)
        {
            if (!collider.gameObject) { continue; }
            if (CheckTargetInTriangle(collider.gameObject.transform.position))
            {
                if (collider.TryGetComponent<Entity>(out Entity entity))
                hitEntities.Add(entity);
            }
            
        }
        ProcessHitTargetsInSystem(hitEntities);
    }
    // If the target is already afflicted by slow, refresh its duration, if not apply a new slow
    public void ProcessHitTargetsInSystem(List<Entity> hitEntities)
    {
        foreach (Entity hitEntity in hitEntities)
        {
            if (hitEntity.statusSystem.CheckForStatusByType(StatusType.Slow))
            {
                hitEntity.statusSystem.ResetStatusByType(StatusType.Slow);
            }
            else
            {
                //Debug.Log("Target Within Web");
                hitEntity.OnRecieveEffect(new ActiveStatusEffect(new SlowStatus(0.25f, "wewa"), new List<BaseCondition> { new TimeCondition(false, 0.1f) }, false));
            }
        }
    }
    // Uses Barycentric Coordinates to check for a target within the system area
    public bool CheckTargetInTriangle(Vector3 target)
    {
        float s1 = C.y - A.y;
        float s2 = C.x - A.x;
        float s3 = B.y - A.y;
        float s4 = target.z - A.y;

        float w1 = (A.x * s1 + s4 * s2 - target.x * s1) / (s3 * s2 - (B.x - A.x) * s1);
        float w2 = (s4 - w1 * s3) / s1;

        return w1 >= 0 && w2 >= 0 && (w1 + w2) <= 1;
    }
}
