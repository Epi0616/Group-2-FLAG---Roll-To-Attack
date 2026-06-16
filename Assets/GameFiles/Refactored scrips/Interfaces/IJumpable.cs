using UnityEngine;

public interface IJumpable
{
    Stat jumpHeight { get; set; }
    Stat jumpSpeed { get; set; }
    Stat impactSpeed { get; set; }
    bool isJumping { get; set; }
    bool canJump { get; set; }

    void CheckForCanJump();
}
