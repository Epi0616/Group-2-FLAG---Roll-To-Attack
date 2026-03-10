using System.Collections.Generic;
using UnityEngine;


// this game object this script is attatched to is being preloaded in project settings, i will refine this with a dedicated object pooling when i have the time
public class TemporaryImpactField : MonoBehaviour
{
    private Material material;
    private Color color = new(1, 0, 0, 0);
    private float lifeSpan = 1, lifeTimer = 0;
    private float radius = 0;
    private float alphaDecay;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        material.color = color;
    }

    private void FixedUpdate()
    {
        BecomeTransparent();
    }

    private void BecomeTransparent()
    {
        lifeTimer += Time.fixedDeltaTime;
        material.color = color;
        if (!(lifeTimer >= lifeSpan - 1)) { return; }

        if (color.a > 0)
        {
            color.a += Time.fixedDeltaTime * -alphaDecay;
            return;
        }
        color.a = 0;

        if (!(lifeTimer >= lifeSpan)) { return; }
        Destroy(gameObject);
    }

    public void adjustObject(float radius, float alpha, float alphaDecay, float lifeSpan)
    {
        this.radius = radius;
        color.a = alpha;
        this.lifeSpan = lifeSpan;
        this.alphaDecay = alphaDecay;

        Vector3 tempScale = transform.localScale;
        tempScale.x = radius * 2;
        tempScale.z = radius * 2;
        transform.localScale = tempScale;

        Vector3 position = transform.position;
        position.y -= 0.5f;
        transform.position = position;
    }
}
