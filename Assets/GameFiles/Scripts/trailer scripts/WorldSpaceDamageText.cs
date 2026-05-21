using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading;

public class WorldSpaceDamageText : MonoBehaviour
{
    [SerializeField] private Renderer myRenderer;
    [SerializeField] private TextMeshPro tmp;
    private Camera targetCamera;
    [SerializeField] private float lifeTime = 500f;
    [SerializeField] private Vector3 originalScale;
    [SerializeField] private Vector3 targetScale;
    //private Vector3 targetWorldUp;
    //private Vector3 targetWorldPosition;
    //private Quaternion targetCameraRotation;

    //set up initialize once enemy spawner is working properly
    public void Initialize(Camera camera)
    {
        targetCamera = camera;
        transform.localScale = originalScale;
        StartCoroutine(DestroyRoutine());
        StartCoroutine(ScaleUpRoutine());   
    }

    private void Awake()
    {
        originalScale = transform.localScale;
        myRenderer.material.renderQueue = 100;
        myRenderer.enabled = false;
        myRenderer.enabled = true;
    }

    private void Update()
    {      
        if (targetCamera == null) return;
        //tmp.ForceMeshUpdate(true, true);
        //Debug.Log(tmp.mesh.bounds);
        transform.rotation = targetCamera.transform.rotation;
        transform.position += Vector3.up * Time.deltaTime * 2f;
        transform.localScale *= 0.9999f;

        //Vector3 directionToCamera = (targetCamera.transform.position - transform.position).normalized;
        //transform.position += directionToCamera * Time.deltaTime * 5f;

        //Vector3 earlyCameraView = targetCamera.transform.position + new Vector3 (0, -7.5f, 10);

        //transform.position = Vector3.Lerp(transform.position, earlyCameraView, Time.deltaTime * 1f);
    }

    private IEnumerator ScaleUpRoutine()
    {
        float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        { 
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 10f);
            yield return null;
        }
        transform.localScale = targetScale;
    }

    private IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(lifeTime);

        float timer = 1;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            tmp.alpha = timer;
            yield return null;
        }

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
