using UnityEngine;
public interface ICastRequirements
{
    public LayerMask environmentLayer { get; set; }
    public Transform castOriginTransform { get; set; }
}
