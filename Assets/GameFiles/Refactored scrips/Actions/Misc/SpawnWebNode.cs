using UnityEngine;
using System;

[Serializable]
public class SpawnWebNode : BaseEntityAction
{
    IWebSpawner webSpawner;

    public SpawnWebNode() { }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);

        webSpawner = ownerEntity as IWebSpawner;

        if (webSpawner == null ) { EndAction(); return; }

        SpawnNode();
    }

    public void SpawnNode()
    {
        // Minimum distance between web nodes
        Collider[] colliders = Physics.OverlapSphere(ownerEntity.transform.position, 15);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<SpiderWebNode>())
            {
                EndAction();
                return;
            }
        }

        // Spawn and Setup a new node whilst informing the manager
        SpiderWebNode newNode = ObjectPoolManager.SpawnObject(webSpawner.webNodePrefab, ownerEntity.transform.position, Quaternion.identity).GetComponent<SpiderWebNode>();
        newNode.hostileMask = ownerEntity.hostileMask;
        SpiderWebChecker.Instance.NewWebNodeAdded(newNode);

        EndAction();
    }

    public override void EndAction()
    {
        isComplete = true;
    }

    public override BaseEntityAction Clone()
    {
        return new SpawnWebNode();
    }
}

public interface IWebSpawner
{
    public GameObject webNodePrefab { get; set; }
}