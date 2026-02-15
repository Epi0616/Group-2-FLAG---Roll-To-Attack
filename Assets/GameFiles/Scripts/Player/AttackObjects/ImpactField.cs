using System.Runtime.CompilerServices;
using UnityEngine;

public class ImpactField : MonoBehaviour
{
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Material material;
    [SerializeField] private Color color = new(1,0,0,1);

    private void Awake()
    {
        capsuleCollider.enabled = false;
        color.a = 0;
    }
    void Update()
    {
        if (color.a > 0)
        {
            color.a += Time.deltaTime * -2f;
            return;
        }

        color.a = 0;
    }

    private void FixedUpdate()
    {
        material.color = color;
    }

    public void ShowOnPlayer(Vector3 position, float radius, Color color)
    {
        Vector3 tempScale = transform.localScale;
        tempScale.x = radius * 2;
        tempScale.z = radius * 2;
        transform.localScale = tempScale;
    
        position.y -= 0.5f;
        transform.position = position;

        this.color = color;
        color.a = 1;
    }
}
