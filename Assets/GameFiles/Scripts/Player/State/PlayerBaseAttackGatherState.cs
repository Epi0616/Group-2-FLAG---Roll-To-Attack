using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerBaseAttackGatherState : PlayerBaseAttackState
{
    protected override void Attack(Collider[] colliders)
    {
        List<GameObject> enemies = new();
        foreach (var collider in colliders)
        {
            if (!collider.gameObject) { continue; }

            if (collider.gameObject.CompareTag("Enemy"))
            {
                enemies.Add(collider.gameObject);
            }
        }

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            ApplyKnockback(enemy);
        }

        CustomAttack(enemies);
        CustomDisplayAttack();
    }

    protected virtual void CustomAttack(List<GameObject> Enemy) { }
}

