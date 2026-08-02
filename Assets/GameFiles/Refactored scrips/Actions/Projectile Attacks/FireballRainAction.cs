using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;

[Serializable]
public class FireballRainAction : BaseEntityAction, ISlam
{
    [SerializeField] protected int SlamDamage;
    [SerializeField] protected Color SlamColor;
    [SerializeField] protected float ChargeTime;
    [SerializeField] protected Stat SlamRange = new Stat(5);
    [SerializeField] protected Vector3 SlamPositionOffset; //not needed as using offset, didnt want to use as would be a missuse of the name

    [SerializeField] protected float startDelay = 0f;
    [SerializeField] protected float actionDuration = 5f;
    [SerializeField] protected int totalFireballs;
    [SerializeField] protected Vector3 centerPoint;
    [SerializeField] protected float radius;

    [SerializeField] private float flameRadius = 5f;
    [SerializeField] private float flameDuration = 20f;
    [SerializeField] private float flameSpeed = 100f;
 

    private IAnimated animated;
    private IFireballAction fireballAction;

    public int slamDamage { get => SlamDamage; set => SlamDamage = value; }
    public Color slamColour { get => SlamColor; set => SlamColor = value; }
    public float chargeTime { get => ChargeTime; set => ChargeTime = value; }
    public Stat slamRange { get => SlamRange; set => SlamRange = value; }
    public Vector3 slamPositionOffset { get => SlamPositionOffset; set => SlamPositionOffset = value; }

    public FireballRainAction() { }
    public FireballRainAction(bool preventsMovement, int slamDamage, Color slamColor, float chargeTime, Stat slamRange, float startDelay, float actionDuration, int totalFireballs, Vector3 centrePoint, float radius)
    {
        this.preventsMovement = preventsMovement;
        this.slamDamage = slamDamage;
        this.slamColour = slamColor;
        this.chargeTime = chargeTime;
        this.slamRange = slamRange;
        this.startDelay = startDelay;
        this.actionDuration = actionDuration;
        this.totalFireballs = totalFireballs;
        this.centerPoint = centrePoint;
        this.radius = radius;
    }

    public override void StartAction(Entity ownerEntity)
    {
        Debug.Log("Starting Fireball Rain Action");
        this.ownerEntity = ownerEntity;
        actionable = ownerEntity as IActionable;
        isComplete = false;

        if (ownerEntity is IFireballAction fireballAction)
        {
            this.fireballAction = fireballAction;
        }

        if (ownerEntity is IAnimated animated)
        { 
            this.animated = animated;
        }

        ownerEntity.StartCoroutine(ActionRoutine());
    }

    private IEnumerator ActionRoutine()
    {
        animated.animationManager.PlayAnimationCrossFade(AnimationType.ScreamUpwards, 0, MixerType.complimentary, 0.5f, 6f);
        yield return new WaitForSeconds(2);
        yield return ownerEntity.StartCoroutine(LaunchFireballsIntoSky(totalFireballs, 2.5f));
        yield return new WaitForSeconds(2.5f);

 
        animated.animationManager.PlayAnimationCrossFade(AnimationType.Idle, 1, MixerType.main);
        yield return ownerEntity.StartCoroutine(FireballRain(totalFireballs));

        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f); //If it works, it works

        EndAction();
    }

    private IEnumerator LaunchFireballsIntoSky(int amountOfRain, float duration)
    {
        amountOfRain /= 2;
        float delay = (duration / amountOfRain);

        while (amountOfRain > 0)
        {
            SpawnFireballIntoSky();
            amountOfRain--;
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator FireballRain(int amountOfRain)
    {
        float delayBetweenFireballs = actionDuration / amountOfRain;

        while (amountOfRain > 0)
        {
            SpawnFireball();
            amountOfRain--;
            yield return new WaitForSeconds(delayBetweenFireballs);
        }
    }

    private void SpawnFireballIntoSky()
    {
        GameObject fireball = ObjectPoolManager.SpawnObject(fireballAction.fireballObj, fireballAction.fireballRootBone.transform.position, Quaternion.identity);
        fireball.transform.localScale = Vector3.one * 0.5f;
        fireball.transform.position += new Vector3(0, 2, 0);

        Vector3 direction = Vector3.up - Vector3.down;
        fireball.GetComponent<Fireball>().Initialize(ownerEntity, direction, slamDamage, 0, 0, 0, flameSpeed);
    }

    private void SpawnFireball()
    {
        Vector3 randomPosition = FindRandomPositionAboveArena();
        GameObject fireball = ObjectPoolManager.SpawnObject(fireballAction.fireballObj, randomPosition, Quaternion.identity);

        Vector3 direction = Vector3.down - Vector3.up;
        fireball.GetComponent<Fireball>().Initialize(ownerEntity, direction, slamDamage, 0, flameRadius, flameDuration, flameSpeed);
    }

    private Vector3 FindRandomPositionAboveArena()
    { 
        float randomX = Random.Range(-radius, radius);
        float randomZ = Random.Range(-radius, radius);

        Vector3 position = new Vector3(centerPoint.x + randomX, centerPoint.y, centerPoint.z + randomZ);
        return position;
    }

    public override void InterruptAction()
    {
        EndAction();
    }
    public override void EndAction()
    {
        isComplete = true;
    }
    public override BaseEntityAction Clone()
    {
        return new FireballRainAction(preventsMovement, slamDamage, slamColour, chargeTime, slamRange, startDelay, actionDuration, totalFireballs, centerPoint, radius);
    }
}
