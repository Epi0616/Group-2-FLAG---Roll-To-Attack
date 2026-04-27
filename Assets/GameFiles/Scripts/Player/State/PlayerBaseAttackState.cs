using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class PlayerBaseAttackState : PlayerMovementState
{
    protected Color myColor = Color.red;
    protected float myRadius;
    protected bool attacked;
    protected AudioClip[] impactSounds;

    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);
        impactSounds = player.playerLightAttackSounds;
        myRadius = player.baseRadiusSize.GetFinalValue();
        attacked = false;
    }
    public override void FixedUpdateState()
    {
        Vector3 targetVelocity = moveDirection * player.moveSpeed.GetFinalValue();

        if (!player.isGrounded)
        {
            targetVelocity.y = player.rb.linearVelocity.y - player.impactSpeed.GetFinalValue();
        }
        
        player.rb.linearVelocity = targetVelocity;

        if (!player.isGrounded) { return; }
        ImpactGround();
    }
    protected virtual void ImpactGround()
    {
        if (attacked) { return; }
        attacked = true;

        PlayImpactSound();

        float magnitude = player.impactSpeed.GetFinalValue() / player.impactSpeed.GetBaseValue() * 2;
        player.AddScreenShake(magnitude);

        Collider[] colliders = new Collider[100];
        int collisions = Physics.OverlapSphereNonAlloc(player.rb.position, myRadius, colliders, player.enemyLayer);
        Attack(colliders, collisions);

        ResetAttackModifiers();
        player.SwitchState(new PlayerMovementState());
    }


    protected virtual void PlayImpactSound()
    {
        if (player.impactSpeed.GetFinalValue() > player.impactSpeed.GetBaseValue())
        {
            //float volumePercent = Mathf.Clamp01(player.impactSpeed.GetFinalValue() / player.impactSpeed.GetBaseValue() - 1);
            AudioManager.instance.PlayRandomSoundClip(player.playerHeavyAttackSounds, new Vector3(0, 0, 0), 0.2f);
        }
        else 
        {
            AudioManager.instance.PlayRandomSoundClip(impactSounds, new Vector3(0, 0, 0), 0.4f);
        }
    }

    protected virtual void Attack(Collider[] colliders, int collisions)
    {
        List<GameObject> Enemies = new();
        for (int i = 0; i < collisions; i++)
        {
            if (!colliders[i].gameObject) { continue; }

            if (colliders[i].gameObject.CompareTag("Enemy"))
            {
                Enemies.Add(colliders[i].gameObject);
            }
        }

        foreach (var Enemy in Enemies)
        {
            if (Enemy == null) continue;
            CustomAttack(Enemy);
            ApplyKnockback(Enemy);
        }
        CustomDisplayAttack();
    }

    protected void ApplyKnockback(GameObject Enemy)
    {
        if (player.impactSpeed.GetFinalValue() > player.impactSpeed.GetBaseValue())
        { 
            EnemyStateController enemy = Enemy.GetComponent<EnemyStateController>();
            float knockbackForce = player.impactSpeed.GetFinalValue() / player.impactSpeed.GetBaseValue();
            //enemy.OnTakeKnockback(player.transform.position, knockbackForce * 2);
           
            enemy.OnRecieveEffect(new ActiveStatusEffect(new KnockbackEffect(player.transform.position, knockbackForce * 2f),
                new List<BaseCondition> { new NavMeshReturnCondition(true, enemy), new DurationCondition(true, 0.75f) } ));
        }
    }

    private void ResetAttackModifiers()
    {
        player.jumpHeight.ResetModifiers();
        player.impactSpeed.ResetModifiers();
        player.baseRadiusSize.ResetModifiers();
    }

    protected virtual void CustomDisplayAttack() { }
    protected virtual void CustomAttack(GameObject Enemy) { }
    //check for attack action pressed needs to do nothing so you cant attack while in the process of performing the current attack
}

