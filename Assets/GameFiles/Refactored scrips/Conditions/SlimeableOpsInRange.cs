using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class SlimeableOpsInRange : BaseCondition
{
    [SerializeField] private float distanceThreshold;
    [SerializeField] private float interval;

    private Entity entity;
    private ISlimeTrail slimeTrail;
    private ISlimeSplit slimeSplit;
    private bool isConditionMet;

    private float timer;

    public SlimeableOpsInRange() { }

    public SlimeableOpsInRange(bool inverse, float distanceThreshold, float interval)
    {
        this.inverse = inverse;
        this.distanceThreshold = distanceThreshold;
        this.interval = interval;

        isConditionMet = false;
    }
    public override void Initialize(Entity entity)
    {
        this.entity = entity;

        if (!(entity is ISlimeTrail slimeTrail)) { Debug.LogError("entity is not of type ISlimeTrail"); return; }
        this.slimeTrail = slimeTrail;

        if (!(entity is ISlimeSplit slimeSplit)) { Debug.LogError("entity is not of type ISlimSplit"); return; }
        this.slimeSplit = slimeSplit;

        timer = interval;
    }
    public override void ConditionUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = interval;
            CheckForOpsInRange();
        }
    }
    public override void ResetCondition()
    {
        isConditionMet = false;
    }
    public override bool IsConditionMet()
    {
        return inverse ? !isConditionMet : isConditionMet;
    }

    private void CheckForOpsInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(entity.transform.position, distanceThreshold * slimeSplit.scale, slimeTrail.slimeableMask);
        foreach (Collider collider in colliders)
        {
            if (!collider.gameObject) { continue; }
            if (collider.gameObject == entity.gameObject) { continue; }

            if (collider.TryGetComponent<Entity>(out Entity newEntity))
            {
                isConditionMet = true;
                return;
            }
        }

        isConditionMet = false;
    }

    public override BaseCondition Clone()
    {
        return new SlimeableOpsInRange(inverse, distanceThreshold, interval);
    }
}
