using UnityEngine;

public class WebNodeHealthSystem : EntityHealthSystem
{
    private SpiderWebNode OwnerNode;
    public override void InitialiseSystem(Entity entity)
    {
        OwnerNode = entity as SpiderWebNode;
    }
    // Notify The Manager of a Node's deletion
    public override void OnDeath()
    {
        OwnerNode.connections.Clear();
        OwnerNode.systems.Clear();

        SpiderWebChecker.Instance.NodeRemoved(OwnerNode);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
