using System.Collections.Generic;
using UnityEngine;


// this game object this script is attatched to is being preloaded in project settings, i will refine this with a dedicated object pooling when i have the time
public class PoisionImpactField : MonoBehaviour
{
    private Material material;
    private Color color = new(0, 1, 0, 1);
    private float lifeSpan = 10, lifeTimer = 0;
    private float damageTickTimer = 0, currentTickCount = 0;


    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        color.a = 0.5f;
        material.color = color;
    }

    private void Start()
    {
        DealDamage();
    }

    private void FixedUpdate()
    {
        TickDamage();
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

    private void TickDamage()
    {
        damageTickTimer += Time.fixedDeltaTime;
        if (!(damageTickTimer >= 1)) { return; }
        DealDamage();
        damageTickTimer = 0;
    }

    private void DealDamage()
    {
        if (!(currentTickCount < 10)) { return; }
        currentTickCount++;

        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.x / 2); // scale x/2 as scale is diamater not radius

        foreach (var collider in colliders)
        {
            if (!collider.gameObject) { continue; }

            if (collider.gameObject.CompareTag("Enemy"))
            {
                collider.gameObject.GetComponent<EnemyBaseClass>().OnTakeDamage(3);
                Debug.Log("dealing damage");
            }
        }
    }

    public void adjustObjectSizeAndRotation(float scale)
    {
        Vector3 tempScale = transform.localScale;
        tempScale.x = scale;
        tempScale.z = scale;
        transform.localScale = tempScale;

        Vector3 position = transform.position;
        position.y -= 0.5f;
        transform.position = position;
    }
}
