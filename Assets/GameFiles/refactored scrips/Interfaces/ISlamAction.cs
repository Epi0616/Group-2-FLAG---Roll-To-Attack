using UnityEngine;

public interface ISlamActionRequirements
{
    public float slamBaseRange {  get; set; }
    public Vector3 slamPositionOffset { get; set; }
    public Color defaultSlamColour { get; set; }
    public float slamChargeUpTime { get; set; }
    public LayerMask environmentMask { get; set; }
    public GameObject DebugSlamObj { get; set; }
    public GameObject SPAWNTHING(GameObject thing, Vector3 pos);
}
