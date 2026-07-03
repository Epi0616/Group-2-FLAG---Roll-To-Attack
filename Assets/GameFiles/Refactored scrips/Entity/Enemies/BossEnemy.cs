using UnityEngine;

public class BossEnemy : BaseAISlamEnemy
{
    protected override void Start()
    {
        base.Start();
        Initialize();
    }
}
