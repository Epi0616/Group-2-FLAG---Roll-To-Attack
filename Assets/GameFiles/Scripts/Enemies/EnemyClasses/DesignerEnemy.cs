using UnityEngine;

public class DesignerEnemy : EnemyStateController
{
    // This enemy has access to all of the public/protected variables in the base controller
    // Stuff such as health, speed etc will all be visible in editor you don't need to define or change those here

    // Enemies are automatically going to use the previous states for movement so you also don't need to write that at all
    // All that needs filling in is the Attack itself and other specific functionality/variables you want



    // This function is called whenever the enemy enters the attack state
    public override void Attack()
    {

    }
    // This function is called whenever the enemy leaves the attack state
    public override void CompleteAttack()
    {

    }


}
    