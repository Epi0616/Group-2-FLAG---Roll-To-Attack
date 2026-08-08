using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class SpawnWaveAction : BaseEntityAction
{
    public static event Action<int, int> SpawnWaveRequest;

    [SerializeField] protected int waveIndex;
    [SerializeField] protected int budget;

    private IAnimated animated;
    private Coroutine actionRoutine;

    public SpawnWaveAction() { }
    public SpawnWaveAction(int waveIndex, int budget)
    {
        this.waveIndex = waveIndex;
        this.budget = budget;
    }

    public override void StartAction(Entity ownerEntity)
    {
        if (!(ownerEntity is IAnimated animated)) return;
        this.animated = animated;

        actionRoutine = ownerEntity.StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        animated.animationManager.PlayAnimationCrossFade(AnimationType.Scream, 1, MixerType.complimentary, 0.2f, 3);

        yield return new WaitForSeconds(1f);
        SpawnWaveRequest?.Invoke(waveIndex, budget);
        yield return new WaitForSeconds(5f);

        actionRoutine = null;
        EndAction();
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
        return new SpawnWaveAction(waveIndex, budget);
    }
}
