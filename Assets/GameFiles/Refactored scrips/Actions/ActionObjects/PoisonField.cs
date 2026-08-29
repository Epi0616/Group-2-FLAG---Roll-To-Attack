using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class PoisonField : MonoBehaviour
{
    //protected Material material;
    //protected Material ringMaterial;
    //[SerializeField] protected MeshRenderer ringMeshRenderer;
    protected Color color;
    protected Color slamColour;
    protected float lifeSpan = 10, lifeTimer = 0;
    protected float damageTickTimer = 0;//, currentTickCount = 0;
    protected float radius = 0;
    protected Entity ownerEntity;
    protected int poisonTickDMG;
    //public AudioClip[] poisonTickSound;

    private MaterialPropertyBlock block;
    [SerializeField] private MeshRenderer[] VFXRenderers;
    [SerializeField] protected DecalProjector[] VFXProjectors;

    protected void Awake()
    {
        block = new MaterialPropertyBlock();
    }

    protected virtual void Start()
    {
        DealDamage();
    }

    protected void FixedUpdate()
    {
        TickDamage();
        BecomeTransparent();
    }

    protected void BecomeTransparent()
    {
        lifeTimer += Time.fixedDeltaTime;

        if (!(lifeTimer >= lifeSpan - 1)) { return; }

        StartCoroutine(FadeAway());
        
    }

    protected void SetTiling()
    {
        foreach (MeshRenderer r in VFXRenderers)
        {
            float x = Random.Range(0f, 1f);
            float y = Random.Range(0f, 1f);
            r.GetPropertyBlock(block);
            block.SetVector("_Offset", new Vector4(x, y, 0, 0));
            r.SetPropertyBlock(block);
        }
    }
    protected void SetDecalTiling()
    {
        foreach (DecalProjector r in VFXProjectors)
        {
            float x = Random.Range(0f, 1f);
            float y = Random.Range(0f, 1f);
            r.material.SetVector("_Offset", new Vector4(x, y, 0, 0));
        }
    }


    protected void AdjustColours(Color color, float alpha)
    {
        //Mathf.Clamp01(color.a);

        //Color darkerColour = new Color(color.r * 0.8f, color.g * 0.8f, color.b * 0.8f, alpha);
        //Color lighterColour = new Color(color.r * 0.9f, color.g * 0.9f, color.b * 0.9f, alpha);

        VFXRenderers[0].GetPropertyBlock(block);
        block.SetFloat("_Opacity", 0.5f);
        VFXRenderers[0].SetPropertyBlock(block);
        VFXRenderers[1].GetPropertyBlock(block);
        block.SetFloat("_Opacity", 0.6f);
        VFXRenderers[1].SetPropertyBlock(block);
        VFXRenderers[2].GetPropertyBlock(block);
        block.SetFloat("_Opacity", 0.3f);
        VFXRenderers[2].SetPropertyBlock(block);

        //foreach (MeshRenderer r in VFXRenderers)
        //{
        //    r.GetPropertyBlock(block);
        //    //block.SetColor("_BaseColour", color * Random.Range(0.8f, 0.9f));
        //    block.SetFloat("_Opacity", alpha);
        //    r.SetPropertyBlock(block);
        //}


        //material.color = darkerColour;
        //ringMaterial.SetColor("_RingColour", lighterColour * 3);
        //ringMaterial.SetFloat("_Opacity", color.a);
    }

    protected void AdjustDecalOpacity()
    {
        //VFXProjectors[0].fadeFactor = 0.5f;
        //VFXProjectors[1].fadeFactor = 0.6f;
        //VFXProjectors[2].fadeFactor = 0.3f;
        foreach (DecalProjector p in VFXProjectors)
        {
            p.fadeFactor = 1f;
        }
    }

    protected IEnumerator FadeAway()
    {
        float timer = 0;
        float a = 1;
        // Add fade for Ring mat got from the ring renderer
        while (timer < 0.5f)
        {
            //Debug.Log("Fadomg");
            timer += Time.deltaTime;
            
            //foreach (MeshRenderer r in VFXRenderers)
            //{
            //    r.GetPropertyBlock(block);
            //    a = Mathf.Clamp01(Mathf.Lerp(block.GetFloat("_Opacity"), 0, timer));
            //    block.SetFloat("_Opacity", a);
            //    r.SetPropertyBlock(block);
            //}

            foreach (DecalProjector p in VFXProjectors)
            {               
                p.fadeFactor = Mathf.Clamp01(Mathf.Lerp(1, 0, (timer / 0.5f)));
                
            }
            yield return null;
        }
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    protected virtual void TickDamage()
    {
        damageTickTimer += Time.fixedDeltaTime;
        if (!(damageTickTimer >= 1)) { return; }
        DealDamage();
        damageTickTimer = 0;
    }

    protected virtual void DealDamage()
    {
        if (ownerEntity == null) return;
        //if (!(currentTickCount < 10)) { return; }
        //currentTickCount++;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, ownerEntity.hostileMask);

        foreach (var collider in colliders)
        {
            if (!collider.gameObject) { continue; }
            if (collider.gameObject == ownerEntity) { continue; }
            if (collider.TryGetComponent<Entity>(out Entity entity))
            { 
                entity.OnTakeDamage(poisonTickDMG, slamColour, DamageType.Poison);

                //if (collider.gameObject.CompareTag("EntitySpawnable")) { continue; } 

                //AudioManager.instance.PlayRandomSoundClip(poisonTickSound, new Vector3(0, 0, 0), 0.6f);
                //Debug.Log("dealing damage");      
            }

        }
    }

    public virtual void Initialize(Entity entity, float radius, float lifespan, int tickDamage, Color colour)
    {
        ownerEntity = entity;
        //SetTiling();

        this.radius = radius;
        damageTickTimer = 0;
        //currentTickCount = 0;
        poisonTickDMG = tickDamage;

        color = colour;
        slamColour = colour;
        this.lifeSpan = lifespan;

        lifeTimer = 0;
        //AdjustColours(color, 1);
        AdjustDecalOpacity();

        Vector3 tempScale = transform.localScale;
        tempScale.x = radius * 2;
        tempScale.z = radius * 2;
        transform.localScale = tempScale;

        VFXProjectors[0].size = new Vector3(radius * 2, radius * 2, VFXProjectors[0].size.z);
        VFXProjectors[1].size = new Vector3((radius * 2) + 5, (radius * 2) + 5, VFXProjectors[1].size.z);
        VFXProjectors[2].size = new Vector3(radius * 2, radius * 2, VFXProjectors[2].size.z);


        Vector3 position = transform.position;
        position.y -= 0.5f;
        transform.position = position;
    }
}
