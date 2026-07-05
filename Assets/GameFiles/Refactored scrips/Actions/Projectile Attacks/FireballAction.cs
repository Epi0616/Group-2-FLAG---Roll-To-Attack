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
            GameObject fireball = ObjectPoolManager.SpawnObject(fireballAction.fireballObj, ownerEntity.transform.position + offset, Quaternion.identity);
            if (ownerEntity is IAnimated animated)
            {

            }

            Vector3 direction = (ownerEntity.target.transform.position - fireball.transform.position).normalized;
            Debug.Log(direction);
            fireball.GetComponent<Fireball>().Initialize(ownerEntity, direction, slamDamage, 0);
        }


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
        return new FireballAction(preventsMovement, offset, slamDamage, slamColour, chargeTime, slamRange);
    }
}
