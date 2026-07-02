using UnityEngine;
using System;
using UnityEngine.VFX;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using System.Collections;

[Serializable]
public class TrackingLaserAction : BaseBoxCastAction , ILaser
{
    [SerializeField] private int TickDamage = 2;
    public int tickDamage { get => TickDamage; set => TickDamage = value; }
    [SerializeField] private float ChargingBeamWidth = 4f;
    public float chargingVisualWidth { get => ChargingBeamWidth; set => ChargingBeamWidth = value; }
    [SerializeField] private float ActiveBeamWidth = 10f;
    public float activeVisualWidth { get => ActiveBeamWidth; set => ActiveBeamWidth = value; }
    [SerializeField] private Color ChargingBeamColour = Color.white;
    public Color chargingVisualColour { get => ChargingBeamColour; set => ChargingBeamColour = value; }
    [SerializeField] private Color ActiveBeamColour = Color.blue;
    public Color activeVisualColour { get => ActiveBeamColour; set => ActiveBeamColour = value; }

    private ILaserRequirements laserAccess;
    private float maxTurnSpeedDegrees = 1080f;
    private float minTurnSpeedDegrees = 60f;
    private float damageTickTimer = 0;

    private IAnimated animated;

    public TrackingLaserAction() { }
    public TrackingLaserAction(int tickDamage, float chargingVisualWidth, float activeVisualWidth, Color chargingVisualColour, Color activeVisualColour,
        float castWidth, float castRange, float chargeDuration, float activeDuration, bool isBlockedByEnvironment, bool doesActionPreventMovement) 
        : base (castWidth, castRange, chargeDuration, activeDuration, isBlockedByEnvironment, doesActionPreventMovement)
    {
        this.tickDamage = tickDamage;
        this.chargingVisualWidth = chargingVisualWidth;
        this.activeVisualWidth = activeVisualWidth;
        this.chargingVisualColour = chargingVisualColour;
        this.activeVisualColour = activeVisualColour;
    }



    public override void StartAction(Entity ownerEntity)
    {
        this.ownerEntity = ownerEntity;
        laserAccess = ownerEntity as ILaserRequirements;
        if (ownerEntity is IAnimated animated) //animation
        {
            this.animated = animated;
        }
        if (laserAccess == null)
        {
            Debug.LogError("ILaserRequirementsMissing");
            EndAction();
        }
        castLayer = LayerMask.GetMask("Ground");
        attackInterrupted = false;
        currentTimer = 0;
        damageTickTimer = 0;
        castOrigin = laserAccess.castOriginTransform;
        if (isBlockedByEnvironment)
        {
            castLayer = laserAccess.environmentLayer | ownerEntity.hostileMask;
        }
        else
        {
            castLayer = ownerEntity.hostileMask;
        }

        if (laserAccess == null)
        {
            currentPhase = CastPhase.Complete;
            EndAction();
        }
        else if (chargeDuration > 0)
        {
            currentPhase = CastPhase.Charging;
            CastChargeStarted();
        }
        else
        {
            currentPhase = CastPhase.Active;
            CastActiveStarted();
        }
    }
    //chargeDuration + activeDuration + 1.5f
    protected override void CastChargeStarted()
    {
        laserAccess.laserVFX.Reinit();
        laserAccess.laserVFX.SetFloat("Duration", 20f);
        laserAccess.laserVFX.SetVector4("Beam Colour", chargingVisualColour);
        laserAccess.laserVFX.enabled = true;
        //Debug.Log("Laser Started");

        animated.animationManager.PlayAnimationCrossFade(AnimationType.Charge, 1);
    }

    protected override void CastChargeUpdate()
    {
        TrackingTurn();        
        //if (laserAccess == null) { return; }
        //if (laserAccess.laserVFX == null) { return; }
        //if (laserAccess.laserHolder == null) { return; }
        //if (chargingVisualWidth == 0) { return; }
        UpdateLaserVFX(chargingVisualWidth, laserAccess.environmentLayer);
    }

    protected override void CastChargeFinished()
    {
       // laserAccess.laserVFX.enabled = false;
    }

    protected override void CastActiveStarted()
    {
        laserAccess.laserVFX.SetVector4("Beam Colour", activeVisualColour);
        laserAccess.laserVFX.enabled = true;

        animated.animationManager.PlayAnimationCrossFade(AnimationType.Attack, 1);
    }

    protected override void CastActiveUpdate()
    {
        damageTickTimer += Time.deltaTime;
        
        
        PerformActivePiercingBoxCast(castLayer);
        
        // don't rotate on active
        UpdateLaserVFX(activeVisualWidth, laserAccess.environmentLayer);
    }

