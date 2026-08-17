using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SplitOnDeath : BaseEntityAction
{
    [SerializeField] private float deathDelay;

    private ISlimeSplit slimeSplit;
    private Coroutine actionRoutine;

    public SplitOnDeath() { }

    public SplitOnDeath(float deathDelay)
    { 
        this.deathDelay = deathDelay;
    }

    public override void StartAction(Entity ownerEntity)
    {
        Debug.Log("splitting on death");
        base.StartAction(ownerEntity);

        if (!(ownerEntity is ISlimeSplit slimeSplit)) return;
        this.slimeSplit = slimeSplit;

        if (slimeSplit.iterationsLeft > 0)
        {
            Debug.Log("starting coroutine");
            actionRoutine = ownerEntity.StartCoroutine(Action());
        }
    }

    private IEnumerator Action()
    { 
        yield return new WaitForSeconds(deathDelay);
        Debug.Log("spawning children");
        SpawnChildren();

        actionRoutine = null;
        EndAction();
    }

    private void SpawnChildren()
    {
        for (int i = 0; i < slimeSplit.childrenSpawned; i++)
        {
            Entity child = ObjectPoolManager.SpawnObject(slimeSplit.childObj, ownerEntity.transform.position, Quaternion.identity).GetComponent<Entity>();
            if (!(child is ISlimeSplit childSlimeSplit)) return;

            child.Reset();

            childSlimeSplit.scale = slimeSplit.scale * 0.75f;
            childSlimeSplit.iterationsLeft = slimeSplit.iterationsLeft - 1;
            child.gameObject.transform.localScale = Vector3.one * childSlimeSplit.scale; //potentially replace Vector3.one with the original starting scale
        }
    }

    public override void InterruptAction()
    {
        Debug.Log("interrupted split on death");
        if (actionRoutine != null)
        {
            ownerEntity.StopCoroutine(actionRoutine);
            actionRoutine = null;
        }

        EndAction();
    }

    public override void EndAction()
    {
        isComplete = true;
    }

    public override BaseEntityAction Clone()
    {
        return new SplitOnDeath(deathDelay);
    }
}
