using UnityEngine;

public class BaseAISlamEnemy : BaseAIEnemy , ISlamActionRequirements
{
    // ISlamAction Interface Propertires
    [Header("ISlam Required Properties")]
    [SerializeField] GameObject ImpactFieldPrefab;
    [SerializeField] LayerMask PedestalLayer;
    public GameObject slamImpactField { get => ImpactFieldPrefab; set => ImpactFieldPrefab = value; }
    public LayerMask pedestalLayer { get => PedestalLayer; set => PedestalLayer = value; }
}
