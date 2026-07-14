using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class FlyAwayAndBack : BaseEntityAction
{
    private Vector3 endPosition;
    private Vector3 startPosition;
    [SerializeField] private float duration = 5f;
    private INavAgent navAgent;

    public FlyAwayAndBack() { }
    public FlyAwayAndBack(bool preventsMovement, float duration)
    { 
        this.preventsMovement = preventsMovement;
        this.duration = duration;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);
        if (ownerEntity is IAnimated animated)
        {
        }

        startPosition = ownerEntity.transform.position;
        endPosition = ownerEntity.transform.position;
        endPosition.y += 65f;

        if (ownerEntity is INavAgent agent)
        { 
            navAgent = ownerEntity as INavAgent;
            navAgent.DisableAIAgent();
        }

        ownerEntity.StartCoroutine(FlyInstructions());
    }

    private IEnumerator FlyInstructions()
    {
        yield return ownerEntity.StartCoroutine(Fly(ownerEntity.transform.position, endPosition, duration));
        Debug.Log("Reached end position, now flying back");
        yield return ownerEntity.StartCoroutine(Fly(ownerEntity.transform.position, startPosition, duration));
        Debug.Log("reached start pos");
        EndAction();
    }

    private IEnumerator Fly(Vector3 start, Vector3 end, float duration)
    {
        navAgent.DisableAIAgent();
        float timer = duration;
        float t = 0;

        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;

            ownerEntity.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }

    public override void EndAction()
    {
        navAgent.EnableAIAgent();
        isComplete = true;
    }

    public override BaseEntityAction Clone()
    {
        return new FlyAwayAndBack(preventsMovement, duration);
    }
}
