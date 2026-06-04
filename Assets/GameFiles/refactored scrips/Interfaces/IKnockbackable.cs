using UnityEngine;

public interface IKnockbackable
{
    public Stat knockbackWeightMod {  get; set; }
    public Stat slammedDamageMod { get; set; }
    public bool isBeingDisplaced { get; set; }

    public bool CheckForDisplacement();
}
