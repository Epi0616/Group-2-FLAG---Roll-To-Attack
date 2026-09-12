using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PackUpArenaManager : MonoBehaviour
{
    private void OnEnable()
    {
        PauseMenu.PackUpScene += PackUp;
        GameOverMenu.PackUpScene += PackUp;
    }
    private void OnDisable()
    {
        PauseMenu.PackUpScene -= PackUp;
        GameOverMenu.PackUpScene -= PackUp;
    }

    private void PackUp()
    {
        ReturnActiveEntitiesToPool();
    }

    private void ReturnActiveEntitiesToPool()
    {
        List<GameObject> activeEntities = ObjectPoolManager.activeObjects.ToList();

        for (int i = activeEntities.Count - 1; i >= 0; i--)
        {
            GameObject obj = activeEntities[i];
            if (obj == null) continue;
            //if (!obj.activeSelf) continue;
            if (obj.TryGetComponent<Entity>(out Entity entity))
            {
                entity.healthSystem.isDead = true;
                if (entity is IActionable actionable)
                {
                    actionable.actionController.InterruptAllActive();
                }

                entity.statusSystem.currentActiveStatusEffects.Clear();
                entity.StopAllCoroutines();
            }

            ObjectPoolManager.ReturnObjectToPool(obj);
        }
    }
}