    public override void EndAction()
    {
        if (ownerEntity.gameObject.activeSelf == true)
        {
            ownerEntity.StartCoroutine(BeamEnd());
        }
        else
        {
            isComplete = true;
        }
        //ownerEntity.bodySystem.body.GetComponent<Animator>().speed = 1f;

        //Debug.Log("Laser Turned Off");

    }

    public IEnumerator BeamEnd()
    {
        
        float cooldownVFXTimer = 0;
        float currentWidth = activeVisualWidth;
        while (cooldownVFXTimer < 1.5f && currentWidth > 0f && !attackInterrupted)
        {
            //Debug.Log("Laser Get Smaller");
            currentWidth -= 0.01f;
            cooldownVFXTimer += Time.deltaTime;
            UpdateLaserVFX(currentWidth, laserAccess.environmentLayer);
            yield return null;
        }
        laserAccess.laserVFX.enabled = false;
        isComplete = true;
    }

    public override void InterruptAction()
    {
        attackInterrupted = true;
        EndAction();
    }

    //protected override void PerformActiveSphereCast(LayerMask layer)
    //{
    //    Vector3 groundedOrigin = new Vector3(castOrigin.position.x, castOrigin.position.y - 3f, castOrigin.position.z);
    //    Ray ray = new Ray(groundedOrigin, GetCastDirection());
    //    if (Physics.SphereCast(ray, castRadius, out hit, castRange.GetFinalValue(), layer))
    //    {
    //        Debug.DrawLine(groundedOrigin, hit.point, Color.aquamarine, 100f);
    //        ProcessHit(hit);
    //    }
    //}

    protected override void PerformActivePiercingBoxCast(LayerMask layer)
    {
        RaycastHit[] hits = Physics.BoxCastAll(GetCastOrigin(), GetBoxExtents(), GetCastDirection(), ownerEntity.transform.rotation, castRange.GetFinalValue(), layer);

        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        if (damageTickTimer > 0.3f)
        {
            foreach (RaycastHit hit in hits)
            {        
                if (hit.collider.CompareTag("Environment") || hit.collider.CompareTag("Pedestal"))
                {
                    break;
                } 

                ProcessHit(hit);
            }
            damageTickTimer = 0;
        }
        
    }


    protected override void ProcessHit(RaycastHit hit)
    {
        if (hit.collider == null) return;
        if (hit.collider.gameObject == ownerEntity.gameObject) { return; }
        if (hit.collider.gameObject.CompareTag("StaticEntity") || hit.collider.gameObject.CompareTag("PhysicsEntity")) { return; }

        Entity hitEntity = hit.collider.gameObject.GetComponent<Entity>();
        if (hitEntity == null) { return; }

        hitEntity.OnTakeDamage(tickDamage, Color.white, DamageType.Normal);
    }

    private void UpdateLaserVFX(float width, LayerMask layer)
    {
        Ray ray = UpdateRay();
        float dist = castRange.GetFinalValue();
        if (Physics.SphereCast(ray, castWidth, out hit, castRange.GetFinalValue(), layer))
        {
            dist = hit.distance;
        }       

        Vector3 scale = laserAccess.laserHolder.localScale;

        scale.x = width;
        scale.y = width;
        scale.z = dist * 1.5f;

        laserAccess.laserHolder.localScale = scale;

    }

    private void TrackingTurn()
    {
        if (ownerEntity.target == null) return;
        //if (currentPhase == CastPhase.Active) { return; }
       
        Quaternion lookRotation = Quaternion.LookRotation(ownerEntity.target.transform.position - ownerEntity.transform.position);   

        // Determine Angle Difference between current rotation and desired rotation
        float angleDifference = Quaternion.Angle(ownerEntity.transform.rotation, lookRotation);

        // Scale to 0-1 for time
        float t = angleDifference / 180f;
        // Use Lerp to dynamically adjust the speed depending on angleDifference
        float turnSpeedDegrees = Mathf.Lerp(minTurnSpeedDegrees, maxTurnSpeedDegrees, t);

        float step = turnSpeedDegrees * Time.deltaTime;
        // Actually Rotate The Beholder
        ownerEntity.transform.rotation = Quaternion.RotateTowards(ownerEntity.transform.rotation, lookRotation, step);
    }

    public override BaseEntityAction Clone()
    {
        return new TrackingLaserAction(tickDamage, chargingVisualWidth, activeVisualWidth, chargingVisualColour, activeVisualColour, castWidth, castRange.GetFinalValue(), chargeDuration, activeDuration, isBlockedByEnvironment, doesActionPreventMovement);
    }
}
