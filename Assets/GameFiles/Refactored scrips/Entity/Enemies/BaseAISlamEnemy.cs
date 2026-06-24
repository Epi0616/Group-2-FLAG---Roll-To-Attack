using UnityEngine;

public class BaseAISlamEnemy : BaseAIEnemy , ISlamActionWithCooldown
{
    // ISlamAction Interface Propertires
    [Header("ISlam Required Properties")]
    [SerializeField] GameObject ImpactFieldPrefab;
    [SerializeField] LayerMask PedestalLayer;
    [SerializeField] private float slamCooldownTime = 0.5f;
    public float cooldownTime { get => slamCooldownTime; set => slamCooldownTime = value; }
    public GameObject slamImpactField { get => ImpactFieldPrefab; set => ImpactFieldPrefab = value; }
    public LayerMask pedestalLayer { get => PedestalLayer; set => PedestalLayer = value; }
}
