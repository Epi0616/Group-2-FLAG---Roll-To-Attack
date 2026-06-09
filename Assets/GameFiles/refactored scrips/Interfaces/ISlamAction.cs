using UnityEngine;

public interface ISlamActionRequirements
{
    public LayerMask environmentMask { get; set; }
    public GameObject SlamImpactField { get; set; }
    public GameObject SPAWNTHING(GameObject thing, Vector3 pos);
}
