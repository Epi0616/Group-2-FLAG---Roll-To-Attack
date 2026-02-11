using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingDamageText : MonoBehaviour
{
    public Camera targetCamera;
    private float lifeTime = 3f;
    private Vector3 targetWorldUp;
    private Vector3 targetWorldPosition;
    private Quaternion targetCameraRotation;

    //set up initialize once enemy spawner is working properly
    public void Initialize(Camera camera)
    {
        //targetCamera = camera;
        targetCamera = FindFirstObjectByType<Camera>();
    }

    private void Start()
    {
        Initialize(targetCamera);
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        LookToCamera();
        FadeOut();
    }

    private void LookToCamera()
    {
        targetCameraRotation = targetCamera.transform.rotation;
        targetWorldPosition = transform.position + targetCameraRotation * Vector3.forward;
        targetWorldUp = targetCameraRotation * Vector3.up;

        transform.LookAt(targetWorldPosition, targetWorldUp);
    }

    private void FadeOut()
    {
        Vector3 tempPosition = transform.position;
        tempPosition.y += Time.fixedDeltaTime * 2f;
        transform.position = tempPosition;

        Vector3 tempScale = transform.localScale;
        tempScale.x = tempScale.y = tempScale.x * 0.99f;
        transform.localScale = tempScale;
    }
}
