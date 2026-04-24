using System.Collections;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private float refreshRate = 1f;
    private bool isVisible = false;
    private float timer = 1;
    [SerializeField] GameObject fpsToggleCheckMark;

    private void OnEnable()
    {
        VideoSettingUI.toggleFPSVisibility += SetTextVisibility;
    }

    private void OnDisable()
    {
        VideoSettingUI.toggleFPSVisibility -= SetTextVisibility;
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            fpsText.text = Mathf.RoundToInt(1 / Time.unscaledDeltaTime) + " FPS";
            timer = refreshRate;
        }
    }

    private void SetTextVisibility()
    {
        isVisible = !isVisible;
        fpsToggleCheckMark.SetActive(isVisible);
        if (isVisible)
        {
            fpsText.alpha = 1;
            return;
        }

        fpsText.alpha = 0;
    }
}