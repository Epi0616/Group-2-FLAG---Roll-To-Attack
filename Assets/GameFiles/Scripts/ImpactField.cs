using System.Runtime.CompilerServices;
using UnityEngine;

public class ImpactField : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Material material;
    [SerializeField] private Color color = new Color(0,1,0,1);

    private void Awake()
    {
        capsuleCollider.enabled = false;
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

    private void OnBecameInvisible()
    {
        capsuleCollider.enabled = false;
    }

    private void OnBecameVisible()
    {
        capsuleCollider.enabled = true;
    }

    public void ShowOnPlayer(Vector3 position)
    {
        position.y = -0.5f;
        transform.position = position;

        color.a = 1;
    }

}
