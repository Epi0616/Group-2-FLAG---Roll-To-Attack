using UnityEngine;

public interface IBoxCast
{
    bool doesActionPreventMovement { get; set; }
    public float castWidth { get; set; }
    public Stat castRange { get; set; }
    public bool isBlockedByEnvironment { get; set; }
    public float chargeDuration { get; set; }
    public float activeDuration { get; set; }

    public float castInterval { get; set; }
}
