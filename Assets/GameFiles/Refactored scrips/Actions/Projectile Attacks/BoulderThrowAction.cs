using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoulderThrowAction : BaseEntityAction, ISlam
{
    [SerializeField] private Vector3 offset = Vector3.zero;

    [SerializeField] protected int SlamDamage;
    [SerializeField] protected Color SlamColor;
    [SerializeField] protected float ChargeTime;
    [SerializeField] protected Stat SlamRange = new Stat(5);
    [SerializeField] protected Vector3 SlamPositionOffset; //not needed as using offset, didnt want to use as would be a missuse of the name

    public int slamDamage { get => SlamDamage; set => SlamDamage = value; }
    public Color slamColour { get => SlamColor; set => SlamColor = value; }
    public float chargeTime { get => ChargeTime; set => ChargeTime = value; }
    public Stat slamRange { get => SlamRange; set => SlamRange = value; }
    public Vector3 slamPositionOffset { get => SlamPositionOffset; set => SlamPositionOffset = value; }

    private ArcingProjectile boulder;
    private IBoulderThrow boulderThrow;
    private IAnimated animated;
    private Coroutine actionRoutine = null;

    public BoulderThrowAction() { }
    public BoulderThrowAction(bool preventsMovement, Vector3 offset, int slamDamage, Color slamColor, float chargeTime, Stat slamRange)
    { 
        this.preventsMovement = preventsMovement;
        this.offset = offset;
        this.slamDamage = slamDamage;
        this.slamColour = slamColor;
        this.chargeTime = chargeTime;
        this.slamRange = slamRange;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);

        if (!(ownerEntity is IBoulderThrow boulderThrow)) return;
        if (!(ownerEntity is IAnimated animated)) return;

        this.boulderThrow = boulderThrow;
        this.animated = animated;

        actionRoutine = ownerEntity.StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        boulder = ObjectPoolManager.SpawnObject(boulderThrow.boulderObj, ownerEntity.transform.position, Quaternion.identity).GetComponent<ArcingProjectile>();

        Vector3 pos = ownerEntity.transform.position;
        pos += ownerEntity.transform.forward * 5f;
        pos.y += 2.5f;
        //ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.RockBurst01), pos, Quaternion.Euler(90, 0, 0)).
        //        GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(overrideColourHues: new rangePair(0.6f, 1f), overrideBurstCount: new rangePair(5, 10), overrideSpeed: new rangePair(-15, -20), overrideShapeRadius: 10));
        ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.RockBurst01), pos, Quaternion.Euler(90, 0, 0)).
                GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> {
                    new ColourHueEffectOverride(new rangePair(0.6f, 1f)),
                    new BurstCountEffectOverride(new rangePair(5, 10)),
                    new StartSpeedEffectOverride(new rangePair(-15, -20)),
                    new ShapeRadiusEffectOverride(10)
                }));
        float animationTime = 3.75f;
        animated.animationManager.PlayAnimationCrossFade(AnimationType.RockThrow, 1, MixerType.main, 0.2f, animationTime);
        yield return TrackBolderToArm(boulderThrow.boulderRootBone, 2.35f);

        actionRoutine = null;
        boulder = null;
        EndAction();
    }

    private IEnumerator TrackBolderToArm(Transform rootBone, float duration)
    {
        float timer = duration;
        float t = 0;

        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;

            boulder.transform.SetPositionAndRotation(rootBone.position + rootBone.rotation * offset, rootBone.rotation);
            yield return null;
        }

        boulder.HandlePathToTarget(ownerEntity, ownerEntity.target.transform.position, 3, slamDamage, slamColour, slamRange.GetFinalValue());
    }

    public override void InterruptAction()
    {
        if (actionRoutine != null)
        {
            ownerEntity.StopCoroutine(actionRoutine);
            actionRoutine = null;
        }
        if (boulder != null)
        {
            boulder.Interrupt();
            boulder = null;
        }
        EndAction();
    }
    public override void EndAction()
    {
        isComplete = true;
    }
    public override BaseEntityAction Clone()
    {
        return new BoulderThrowAction(preventsMovement, offset, slamDamage, slamColour, chargeTime, slamRange);
    }


    //protected override void SetupSlam()
    //{
    //    slamVariablesAccess = ownerEntity as ISlamActionRequirements;
    //    chargeUpTimer = 0;
    //    chargeComplete = false;
    //    attackInterrupted = false;

    //    slamOrigin = ownerEntity.target.transform.position;

    //    IBoulderThrow boulderThrow = ownerEntity as IBoulderThrow;
    //    GameObject boulder = ObjectPoolManager.SpawnObject(boulderThrow.boulderObj, ownerEntity.transform.position, Quaternion.identity);
    //    boulder.GetComponent<ThrowableBoulder>().HandlePathToTarget(slamOrigin, chargeTime);

    //    SpawnSlamStartVFX();
    //}
}
