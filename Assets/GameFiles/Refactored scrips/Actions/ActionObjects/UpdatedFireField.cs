using UnityEngine;
using System.Collections;

public class UpdatedFireField : FireField
{
    [SerializeField] private MeshRenderer[] VFXRenderers;
    private MaterialPropertyBlock block;

    protected override void Awake()
    {
        block = new MaterialPropertyBlock();
        SetTiling();
    }

    public override void Initialize(Entity ownerEntity, Color color, float radius, int initialDamage, int tickDamage, float lifespan, float tickRate)
    {
        
        this.ownerEntity = ownerEntity;
        this.radius = radius;
        this.color = color;
        this.initialDamage = initialDamage;

        //this.color.a = 0.3f;
       // AdjustColors();
        AdjustScale(radius);

        foreach (MeshRenderer r in VFXRenderers)
        {
            r.GetPropertyBlock(block);
            block.SetFloat("_Opacity", 1);
            r.SetPropertyBlock(block);
        }

        StartCoroutine(TickDamage(lifespan, tickRate, tickDamage));
    }

    private void SetTiling()
    {
        foreach (MeshRenderer  r in VFXRenderers)
        {
            float x = Random.Range(0f, 1f);
            float y = Random.Range(0f, 1f);
            r.GetPropertyBlock(block);
            block.SetVector("_Offset", new Vector4(x, y, 0, 0));
            r.SetPropertyBlock(block);
        }
    }

    protected override void AdjustColors()
    {
        //Mathf.Clamp01(color.a);

        //Color darkerColour = new Color(color.r * 0.7f, color.g * 0.7f, color.b * 0.7f, color.a);
        //Color lighterColour = new Color(color.r * 1.2f, color.g * 1.2f, color.b * 1.2f, color.a);

        //material.color = darkerColour;
        //ringMaterial.SetColor("_RingColour", lighterColour * 3);
        //ringMaterial.SetFloat("_Opacity", color.a);
    }

    protected override IEnumerator FadeAway()
    {
        float timer = 0;
        float a = 1;
        // Add fade for Ring mat got from the ring renderer
        while (timer < 0.5f)
        {
            //Debug.Log("Fadomg");
            timer += Time.deltaTime;
            a = Mathf.Clamp01(Mathf.Lerp(1f, 0, timer / 0.5f));
            foreach (MeshRenderer r in VFXRenderers)
            {
                r.GetPropertyBlock(block);
                block.SetFloat("_Opacity", a);
                r.SetPropertyBlock(block);
            }
            yield return null;
        }
    }
}
