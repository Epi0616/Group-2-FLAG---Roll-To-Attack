using UnityEngine;
using UnityEngine.VFX;

public interface ILaserRequirements : ICastRequirements
{
    public VisualEffect laserVFX { get; set; }
    public Transform laserHolder { get; set; }
}

