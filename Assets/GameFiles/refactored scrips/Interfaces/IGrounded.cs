using UnityEngine;

public interface IGrounded
{
    public bool isGrounded { get; set; }
    public LayerMask groundLayer { get; set; }
    public void CheckForGrounded();
}
