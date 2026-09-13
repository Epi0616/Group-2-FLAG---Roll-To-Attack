using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessing : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private Vignette vignette;

    private Coroutine vignetteRoutine;

    private void OnEnable()
    {
        PostProcessingSetting.togglePostProcessing += TogglePostProcessing;
        VideoSettingUI.togglePostProcessing += TogglePostProcessing;

        PlayerHealthSystem.DamageConfirmed += ActivateVignette;
    }

    private void OnDisable()
    {
        PostProcessingSetting.togglePostProcessing -= TogglePostProcessing;
        VideoSettingUI.togglePostProcessing -= TogglePostProcessing;

        PlayerHealthSystem.DamageConfirmed -= ActivateVignette;
    }

    private void Start()
    {
        if (volume.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true;
        }
    }

    private void TogglePostProcessing(bool isActive)
    { 
        volume.weight = isActive ? 1 : 0;
    }

    private void ActivateVignette(float timer) //float value intended for iframe timer
    {
        if (vignette == null) return;

        if (vignetteRoutine != null)
        { 
            StopCoroutine(vignetteRoutine);
        }

        vignetteRoutine = StartCoroutine(FadeToAndFromVignette(0.5f, 0, 0.05f));
    }

    private IEnumerator FadeToAndFromVignette(float to, float from, float duration)
    {
        float initialFrom = vignette.intensity.value;

        float timer = 0;
        float t = 0;

        while (t < 1)
        { 
            timer += Time.deltaTime;
            t = timer / duration;
            vignette.intensity.value = Mathf.Lerp(initialFrom, to, t);
            yield return null;
        }

        vignette.intensity.value = to;


        timer = 0;
        t = 0;

        while (t < 1)
        {
            timer += Time.deltaTime;
            t = timer / (duration * 20);
            vignette.intensity.value = Mathf.Lerp(to, from, t);
            yield return null;
        }

        vignette.intensity.value = from;
    }
}
