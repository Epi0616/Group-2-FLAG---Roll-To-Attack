using UnityEngine;

public interface IGrounded
{
    public bool isGrounded { get; set; }
    public LayerMask environmentMask { get; set; }
    public void CheckForGrounded();
}
