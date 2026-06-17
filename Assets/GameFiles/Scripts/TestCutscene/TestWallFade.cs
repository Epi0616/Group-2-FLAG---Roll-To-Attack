using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class TestWallFade : MonoBehaviour
{
    [SerializeField] private Material testMat;
    [SerializeField] private Material normalMat;
    [SerializeField] private float seconds;
    [SerializeField] private ParticleSystem ps;
    private MeshRenderer[] meshRenderers;
    private float a;
    private bool checkMat;

    private void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>(); //Gets all mesh renderers of child objs
        foreach (var renderer in meshRenderers)
        {
            renderer.material = testMat; //Sets all materials to test mat
        }

        a = 0;
        checkMat = false;
    }

    private void Update()
    {
        if (a < 1f)
        {
            foreach (var renderer in meshRenderers) //Increases alpha of every mat each frame
            {
                renderer.material.SetFloat("Alpha", a);

                Debug.Log(renderer.material.GetFloat("Alpha"));
            }

            a += Time.deltaTime / seconds;
        }
        else if (!checkMat)
        {
            foreach (var renderer in meshRenderers) //Sets all mats to normal mat
            {
                renderer.material = normalMat;

                checkMat = true;
            }
        }
    }
}
