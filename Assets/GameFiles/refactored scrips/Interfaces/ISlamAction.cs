using UnityEngine;

public interface ISlamActionRequirements
{
    public LayerMask groundLayer { get; set; }
    public GameObject SlamImpactField { get; set; }
}
