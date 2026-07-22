using System.Collections.Generic;
using UnityEngine;


// this game object this script is attatched to is being preloaded in project settings, i will refine this with a dedicated object pooling when i have the time
public class TemporaryImpactField : MonoBehaviour
{
    private Material material;
    private Material ringMaterial;
    [SerializeField] private MeshRenderer ringRenderer;
    private Color color = new(1, 0, 0, 0);
    private float lifeSpan = 1, lifeTimer = 0;
    private float radius = 0;
    private float alphaDecay;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        ringMaterial = ringRenderer.material;
        AdjustColours(color);
    }

    private void FixedUpdate()
    {
        BecomeTransparent();
    }

    private void BecomeTransparent()
    {
        lifeTimer += Time.fixedDeltaTime;
        AdjustColours(color);
        if (!(lifeTimer >= lifeSpan - 1)) { return; }

        if (color.a > 0)
        {
            color.a += Time.fixedDeltaTime * -alphaDecay;
            return;
        }
        color.a = 0;

        if (!(lifeTimer >= lifeSpan)) { return; }
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    protected void AdjustColours(Color color)
    {
        Color darkerColour = new Color(color.r * 0.7f, color.g * 0.7f, color.b * 0.7f, color.a);
        Color lighterColour = new Color(color.r * 1.2f, color.g * 1.2f, color.b * 1.2f, color.a);
        material.color = darkerColour;
        ringMaterial.SetColor("_RingColour", lighterColour);
        if (color.a < 0f) { color.a = 0; }
        else if (color.a > 1f) { color.a = 1f; }
        ringMaterial.SetFloat("_Opacity", color.a);
    }

    public void adjustObject(float radius, float alpha, float alphaDecay, float lifeSpan)
    {
        this.radius = radius;
        color.a = alpha;
        this.lifeSpan = lifeSpan;
        this.alphaDecay = alphaDecay;
        lifeTimer = 0;

        Vector3 tempScale = transform.localScale;
        tempScale.x = radius * 2;
        tempScale.z = radius * 2;
        transform.localScale = tempScale;

        Vector3 position = transform.position;
        //position.y -= 0.5f;
        transform.position = position;
    }
}
