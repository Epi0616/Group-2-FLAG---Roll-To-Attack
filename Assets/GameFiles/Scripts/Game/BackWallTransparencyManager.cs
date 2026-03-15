using UnityEngine;
using System.Collections.Generic;

public class BackWallTransparencyManager: MonoBehaviour
{
    [SerializeField] private List<GameObject> backWallObjects;
    [SerializeField] private GameObject playerReference;
    private Vector3 playerPosition;
    private bool isTransparent = false;

    private void Update()
    {
        playerPosition = playerReference.transform.position;

        if (playerPosition.z > -30 && isTransparent)
        {
            UpdateTransparency(isTransparent);
            isTransparent = false;

        }

        if (playerPosition.z <= -30 && !isTransparent)
        {
            UpdateTransparency(isTransparent);
            isTransparent = true;
            Debug.Log("Player is behind the back wall, making it transparent.");
        }
    }

    private void UpdateTransparency(bool enabled)
    {
        foreach (var obj in backWallObjects)
        {
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            renderer.enabled = enabled;
        }
    }
}
