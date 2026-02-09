using System.Collections.Generic;
using UnityEngine;


public class ImpactField : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material material;
    [SerializeField] private Color color = new Color(0,1,0,1);

    private void Awake()
    {
        color.a = 0;
    }
    void Update()
    {
        if (color.a > 0)
        {
            color.a += Time.deltaTime * -1f;
            return;
        }

        color.a = 0;
    }

    private void FixedUpdate()
    {
        material.color = color;
    }

    public void ShowOnPlayer(Vector3 position)
    {
        position.y = -0.5f;
        transform.position = position;

        color.a = 1;

        DealDamage(50);
    }

    private void DealDamage(int damage)
    {
        Collider[] touching = Physics.OverlapSphere(transform.position, 2.5f);

        Debug.Log(touching.Length);

        for (int i = 0; i < touching.Length; i++)
        { 
            Collider currentCollider = touching[i];
            if (currentCollider.gameObject.CompareTag("Enemy"))
            {
                currentCollider.gameObject.GetComponent<EnemyBaseClass>().OnTakeDamage(damage);
            }
        }


    }
}
