using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class WebSpit : BaseEntityAction
{
    private IRadialProjectile radialProjectile;
    private Coroutine actionRoutine;

    public WebSpit() { }

    public WebSpit(bool preventsMovement)
    { 
        this.PreventsMovement = preventsMovement;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);

        if (!(ownerEntity is IRadialProjectile radialProjectile)) { Debug.LogError("ownerEntity is not of type IRadialProjectile"); return; }
        this.radialProjectile = radialProjectile;

        actionRoutine = ownerEntity.StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        yield return Vibrate(1, 0.02f);
        SpawnWeb();

        actionRoutine = null;
        EndAction();
    }

    private void SpawnWeb()
    {
        Vector3 spawnPosition = ownerEntity.transform.position + ownerEntity.transform.forward * 2;
        Quaternion projectileRotation = Quaternion.LookRotation(ownerEntity.transform.forward, Vector3.up);

        GameObject projectile = radialProjectile.radialObj;
        RadialProjectile web = ObjectPoolManager.SpawnObject(projectile, spawnPosition, projectileRotation).GetComponent<RadialProjectile>();
        web.Initialize(ownerEntity, 35, 15);
    }

    private IEnumerator Vibrate(float duration, float intensity)
    {
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            ownerEntity.bodySystem.Vibrate(intensity);
            yield return null;
        }
    }

    public override void InterruptAction()
    {
        if (actionRoutine != null)
        {
            ownerEntity.StopCoroutine(actionRoutine);
            actionRoutine = null;
        }
        EndAction();
    }

    public override void EndAction()
    {
        isComplete = true;
    }

    public override BaseEntityAction Clone()
    {
        return new WebSpit(preventsMovement);
    }
}
