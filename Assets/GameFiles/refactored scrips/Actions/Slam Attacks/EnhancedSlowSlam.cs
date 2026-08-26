using UnityEngine;
using System;
using UnityEngine.ResourceManagement;

[Serializable]
public class EnhancedSlowSlam : BaseSlamAction, IEnhancedAbility
{
    public float SlowMult = 1.4f;
    private ISlowBubbleSpawner ISBS;
    public int enhancementLevel { get; set; }

    public EnhancedSlowSlam() { }

    public EnhancedSlowSlam(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, float SlowMult, bool DoesPrevent, int enhancementLevel) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        this.SlowMult = SlowMult;
        this.enhancementLevel = enhancementLevel;
    }

    public override void StartAction(Entity entity)
    {
        base.StartAction(entity);
        ISBS = ownerEntity as ISlowBubbleSpawner;
    }

    public override void SpawnSlamStartVFX()
    {

    }

    protected override void ApplyHeavyEffectPerEntity(Entity hitEntity)
    {
        
    }

    public override void ExtraSlamEffect()
    {
        //if (ISBS.currentBubbleInstance != null)
        //{
        //    ISBS.currentBubbleInstance.DestroyMe();  
        //}

        //ISBS.currentBubbleInstance = (ObjectPoolManager.SpawnObject(ISBS.slowBubblePrefab, slamOrigin, Quaternion.identity)).GetComponent<EnhancedSlowingBubble>();
        //ISBS.currentBubbleInstance.Initialize(ownerEntity, SlowMult, slamRange.GetFinalValue(), 20f, slamColour, enhancementLevel);
        EnhancedSlowingBubble bubble = (ObjectPoolManager.SpawnObject(ISBS.slowBubblePrefab, slamOrigin, Quaternion.identity)).GetComponent<EnhancedSlowingBubble>();
        bubble.Initialize(ownerEntity, SlowMult, slamRange.GetFinalValue(), 5f, slamColour, enhancementLevel);
    }

    public override BaseEntityAction Clone()
    {
        return new EnhancedSlowSlam(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, SlowMult, preventsMovement, enhancementLevel);
    }
}
