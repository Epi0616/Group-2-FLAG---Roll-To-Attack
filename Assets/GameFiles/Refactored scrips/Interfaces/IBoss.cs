using NUnit.Framework;
using System.Collections.Generic;
using System;

public interface IBoss
{
    static event Action<BaseBossEnemy> BossEnable;
    static event Action<BaseBossEnemy> BossDisable;
    event Action<List<MilestoneDescriptor>> SetMilestones;
    event Action UpdateHealth;
    event Action UpdateShields;

    List<MilestoneDescriptor> milestones { get; set; }

    void HandleEnable();
    void HandleDisable();
    void HandleSetMilestones();
    void HandleUpdateHealth();
    void HandleUpdateShields();
}
