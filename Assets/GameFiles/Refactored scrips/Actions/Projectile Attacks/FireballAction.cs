using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class FireballAction : BaseEntityAction, ISlam
{
    [SerializeField] private Vector3 offset = Vector3.zero;

    [SerializeField] protected int SlamDamage;
    [SerializeField] protected Color SlamColor;
    [SerializeField] protected float ChargeTime;
    [SerializeField] protected Stat SlamRange = new Stat(5);
    [SerializeField] protected Vector3 SlamPositionOffset; //not needed as using offset, didnt want to use as would be a missuse of the name

    private Fireball fireball;
    private Coroutine endActionDelayRoutine, trackFireballToMouthRoutine;

    public int slamDamage { get => SlamDamage; set => SlamDamage = value; }
    public Color slamColour { get => SlamColor; set => SlamColor = value; }
    public float chargeTime { get => ChargeTime; set => ChargeTime = value; }
    public Stat slamRange { get => SlamRange; set => SlamRange = value; }
    public Vector3 slamPositionOffset { get => SlamPositionOffset; set => SlamPositionOffset = value; }

    public FireballAction() { }
    public FireballAction(bool preventsMovement, Vector3 offset, int slamDamage, Color slamColor, float chargeTime, Stat slamRange)
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

        if (ownerEntity is IFireballAction fireballAction)
        {
            fireball = ObjectPoolManager.SpawnObject(fireballAction.fireballObj, ownerEntity.transform.position + offset, Quaternion.identity).GetComponent<Fireball>();
            if (ownerEntity is IAnimated animated)
            {
                float animationTime = 3.75f;
                animated.animationManager.PlayAnimationCrossFade(AnimationType.Attack, 1, MixerType.complimentary, 0.2f, animationTime);
                endActionDelayRoutine = ownerEntity.StartCoroutine(EndActionDelay(animationTime));
                trackFireballToMouthRoutine = ownerEntity.StartCoroutine(TrackFireballToMouth(fireball.gameObject, fireballAction.fireballRootBone.transform, 2.25f));
            }
        }
    }

    private IEnumerator TrackFireballToMouth(GameObject fireball, Transform rootBone, float duration)
    {
        Vector3 fireballPos = rootBone.position + rootBone.rotation * offset;
        float startY = rootBone.position.y;

        Vector3 startScale = fireball.transform.localScale;
        Vector3 smallScale = fireball.transform.localScale / 10;

        fireball.transform.localScale = smallScale;

        float timer = duration;
        float t = 0;
        float easeInT = 0;

        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;
            easeInT = 1f - Mathf.Pow(Mathf.Max(0f, 1f - t), 0.2f);

            fireball.transform.localScale = Vector3.Lerp(smallScale, startScale, easeInT);
            //Quaternion rotation = rootBone.rotation * Quaternion.Euler(0, 180, 0);
            fireballPos = rootBone.position;
            //fireballPos.y = startY;
            fireball.transform.SetPositionAndRotation(fireballPos + rootBone.rotation * offset, rootBone.rotation);
            yield return null;
        }

        fireball.transform.localScale = startScale;

        Vector3 direction = (ownerEntity.target.transform.position - fireball.transform.position).normalized;
        fireball.GetComponent<Fireball>().Initialize(ownerEntity, direction, slamDamage, 6); ;
    }

    private IEnumerator EndActionDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        EndAction();
    }

    public override void InterruptAction()
    {
        if (trackFireballToMouthRoutine != null)
        {
            ownerEntity.StopCoroutine(trackFireballToMouthRoutine);
        }
        if (endActionDelayRoutine != null)
        { 
            ownerEntity.StopCoroutine(endActionDelayRoutine);
        }

        if (fireball.gameObject != null && fireball.active)
        {
            ObjectPoolManager.ReturnObjectToPool(fireball.gameObject);
        }

        EndAction();
    }
    public override void EndAction()
    {
        isComplete = true;
    }
    public override BaseEntityAction Clone()
    {
        return new FireballAction(preventsMovement, offset, slamDamage, slamColour, chargeTime, slamRange);
    }
}
