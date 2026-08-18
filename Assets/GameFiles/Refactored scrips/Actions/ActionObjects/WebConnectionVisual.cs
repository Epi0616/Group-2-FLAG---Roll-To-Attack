using UnityEngine;

public class WebConnectionVisual : MonoBehaviour
{
    public LineRenderer lr;
    private bool hasBeenDestroyed = false;

    private void Awake()
    {
        if (lr == null) { lr = GetComponent<LineRenderer>(); }
    }
    public void SetConnectionRenderer(SpiderWebConnection connection)
    {
        hasBeenDestroyed = false;
        lr.positionCount = 2;
        lr.SetPosition(0, connection.NodeA.transform.position);
        lr.SetPosition(1, connection.NodeB.transform.position);
    }

    public void DestroyMe()
    {
        if (hasBeenDestroyed) { return; }
        hasBeenDestroyed = true;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
