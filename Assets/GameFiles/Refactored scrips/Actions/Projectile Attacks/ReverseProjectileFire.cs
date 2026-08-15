using System.Collections;
using UnityEngine;

public class ReverseProjectileFire : BaseEntityAction
{
    private Coroutine actionRoutine;

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);

        actionRoutine = ownerEntity.StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        actionRoutine = null;
        yield return null;
    }

    public override void InterruptAction()
    {
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
        return new ReverseProjectileFire();
    }
}
