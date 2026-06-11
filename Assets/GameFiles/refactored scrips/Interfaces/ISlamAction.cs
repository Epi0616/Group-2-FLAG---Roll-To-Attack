using UnityEngine;

public interface ISlamActionRequirements
{
    public LayerMask groundLayer { get; set; }
    public LayerMask pedestalLayer { get; set; }
    public GameObject slamImpactField { get; set; }
}
