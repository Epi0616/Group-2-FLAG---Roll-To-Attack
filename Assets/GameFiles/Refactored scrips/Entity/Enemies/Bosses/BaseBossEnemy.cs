using UnityEngine;
using System;
using System.Collections.Generic;
using NUnit.Framework;

public class BaseBossEnemy : BaseAISlamEnemy,
    IBoss,
    IShieldable
{
    [Header("IShieldable")]
    public int initialShieldStacks { get; set; }
    public int currentShieldStacks { get; set; }
    public void HandleUpdateShieldStacks()
    {
        UpdateShields?.Invoke();
    }

    //IBoss
    public static event Action<BaseBossEnemy> BossEnable;
    public static event Action<BaseBossEnemy> BossDisable;
    public event Action<List<MilestoneDescriptor>> SetMilestones;
    public event Action UpdateHealth;
    public event Action UpdateShields;

    [SerializeField] private List<MilestoneDescriptor> Milestones;
    public List<MilestoneDescriptor> milestones { get => Milestones; set => Milestones = value; }

    public void HandleEnable()
    {
        BossEnable?.Invoke(this);
    }

    public void HandleDisable()
    { 
        BossDisable?.Invoke(this);
    }

    public void HandleSetMilestones()
    {
        SetMilestones?.Invoke(milestones);
    }

    public void HandleUpdateHealth()
    {
        UpdateHealth?.Invoke();
    }

    public void HandleUpdateShields()
    { 
        UpdateShields?.Invoke();
    }
}
