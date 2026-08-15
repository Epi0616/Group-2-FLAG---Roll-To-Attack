using UnityEngine;
public interface ICrashCollider
{
    Collider crashCollider { get; }
    Vector3 crashPosition { get; }
    bool hasCrashed { get; set; }
}
