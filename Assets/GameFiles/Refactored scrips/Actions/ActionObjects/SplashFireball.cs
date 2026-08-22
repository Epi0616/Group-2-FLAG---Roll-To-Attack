using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SplashFireball : ArcingProjectile
{
    [SerializeField] private GameObject radialObj;
    private int amount = 10;
    private float radius = 0;
    private float debrisDistance = 50f;
    private float debrisSpeed = 10f;

    protected override IEnumerator LifeTime(float lifeTime)
    {
        yield return null;
    }

    protected override IEnumerator PathToTarget(Vector3 target, Vector3 initialPosition, float durationOfTravel)
    {
        float timer = durationOfTravel;
        float t = 0;

        Vector3 position;

        float peakInArc = GetPeakInArc(target);
        if (currentShadowDecal != null)
        {
            currentShadowDecal.StartGrowAndShrink(0.6f, durationOfTravel);
        }
        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (durationOfTravel - timer) / durationOfTravel;

            position = Vector3.Lerp(initialPosition, target, t);
            position.y += ArcY(target, initialPosition, peakInArc, t);

            LookToNextPosition(position);
            transform.position = position;

            yield return null;
        }
        transform.position = target;
    }

    private void LookToNextPosition(Vector3 nextPosition)
    {
        Vector3 direction = nextPosition - transform.position;

        if (direction.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public override void Slam()
    {
        base.Slam();
        currentShadowDecal.DestroyMe();
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    public override void OnHitEffect()
    {
        Vector3 direction = Vector3.forward;

        for (int i = 0; i < amount; i++)
        {
            float angle = ((360f / amount) * i);

            Vector3 projectileDirection = Quaternion.AngleAxis(angle, Vector3.up) * direction;
            Vector3 spawnPosition = transform.position + (projectileDirection * radius);
            Quaternion projectileRotation = Quaternion.LookRotation(projectileDirection, Vector3.up);

            GameObject projectile = radialObj;
            RadialProjectile wingbeat = ObjectPoolManager.SpawnObject(projectile, spawnPosition, projectileRotation).GetComponent<RadialProjectile>();

            //if (wingbeat is IDecalShadowCast shadow)
            //{
            //    shadow.currentShadowDecal = ObjectPoolManager.SpawnObject(shadow.shadowDecalPrefab, wingbeat.transform.position, new Quaternion(1, 0, 0, 1)).GetComponent<ShadowDecal>();
            //    shadow.currentShadowDecal.SetupProjector(new Vector2(3f, 3f), new Quaternion(1, 0, 0, 1), new Vector3(0, 0, 0), true, false, wingbeat.gameObject);
            //}

            wingbeat.Initialize(ownerEntity, debrisDistance, debrisSpeed);
        }
    }
}
