using UnityEngine;

public class WebConnectionVisual : MonoBehaviour
{
    public LineRenderer lr;
    private bool hasBeenDestroyed = false;
    private Vector3 NodeAFloorPos;
    private Vector3 NodeBFloorPos;

    private void Awake()
    {
        if (lr == null) { lr = GetComponent<LineRenderer>(); }
    }
    public void SetConnectionRenderer(SpiderWebConnection connection)
    {
        NodeAFloorPos = new Vector3(connection.NodeA.transform.position.x, connection.NodeA.transform.position.y - (connection.NodeA.bodySystem.renderer.bounds.size.y / 2) + 0.01f, connection.NodeA.transform.position.z);
        NodeBFloorPos = new Vector3(connection.NodeB.transform.position.x, connection.NodeB.transform.position.y - (connection.NodeB.bodySystem.renderer.bounds.size.y / 2) + 0.01f, connection.NodeB.transform.position.z);
        hasBeenDestroyed = false;
        lr.positionCount = 2;
        lr.SetPosition(0, NodeAFloorPos);
        lr.SetPosition(1, NodeBFloorPos);
    }

    public void DestroyMe()
    {
        if (hasBeenDestroyed) { return; }
        hasBeenDestroyed = true;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
