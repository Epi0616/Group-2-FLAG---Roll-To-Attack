using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BackWallTransparencyManager: MonoBehaviour
{
    [SerializeField] private List<GameObject> backWallObjects;
    [SerializeField] private GameObject playerReference;
    [SerializeField] private Material transparentMaterial;
    [SerializeField] private Material backWallMaterial;
    private Vector3 playerPosition;
    private bool isTransparent = true;

    //"update loop"
    //{
    //    playerPosition = playerReference.transform.position;

    //    if (playerPosition.z > -13 && isTransparent)
    //    {
    //        UpdateTransparency();
    //        isTransparent = false;
    //    }

    //    if (playerPosition.z <= -13 && !isTransparent)
    //    {
    //        UpdateTransparency();
    //        isTransparent = true;
    //        //Debug.Log("Player is behind the back wall, making it transparent.");
    //    }
    //}

    //private void UpdateTransparency()
    //{
    //    foreach (var obj in backWallObjects)
    //    {
    //        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();

    //        if (isTransparent)
    //        {
    //            StartCoroutine(ShowRoutine(transparentMaterial, renderer));
    //            continue;
    //        }

    //        StartCoroutine(FadeRoutine(transparentMaterial, renderer));
    //    }
    //}

    //private IEnumerator FadeRoutine(Material transparentMaterial, MeshRenderer renderer)
    //{

    //    while (transparentMaterial.color.a > 0.2)
    //    {
    //        var tempColor = transparentMaterial.color;
    //        tempColor.a -= 0.01f;
    //        transparentMaterial.color = tempColor;
    //        renderer.material = transparentMaterial;

    //        yield return new WaitForSeconds(0.07f);
    //    }
    //}

    //private IEnumerator ShowRoutine(Material transparentMaterial, MeshRenderer renderer)
    //{

    //    while (transparentMaterial.color.a < 0.7)
    //    {
    //        var tempColor = transparentMaterial.color;
    //        tempColor.a += 0.01f;
    //        transparentMaterial.color = tempColor;
    //        renderer.material = transparentMaterial;

    //        yield return new WaitForSeconds(0.07f);
    //    }
    //}
}
