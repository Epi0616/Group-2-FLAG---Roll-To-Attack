using UnityEngine;

public class BaseOrbitObject : MonoBehaviour , IOrbitObject
{
    protected float lifeSpan = 15f;
    protected float radius = 5f;
    protected float speed = 360f;
    protected int damage = 16;
    protected Entity ownerEntity;
    protected Vector3 desiredWorldUp = new Vector3(90, 0, 0);


    public float age = 0;
    protected GameObject anchorObj;
    protected float angle;
    protected Quaternion rotation;
    protected Vector3 offset;
    protected float tempY;
    protected bool isDestroyed = false;

    //public AudioClip[] spikeOnHitSound;

    public void Initialize(Entity ownerEntity, GameObject anchorObj, float radius, float orbitSpeed, int objDamage, float lifetime)
    {
        isDestroyed = false;
        age = 0;
        this.radius = radius;
        speed = orbitSpeed;
        this.ownerEntity = ownerEntity;
        lifeSpan = lifetime;
        this.anchorObj = anchorObj;
        tempY = anchorObj.transform.position.y + 30f;
        damage = objDamage;

    }

    public void UpdateAngle(float angle)
    {
        this.angle = angle;
    }

    protected virtual void Update()
    {
        CheckForExpiration();

        if (ownerEntity == null) return;
        OrbitAnchor();
    }

    protected virtual void OrbitAnchor()
    {
        angle += speed * Time.deltaTime;
        rotation = Quaternion.Euler(0, angle, 0);
        offset = rotation * Vector3.forward * radius;

        if (anchorObj.transform.position.y < tempY) { tempY = anchorObj.transform.position.y; }
        transform.position = new Vector3(anchorObj.transform.position.x, tempY, anchorObj.transform.position.z) + offset;

        Vector3 targetVector = new Vector3(anchorObj.transform.position.x, tempY, anchorObj.transform.position.z);
        transform.LookAt(targetVector, desiredWorldUp);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }

    protected virtual void DamageTarget(Entity entity)
    {
        
    }

    protected virtual void CheckForExpiration()
    {
        age += Time.deltaTime;
        if (!(age >= lifeSpan) && ownerEntity != null) { return; }

        DestroyMe();
    }

    protected virtual void DestroyMe()
    {
        if (isDestroyed) return;
        isDestroyed = true;
        (ownerEntity as IOrbitSpikeSpawner).RemoveObjectFromOrbit(this);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
