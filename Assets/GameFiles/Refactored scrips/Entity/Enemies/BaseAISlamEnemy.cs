using UnityEngine;

public class BaseAISlamEnemy : BaseAIEnemy , ISlamActionRequirements
{
    // ISlamAction Interface Propertires
    [Header("ISlam Required Properties")]
    [SerializeField] GameObject ImpactFieldPrefab;
    public GameObject slamImpactField { get => ImpactFieldPrefab; set => ImpactFieldPrefab = value; }
}
