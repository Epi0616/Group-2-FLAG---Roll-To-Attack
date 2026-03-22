using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerBaseAttackGatherState : PlayerBaseAttackState
{
    protected override void Attack(Collider[] colliders, int collisions)
    {
        List<GameObject> enemies = new();

        for (int i = 0; i < collisions; i++)
        {
            if (!colliders[i].gameObject) { continue; }

            if (colliders[i].gameObject.CompareTag("Enemy"))
            {
                enemies.Add(colliders[i].gameObject);
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

