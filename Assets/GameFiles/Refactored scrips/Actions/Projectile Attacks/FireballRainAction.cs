using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class FireballRainAction : BaseEntityAction, ISlam
{
    public static event Action<WaveObj> SpawnWaveRequest;

    [SerializeField] private WaveObj fireballRainEnemies;

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

    private IAnimated animated;
    private IFireballAction fireballAction;

    private Coroutine actionRoutine;

    public int slamDamage { get => SlamDamage; set => SlamDamage = value; }
    public Color slamColour { get => SlamColor; set => SlamColor = value; }
    public float chargeTime { get => ChargeTime; set => ChargeTime = value; }
    public Stat slamRange { get => SlamRange; set => SlamRange = value; }
    public Vector3 slamPositionOffset { get => SlamPositionOffset; set => SlamPositionOffset = value; }

    public FireballRainAction() { }
    public FireballRainAction(bool preventsMovement, WaveObj fireballRainEnemies, int slamDamage, Color slamColor, float chargeTime, Stat slamRange, float startDelay, float actionDuration, int totalFireballs, Vector3 centrePoint, float radius)
    {
        this.preventsMovement = preventsMovement;
        this.fireballRainEnemies = fireballRainEnemies;
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

        actionRoutine = ownerEntity.StartCoroutine(ActionRoutine());
    }

    private IEnumerator ActionRoutine()
    {
        BecomeInvulnerable();
        animated.animationManager.PlayAnimationCrossFade(AnimationType.ScreamUpwards, 0, MixerType.complimentary, 0.5f, 6f);
        yield return new WaitForSeconds(2);

        yield return LaunchFireballsIntoSky(totalFireballs, 2.5f);
        yield return new WaitForSeconds(3.5f);

        SpawnWaveRequest?.Invoke(fireballRainEnemies);
        yield return FireballRain(totalFireballs);

        yield return new WaitForSeconds(10);

        actionRoutine = null;
        EndAction();
    }

    private void BecomeInvulnerable()
    {
        ActiveStatusEffect invulnerableEffect = new(new BaseInvulnerableEffect(), new List<BaseCondition>() { new TimeCondition(true, 6)}, false);
        ownerEntity.OnRecieveEffect(invulnerableEffect);
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
            SpawnFireballRain();
            amountOfRain--;
            yield return new WaitForSeconds(delayBetweenFireballs);
        }
    }

    private void SpawnFireballIntoSky()
    {
        Vector3 fireballPosition = fireballAction.fireballRootBone.transform.position + new Vector3 (0, 2, 0);
        Quaternion fireballRotation = Quaternion.LookRotation(Vector3.up, Vector3.up);

        GameObject fireball = ObjectPoolManager.SpawnObject(fireballAction.fireballObj, fireballPosition, fireballRotation);
        fireball.transform.rotation = fireballRotation;
        fireball.transform.position = fireballPosition;
        //fireball.transform.localScale = Vector3.one * 0.5f;

        fireball.GetComponent<Fireball>().Initialize(ownerEntity, 50f, 7, 2);
    }

    private void SpawnFireballRain()
    {
        Vector3 randomPosition = FindRandomPositionAboveArena();
        Quaternion fireballRotation = Quaternion.LookRotation(Vector3.down, Vector3.up);

        GameObject fireball = ObjectPoolManager.SpawnObject(fireballAction.fireballObj, randomPosition, fireballRotation);
        fireball.transform.rotation = fireballRotation;
        fireball.transform.position = randomPosition;

        fireball.GetComponent<Fireball>().Initialize(ownerEntity, 50f, 7, 2);
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
        Debug.Log("Interrupting Fireball Rain Action");

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
        return new FireballRainAction(preventsMovement, fireballRainEnemies, slamDamage, slamColour, chargeTime, slamRange, startDelay, actionDuration, totalFireballs, centerPoint, radius);
    }
}
