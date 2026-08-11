using System;
using System.Collections;
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

    private ThrowableBoulder boulder;

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
        this.ownerEntity = ownerEntity;
        actionable = ownerEntity as IActionable;
        isComplete = false;

        if (ownerEntity is IBoulderThrow boulderThrow)
        {
            boulder = ObjectPoolManager.SpawnObject(boulderThrow.boulderObj, ownerEntity.transform.position, Quaternion.identity).GetComponent<ThrowableBoulder>();
            if (ownerEntity is IAnimated animated)
            {
                float animationTime = 3.75f;
                animated.animationManager.PlayAnimationCrossFade(AnimationType.RockThrow, 1, MixerType.main, 0.2f, animationTime);
                ownerEntity.StartCoroutine(EndActionDelay(animationTime));
                ownerEntity.StartCoroutine(TrackBolderToArm(boulderThrow.boulderRootBone, 2.35f));

                Vector3 pos = ownerEntity.transform.position;
                pos += ownerEntity.transform.forward * 5f;
                pos.y += 1.25f;
                ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.RockBurst01), pos, Quaternion.Euler(90, 0, 0)).
                        GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(overrideColourHues: new rangePair(0.05f, 0.1f), overrideBurstCount: new rangePair(25, 30), overrideSpeed: new rangePair(-15, -20), overrideShapeRadius: 10));
            }
        }
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

    private IEnumerator EndActionDelay(float duration)
    { 
        yield return new WaitForSeconds(duration);
        EndAction();
    }

    public override void InterruptAction()
    {
        boulder.Interrupt();
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
