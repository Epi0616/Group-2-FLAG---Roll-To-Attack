using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PackUpArenaManager : MonoBehaviour
{
    private void OnEnable()
    {
        PauseMenu.PackUpScene += ReturnActiveEntitiesToPool;
        GameOverMenu.PackUpScene += ReturnActiveEntitiesToPool;
    }
    private void OnDisable()
    {
        PauseMenu.PackUpScene -= ReturnActiveEntitiesToPool;
        GameOverMenu.PackUpScene -= ReturnActiveEntitiesToPool;
    }

    public void ReturnActiveEntitiesToPool()
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
                //if (entity is IMoveable moveable)
                //{ 
                //    moveable.movementController.
                //}
                entity.statusSystem.currentActiveStatusEffects.Clear();
                ObjectPoolManager.ReturnObjectToPool(obj);
            }  
        }
    }
}
