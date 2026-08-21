using UnityEngine;

public interface IInvulnerable
{
    bool isInvulnerable { get; set; }
    void CheckForInvulnerable();
}
