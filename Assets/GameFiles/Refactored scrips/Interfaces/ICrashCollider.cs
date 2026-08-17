using UnityEngine;
public interface ICrashCollider
{
    LayerMask crashLayerMask { get; }
    Collider crashCollider { get; }
    Vector3 crashPosition { get; }
    bool hasCrashed { get; set; }
}
