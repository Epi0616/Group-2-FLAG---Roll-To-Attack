using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PackUpArenaManager : MonoBehaviour
{
    private void OnEnable()
    {
        PauseMenu.ReturnToIntro += ReturnActiveEntitiesToPool;
    }
    private void OnDisable()
    {
        PauseMenu.ReturnToIntro -= ReturnActiveEntitiesToPool;
    }

    public void ReturnActiveEntitiesToPool()
    {
        List<GameObject> activeEntities = ObjectPoolManager.activeObjects.ToList();

        for (int i = activeEntities.Count - 1; i >= 0; i--)
        {
            GameObject obj = activeEntities[i];
            if (obj == null) continue;
            if (obj.TryGetComponent<Entity>(out Entity entity))
            {
                if (entity is IActionable temp)
                {
                    temp.actionController.InterruptAllActive();
                }
                entity.statusSystem.currentActiveStatusEffects.Clear();
            }
            ObjectPoolManager.ReturnObjectToPool(obj);
        }
    }
}
