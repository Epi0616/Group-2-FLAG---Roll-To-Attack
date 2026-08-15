using UnityEngine;

public class WebNodeHealthSystem : EntityHealthSystem
{
    private SpiderWebNode OwnerNode;
    public override void InitialiseSystem(Entity entity)
    {
        OwnerNode = entity as SpiderWebNode;
    }
    public override void OnDeath()
    {
        SpiderWebChecker.Instance.NodeRemoved(OwnerNode);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
