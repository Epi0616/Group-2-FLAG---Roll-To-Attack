using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnhancedSlowingBubble : MonoBehaviour
{
    private HashSet<Entity> effectedEntities;
    private HashSet<RadialProjectile> effectedProjectiles;

    private Material material;
    private Color color;
    private Color slamColour;
    private float lifeSpan = 10, lifeTimer = 0;
    private float slowMult;
    private Entity ownerEntity;
    private float radius;
    private int enhancementLevel;
    private Vector3 originalScale;
    private bool isDestroyed = false;

    private MaterialPropertyBlock block;
    private MeshRenderer renderer;
    [SerializeField] private MeshRenderer ringRenderer;

    private float hitIntervalTimer = 0;

    protected void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        block = new MaterialPropertyBlock();
    }

    protected virtual void Start()
    {
        effectedEntities = new HashSet<Entity>();
        effectedProjectiles = new HashSet<RadialProjectile>();
        originalScale = transform.localScale;
    }

    protected void Update()
    {
        lifeTimer += Time.deltaTime;
        hitIntervalTimer += Time.deltaTime;
        if (hitIntervalTimer > 0.15f)
        {
            CheckForTargets();
            hitIntervalTimer = 0;
        }
        if (lifeTimer > lifeSpan)
        {
            lifeTimer = 0;
            StartCoroutine(FadeAway());
            StartCoroutine(FadeRingAway());
            //DestroyMe();
            
        }
    }

    protected void FixedUpdate()
    {
        //foreach (Entity entity in effectedEntities)
        //{
        //    entity.statusSystem.ResetStatusByType(StatusType.Slow);
        //}
        if (!isDestroyed)
        {
            foreach (RadialProjectile projectile in effectedProjectiles.ToList())
            {
                if (projectile.gameObject.activeInHierarchy)
                {
                    projectile.speed.SetMultiplier(0.25f);
                }
                else
                {
                    effectedProjectiles.Remove(projectile);
                }
                
            }
        }
        
    }

    private void CheckForTargets()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, ownerEntity.hostileMask);

        foreach (Collider collider in colliders)
        {
            GameObject hit = collider.gameObject;
            if (hit.CompareTag("StaticEntity") || hit.CompareTag("PhysicsEntity")) { continue; }

            Entity hitEntity = hit.GetComponent<Entity>();
            if (hitEntity == ownerEntity) { continue; }
            if (hitEntity == null) { continue; }
            //effectedEntities.Add(hitEntity);
            if (hitEntity.statusSystem.CheckForStatusByType(StatusType.BubbleSlow))
            {
                hitEntity.statusSystem.ResetStatusByType(StatusType.BubbleSlow);
            }
            else
            {
                hitEntity.OnRecieveEffect(new ActiveStatusEffect(new SlowStatus(slowMult, false),
                    new List<BaseCondition> { new TimeCondition(true, 0.2f) }, true));
            }




        }
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.CompareTag("StaticEntity") || hit.CompareTag("PhysicsEntity")) { return; }

        if (hit.TryGetComponent<RadialProjectile>(out RadialProjectile projectile))
        {
            effectedProjectiles.Add(projectile);
        }   
        
    }


    public void OnTriggerExit(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.CompareTag("StaticEntity") || hit.CompareTag("PhysicsEntity")) { return; }

        if (hit.TryGetComponent<RadialProjectile>(out RadialProjectile projectile))
        {
            effectedProjectiles.Remove(projectile);
            projectile.speed.ResetModifiers();
        }
    }

    public virtual void Initialize(Entity entity, float slowMult, float radius, float lifespan, Color colour, int enhancementLevel)
    {
        isDestroyed = false;
        ownerEntity = entity;
        this.slowMult = slowMult;
        color = colour;
        this.radius = radius;
        slamColour = colour;
        this.lifeSpan = lifespan + enhancementLevel;
        this.enhancementLevel = enhancementLevel;
        //if (effectedEntities == null)
        //{
        //    effectedEntities = new HashSet<Entity>();
        //}
        //effectedEntities.Clear();
        if (effectedProjectiles == null)
        {
            effectedProjectiles = new HashSet<RadialProjectile>();
        }
        effectedProjectiles.Clear();

        lifeTimer = 0;
        color.a = 0.5f;
        renderer.GetPropertyBlock(block);
        block.SetFloat("_Opacity", 0.5f);
        renderer.SetPropertyBlock(block);
        ringRenderer.GetPropertyBlock(block);
        block.SetFloat("_Opacity", 1);
        ringRenderer.SetPropertyBlock(block);

        Vector3 tempScale = originalScale;
        tempScale.x = radius;
        tempScale.y = radius * 0.6f;
        tempScale.z = radius;
        transform.localScale = tempScale;

        Vector3 position = transform.position;
        position.y -= 0.2f;
        transform.position = position;
    }

    public virtual void DestroyMe()
    {
        if (isDestroyed) return;
        isDestroyed = true;
        //while (color.a > 0)
        //{
        //    color.a += Time.fixedDeltaTime * -0.2f;
        //    renderer.GetPropertyBlock(block);
        //    block.SetColor("_BaseColor", color);
        //    renderer.SetPropertyBlock(block);
        //}
        //color.a = 0f;
        //block.SetColor("_BaseColour", color);
        //renderer.SetPropertyBlock(block);
        foreach (RadialProjectile projectile in effectedProjectiles)
        {
            projectile.speed.ResetModifiers();
        }
        effectedProjectiles.Clear();
        //foreach (Entity entity in effectedEntities)
        //{
        //    entity.bodySystem.RemoveShaderPowerIncrement(0.34f, 0.25f, ShaderType.Slow);
        //}
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    //public IEnumerator FallIntoFloor()
    //{
    //    float timer = 0;
    //    while (timer < 1.5f)
    //    {
    //        timer += Time.deltaTime;
    //        transform.position = new Vector3(transform.position.x, transform.position.y - 0.03f, transform.position.z);
            
    //        yield return null;
    //    }

    //   DestroyMe();
    //}

    public IEnumerator FadeAway()
    {
        float timer = 0;
        renderer.GetPropertyBlock(block);
        float a = 0.5f;
        // Add fade for Ring mat got from the ring renderer
        while (timer < 0.5f)
        {
            //Debug.Log("Fadomg");
            timer += Time.deltaTime;
            a = Mathf.Clamp01(Mathf.Lerp(0.5f, 0, timer / 0.5f));
            block.SetFloat ("_Opacity", a);
            renderer.SetPropertyBlock(block);
            yield return null;
        }
        DestroyMe();
    }
    public IEnumerator FadeRingAway()
    {
        float timer = 0;
        ringRenderer.GetPropertyBlock(block);
        float a = 1;
        // Add fade for Ring mat got from the ring renderer
        while (timer < 0.5f)
        {
            //Debug.Log("Fadomg");
            timer += Time.deltaTime;
            a = Mathf.Clamp01(Mathf.Lerp(1f, 0, timer / 0.5f));
            block.SetFloat("_Opacity", a);
            ringRenderer.SetPropertyBlock(block);
            yield return null;
        }
    }
}
