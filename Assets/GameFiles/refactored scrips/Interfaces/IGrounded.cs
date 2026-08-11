using UnityEngine;

public interface IGrounded
{
    public GameObject groundCheckCastPoint { get; set; }
    public bool isGrounded { get; set; }
    public LayerMask groundLayer { get; set; }
    float groundCheckDistance { get; set; }
    public void CheckForGrounded();
}
