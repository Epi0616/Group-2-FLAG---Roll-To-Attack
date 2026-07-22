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
            GameObject boulder = ObjectPoolManager.SpawnObject(boulderThrow.boulderObj, ownerEntity.transform.position, Quaternion.identity);
            if (ownerEntity is IAnimated animated)
            {
                float animationTime = 3.75f;
                animated.animationManager.PlayAnimationCrossFade(AnimationType.RockThrow, 1, MixerType.main, 0.2f, animationTime);
                ownerEntity.StartCoroutine(EndActionDelay(animationTime));
                ownerEntity.StartCoroutine(TrackBolderToArm(boulder, boulderThrow.boulderRootBone, 2.35f));
            }
        }
    }

    private IEnumerator TrackBolderToArm(GameObject boulder, Transform rootBone, float duration)
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

        boulder.GetComponent<ThrowableBoulder>().HandlePathToTarget(ownerEntity, ownerEntity.target.transform.position, 3, slamDamage, slamColour, slamRange.GetFinalValue());
    }

    private IEnumerator EndActionDelay(float duration)
    { 
        yield return new WaitForSeconds(duration);
        EndAction();
    }

    public override void InterruptAction()
    {
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
