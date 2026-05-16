using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CinematicDisplayScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private RectTransform rectTransform;
    private Camera targetCamera;
    [SerializeField] private float lifeTime = 5;
    [SerializeField] private Vector3 scale = new (1,1,1);
    [SerializeField] private Vector3 targetScale = new(20, 20, 20);
    private Canvas canvas;

    public void Initialize(Camera camera, Canvas screen)
    {
        targetCamera = camera;
        transform.SetParent(screen.transform);
        StartCoroutine(DestroyRoutine());
        canvas = screen;

        transform.localScale = scale;
        Vector2 screenPosition = WorldToCanvasPosition(transform.position);

        //Vector2 screenPosition = targetCamera.WorldToScreenPoint(transform.position);
        rectTransform.anchoredPosition = screenPosition;
    }

    private void Update()
    {
        if (targetCamera == null) return;

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 2 * Time.deltaTime);
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, Vector2.zero, 2 * Time.deltaTime);
    }

    private Vector2 WorldToCanvasPosition(Vector3 worldPosition)
    {
        Vector2 screenPosition = targetCamera.WorldToScreenPoint(worldPosition);

        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, null, out Vector2 canvasPosition);

        return canvasPosition;
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
