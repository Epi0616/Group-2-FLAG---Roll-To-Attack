using UnityEngine;

public interface IStunable
{
    public bool isStunned { get; set; }
    public bool canBeStunned { get; set; }
    float stunInterval { get; set; }
    float currentStunInterval { get; set; }
    void CheckForStunned();
    void ResetStunInterval();
}
