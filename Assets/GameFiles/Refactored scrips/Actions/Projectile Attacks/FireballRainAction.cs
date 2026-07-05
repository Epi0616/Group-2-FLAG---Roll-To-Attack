using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class FireballRainAction : BaseEntityAction, ISlam
{
    [SerializeField] protected int SlamDamage;
    [SerializeField] protected Color SlamColor;
    [SerializeField] protected float ChargeTime;
    [SerializeField] protected Stat SlamRange = new Stat(5);
    [SerializeField] protected Vector3 SlamPositionOffset; //not needed as using offset, didnt want to use as would be a missuse of the name

    [SerializeField] protected int amountOfRain;
    [SerializeField] protected Vector3 centerPoint;
    [SerializeField] protected float radius;
    private IFireballAction fireballAction;

    public int slamDamage { get => SlamDamage; set => SlamDamage = value; }
    public Color slamColour { get => SlamColor; set => SlamColor = value; }
    public float chargeTime { get => ChargeTime; set => ChargeTime = value; }
    public Stat slamRange { get => SlamRange; set => SlamRange = value; }
    public Vector3 slamPositionOffset { get => SlamPositionOffset; set => SlamPositionOffset = value; }

    public FireballRainAction() { }
    public FireballRainAction(bool preventsMovement, int slamDamage, Color slamColor, float chargeTime, Stat slamRange, int amountOfRain, Vector3 centrePoint, float radius)
    {
        this.preventsMovement = preventsMovement;
        this.slamDamage = slamDamage;
        this.slamColour = slamColor;
        this.chargeTime = chargeTime;
        this.slamRange = slamRange;
        this.amountOfRain = amountOfRain;
        this.centerPoint = centrePoint;
        this.radius = radius;
    }

    public override void StartAction(Entity ownerEntity)
    {
        this.ownerEntity = ownerEntity;
        actionable = ownerEntity as IActionable;
        isComplete = false;

        if (ownerEntity is IFireballAction)
        {
            fireballAction = ownerEntity as IFireballAction;
            ownerEntity.StartCoroutine(FireballRain(amountOfRain));
        }
    }

    private IEnumerator FireballRain(int amountOfRain)
    {
        while (amountOfRain > 0)
        {
            SpawnFireball();
            amountOfRain--;
            yield return new WaitForSeconds(0.5f);
        }

        EndAction();
    }

    private void SpawnFireball()
    {
        Vector3 randomPosition = FindRandomPositionAboveArena();
        GameObject fireball = ObjectPoolManager.SpawnObject(fireballAction.fireballObj, randomPosition, Quaternion.identity);

        Vector3 direction = Vector3.down - Vector3.up;
        fireball.GetComponent<Fireball>().Initialize(ownerEntity, direction, slamDamage, 0);
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
        return new FireballRainAction(preventsMovement, slamDamage, slamColour, chargeTime, slamRange, amountOfRain, centerPoint, radius);
    }
}
