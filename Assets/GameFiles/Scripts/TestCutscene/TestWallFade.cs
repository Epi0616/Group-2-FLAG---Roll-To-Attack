using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class TestWallFade : MonoBehaviour
{
    [SerializeField] private Material testMat;
    [SerializeField] private Material normalMat;
    [SerializeField] private float secondsToFade;
    [SerializeField] private float timeToRise;
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
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * timeToRise, transform.position.z); //Moves wall back each frame
        if(transform.position.y > 11f)
        {
            transform.position = new Vector3(14, 11, -17); //Stops wall at y = 11
        }
        if (a < 1f)
        {
            foreach (var renderer in meshRenderers) //Increases alpha of every mat each frame
            {
                renderer.material.SetFloat("Alpha", a);

                Debug.Log(renderer.material.GetFloat("Alpha"));
            }

            a += Time.deltaTime / secondsToFade;
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
