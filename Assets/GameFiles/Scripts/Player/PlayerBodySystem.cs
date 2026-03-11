using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerBodySystem : MonoBehaviour
{
    public GameObject body;
    public Quaternion originalRotation;
    private float iFrameTimer;

    private void OnEnable()
    {
        HealthSystem.IFrames += DisplayIFrames;
    }
    private void OnDisable()
    {
        HealthSystem.IFrames -= DisplayIFrames;
    }

    void Start()
    {
        originalRotation = body.transform.rotation;
    }

    void Update()
    {
        iFrameTimer -= Time.deltaTime;
    }

    public void ShakeDiceBody(float magnitude)
    {
        float x = Mathf.Sin(Time.time * 50f) * magnitude;
        float y = Mathf.Sin(Time.time * 50f) * magnitude;
        float z = Mathf.Sin(Time.time * 50f) * magnitude;
        body.transform.rotation = originalRotation * Quaternion.Euler(x, y, z);
    }

    private void DisplayIFrames(float timer)
    {
        iFrameTimer = timer;
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        Debug.Log("starting corotuine");
        bool toggle = false;
        while (iFrameTimer > 0)
        {
            toggle = !toggle;
            body.SetActive(toggle);

            float t = 1 - Mathf.Clamp01(iFrameTimer / 2f);
            float interval = Mathf.Lerp(0.25f, 0.05f, t);

            yield return new WaitForSeconds(interval);
        }
        body.SetActive(true);
    }
}
