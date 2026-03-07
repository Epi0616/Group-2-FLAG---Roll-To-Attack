using System.Collections.Generic;
using UnityEngine;


// this game object this script is attatched to is being preloaded in project settings, i will refine this with a dedicated object pooling when i have the time
public class TemporaryImpactField : MonoBehaviour
{
    private Material material;
    private Color color = new(1, 0, 0, 1);
    private float lifeSpan = 1, lifeTimer = 0;
    private float damageTickTimer = 0, currentTickCount = 0;
    private float radius = 0;


    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        color.a = 1f;
        material.color = color;
    }

    private void FixedUpdate()
    {
        BecomeTransparent();
    }

    private void BecomeTransparent()
    {
        lifeTimer += Time.fixedDeltaTime;

        if (!(lifeTimer >= lifeSpan - 1)) { return; }
        material.color = color;

        if (color.a > 0)
        {
            color.a += Time.fixedDeltaTime * -0.5f;
            return;
        }
        color.a = 0;

        if (!(lifeTimer >= lifeSpan)) { return; }
        Destroy(gameObject);
    }

    public void adjustObjectSizeAndRotation(float radius)
    {
        this.radius = radius;

        Vector3 tempScale = transform.localScale;
        tempScale.x = radius * 2;
        tempScale.z = radius * 2;
        transform.localScale = tempScale;

        Vector3 position = transform.position;
        position.y -= 0.5f;
        transform.position = position;
    }
}
