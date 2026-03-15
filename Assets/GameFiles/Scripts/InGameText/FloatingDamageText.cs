using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField] private Renderer myRenderer;
    private Camera targetCamera;
    private float lifeTime = 3f;
    private Vector3 originalScale;
    //private Vector3 targetWorldUp;
    //private Vector3 targetWorldPosition;
    //private Quaternion targetCameraRotation;

    //set up initialize once enemy spawner is working properly
    public void Initialize(Camera camera)
    {
        targetCamera = camera;
        transform.localScale = originalScale;
        StartCoroutine(DestroyRoutine());
    }

    private void Awake()
    {
        originalScale = transform.localScale;
        myRenderer.material.renderQueue = 100;
    }

    private void Update()
    {
        if (targetCamera == null) return;

        transform.rotation = targetCamera.transform.rotation;
        transform.position += Vector3.up * Time.deltaTime * 3f;
        transform.localScale *= 0.995f;
    }

    private IEnumerator DestroyRoutine()
    { 
        yield return new WaitForSeconds(lifeTime);

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    //private void LookToCamera()
    //{
    //    if (targetCamera == null) return;

    //    targetCameraRotation = targetCamera.transform.rotation;
    //    targetWorldPosition = transform.position + targetCameraRotation * Vector3.forward;
    //    targetWorldUp = targetCameraRotation * Vector3.up;

    //    transform.LookAt(targetWorldPosition, targetWorldUp);
    //}

    //private void FadeOut()
    //{
    //    Vector3 tempPosition = transform.position;
    //    tempPosition.y += Time.deltaTime * 2f;
    //    transform.position = tempPosition;

    //    Vector3 tempScale = transform.localScale;
    //    tempScale.x = tempScale.y = tempScale.x * 0.999f;
    //    transform.localScale = tempScale;
    //}
}
