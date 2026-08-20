using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArcingProjectileThrowAction : BaseEntityAction, ISlam
{
    [SerializeField] protected Vector3 offset = Vector3.zero;

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

    protected ArcingProjectile projectile;
    protected IArcingProjectile arcingProjectile;
    protected IAnimated animated;
    protected Coroutine actionRoutine = null;

    public ArcingProjectileThrowAction() { }
    public ArcingProjectileThrowAction(bool preventsMovement, Vector3 offset, int slamDamage, Color slamColor, float chargeTime, Stat slamRange)
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

        if (!(ownerEntity is IArcingProjectile arcingProjectile)) return;
        this.arcingProjectile = arcingProjectile;

        if (!(ownerEntity is IAnimated animated)) return;
        this.animated = animated;

        actionRoutine = ownerEntity.StartCoroutine(Action());
    }

    protected virtual IEnumerator Action()
    {
        projectile = ObjectPoolManager.SpawnObject(arcingProjectile.arcingProjectileObj, ownerEntity.transform.position, Quaternion.identity).GetComponent<ArcingProjectile>();
        if (projectile is IDecalShadowCast shadow)
        {
            shadow.currentShadowDecal = ObjectPoolManager.SpawnObject(shadow.shadowDecalPrefab, projectile.transform.position, new Quaternion(1, 0, 0, 1)).GetComponent<ShadowDecal>();
            shadow.currentShadowDecal.SetupProjector(new Vector2(7, 7), new Quaternion(1, 0, 0, 1), new Vector3(0, 5, 0), true, false, projectile.gameObject);
            shadow.currentShadowDecal.SetNewOffset(new Vector3(0, -3, 0), true, 2);
        }
        //projectile.currentShadowDecal = ObjectPoolManager.SpawnObject(projectile.shadowDecalPrefab, projectile.transform.position, Quaternion.identity).GetComponent<ShadowDecal>();
        //projectile.currentShadowDecal.SetupProjector(new Vector2(8, 8), new Quaternion(0, 0, 0, 0), new Vector3(0, 0, -5f), true, false, projectile.transform);
        Vector3 pos = ownerEntity.transform.position;
        pos += ownerEntity.transform.forward * 5f;
        pos.y += 2.5f;
        //ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.RockBurst01), pos, Quaternion.Euler(90, 0, 0)).
        //        GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(overrideColourHues: new rangePair(0.6f, 1f), overrideBurstCount: new rangePair(5, 10), overrideSpeed: new rangePair(-15, -20), overrideShapeRadius: 10));
        ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.RockBurst01), pos, Quaternion.Euler(90, 0, 0)).
                GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> {
                    new BurstCountEffectOverride(new rangePair(3, 7)),
                    new StartSpeedEffectOverride(new rangePair(-15, -20)),
                    new ShapeRadiusEffectOverride(10)
                }));
        float animationTime = 3.75f;
        animated.animationManager.PlayAnimationCrossFade(AnimationType.RockThrow, 1, MixerType.main, 0.2f, animationTime);
        yield return TrackBolderToArm(arcingProjectile.arcingProjectileRootBone, 2.35f);

        actionRoutine = null;
        projectile = null;
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

            projectile.transform.SetPositionAndRotation(rootBone.position + rootBone.rotation * offset, rootBone.rotation);
            yield return null;
        }

        projectile.HandlePathToTarget(ownerEntity, ownerEntity.target.transform.position, 3, slamDamage, slamColour, slamRange.GetFinalValue());
    }

    public override void InterruptAction()
    {
        if (actionRoutine != null)
        {
            ownerEntity.StopCoroutine(actionRoutine);
            actionRoutine = null;
        }
        if (projectile != null)
        {
            projectile.Interrupt();
            projectile = null;
        }
        EndAction();
    }
    public override void EndAction()
    {
        isComplete = true;
    }
    public override BaseEntityAction Clone()
    {
        return new ArcingProjectileThrowAction(preventsMovement, offset, slamDamage, slamColour, chargeTime, slamRange);
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
