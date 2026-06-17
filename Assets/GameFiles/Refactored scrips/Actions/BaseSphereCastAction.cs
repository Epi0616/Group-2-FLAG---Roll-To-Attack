using UnityEngine;
using System;
using System.Collections.Generic;

public interface IBoxCast
{
    bool doesActionPreventMovement { get; set; }
    public float castWidth {  get; set; }
    public Stat castRange { get; set; }
    public bool isBlockedByEnvironment { get; set; }
    public float chargeDuration { get; set; }
    public float activeDuration { get; set; }

    public float castInterval { get; set; }
}

public interface ICastRequirements
{
    public LayerMask environmentLayer {  get; set; }
    public Transform castOriginTransform { get; set; }
}
[Serializable]
public class BaseSphereCastAction : BaseEntityAction , IBoxCast
{
    // ISphereCast Requirements
    [SerializeField] protected float CastBoxCheckWidth;
    [SerializeField] protected Stat CastRange = new Stat(10);
    [SerializeField] protected float ChargeDuration;
    [SerializeField] protected float ActiveDuration;
    [SerializeField] protected float CastInterval;
    [SerializeField] protected bool IsCastBlockedByEnvironment;
    [SerializeField] protected bool DoesActionPreventMovement;
    
    public float castWidth { get => CastBoxCheckWidth; set => CastBoxCheckWidth = value; }
    public Stat castRange { get => CastRange; set => CastRange = value; }
    public float chargeDuration { get => ChargeDuration; set => ChargeDuration = value; }
    public float activeDuration { get => ActiveDuration; set => ActiveDuration = value; }
    public float castInterval { get => CastInterval; set => CastInterval = value; }
    public bool isBlockedByEnvironment { get => IsCastBlockedByEnvironment; set => IsCastBlockedByEnvironment = value; }
    public bool doesActionPreventMovement { get => DoesActionPreventMovement; set => DoesActionPreventMovement = value; }
    

    protected Transform castOrigin;
    protected float castHeight = 10f;
    protected bool attackInterrupted;
    protected float currentTimer;
    protected RaycastHit hit;
    protected ICastRequirements castAccess;
    protected LayerMask castLayer;
    protected enum CastPhase { Charging, Active, Complete }
    protected CastPhase currentPhase;

    public BaseSphereCastAction() { }

    public BaseSphereCastAction(float castWidth, float castRange, float chargeDuration, float activeDuration, bool isBlockedByEnvironment, bool doesActionPreventMovement) 
    {
        this.castWidth = castWidth;
        this.castRange = new Stat(castRange);
        this.chargeDuration = chargeDuration;
        this.activeDuration = activeDuration;
        this.isBlockedByEnvironment = isBlockedByEnvironment;
        preventsMovement = doesActionPreventMovement;
    }

    public override void StartAction(Entity ownerEntity)
    {
        castLayer = LayerMask.GetMask("Ground");
        base.StartAction(ownerEntity);
        attackInterrupted = false;
        currentTimer = 0;
        castAccess = ownerEntity as ICastRequirements;
        castOrigin = castAccess.castOriginTransform;
        if (isBlockedByEnvironment)
        {
            castLayer = castAccess.environmentLayer | ownerEntity.hostileMask;
        }
        else
        {
            castLayer = ownerEntity.hostileMask;
        }

        if (castAccess == null)
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

    public override void UpdateAction()
    {
        if (attackInterrupted || ownerEntity == null) { return; }
        currentTimer += Time.deltaTime;

        switch (currentPhase)
        {
            case CastPhase.Charging:
                if (currentTimer > chargeDuration)
                {
                    currentTimer = 0;
                    currentPhase = CastPhase.Active;
                    CastChargeFinished();
                    CastActiveStarted();   
                }
                CastChargeUpdate();
                break;
            case CastPhase.Active:
                if (currentTimer > activeDuration)
                {                
                    currentPhase = CastPhase.Complete;
                    EndAction();
                }
                CastActiveUpdate();
                break;
        }

    }
    public override void FixedUpdateAction() { }
    public override void InterruptAction() { }
    public override void EndAction() { isComplete = true; }

    protected virtual Vector3 GetCastDirection() { return ownerEntity.transform.forward; }
    protected virtual Vector3 GetBoxExtents() { return new Vector3(castWidth, castHeight, 1f); }
    protected virtual Vector3 GetCastOrigin() { return new Vector3(castOrigin.position.x, castOrigin.position.y - castHeight / 2, castOrigin.position.z); }
    protected virtual Ray UpdateRay() { return new Ray(castOrigin.position, GetCastDirection()); }
    protected virtual void ProcessHit(RaycastHit hit) { }

    protected virtual void PerformActiveBoxCast(LayerMask layer)
    {
        Ray ray = UpdateRay();
        RaycastHit hit;
        Physics.BoxCast(GetCastOrigin(), GetBoxExtents(), GetCastDirection(), out hit, ownerEntity.transform.rotation, castRange.GetFinalValue(), layer);

    }

    protected virtual void PerformActivePiercingBoxCast(LayerMask layer)
    {
        Ray ray = UpdateRay();
        RaycastHit[] hits = Physics.BoxCastAll(GetCastOrigin(), GetBoxExtents(), GetCastDirection(), ownerEntity.transform.rotation, castRange.GetFinalValue(), layer);

    }

    protected virtual void CastChargeStarted() { }
    protected virtual void CastChargeFinished() { }
    protected virtual void CastActiveStarted() { }
    protected virtual void CastChargeUpdate() { }
    protected virtual void CastActiveUpdate() { }
    
    public override BaseEntityAction Clone()
    {
        return new BaseSphereCastAction(castWidth, castRange.GetFinalValue(), chargeDuration, activeDuration, isBlockedByEnvironment, doesActionPreventMovement);
    }
}
