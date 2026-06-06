using UnityEngine;

public interface ISlamActionRequirements
{
    public float slamBaseRange {  get; set; }
    public Vector3 slamPositionOffset { get; set; }
    public Color defaultSlamColour { get; set; }
    public float slamChargeUpTime { get; set; }
    public LayerMask environmentMask { get; set; }
}
