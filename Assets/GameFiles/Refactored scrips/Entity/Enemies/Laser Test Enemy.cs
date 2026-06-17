using UnityEngine;
using UnityEngine.VFX;

public class LaserTestEnemy : BaseAIEnemy , ILaserRequirements
{
    [SerializeField] private LayerMask EnvironmentLayer;
    public LayerMask environmentLayer { get => EnvironmentLayer; set => EnvironmentLayer = value; }
    [SerializeField] private Transform FiringOrigin;
    public Transform castOriginTransform { get => FiringOrigin; set => FiringOrigin = value; }

    [SerializeField] private VisualEffect LaserVFX;
    public VisualEffect laserVFX { get => LaserVFX; set => LaserVFX = value; }
    [SerializeField] private Transform LaserHolderTransform;
    public Transform laserHolder { get => LaserHolderTransform; set => LaserHolderTransform = value; }
}
