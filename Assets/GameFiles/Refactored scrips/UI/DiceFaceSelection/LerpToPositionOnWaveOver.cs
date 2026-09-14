using System.Collections;
using UnityEngine;

public class LerpToPositionOnWaveOver : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    [SerializeField] Vector2 from, to;

    private void OnEnable()
    {
        DiceFaceSelectionUIManager.DiceFaceSelectionStart += HandleWaveOver;
        AbilityPanel.AbilitySelected += HandleAbilityPicked;
        HealthOption.HealthChosen += HandleHealthChosen;
    }

    private void OnDisable()
    {
        DiceFaceSelectionUIManager.DiceFaceSelectionStart -= HandleWaveOver;
        AbilityPanel.AbilitySelected -= HandleAbilityPicked;
        HealthOption.HealthChosen -= HandleHealthChosen;
    }

    public void HandleAbilityPicked(AbilityPanel panel)
    {
        StartCoroutine(LerpToFrom(from, to, 0.25f));
    }

    public void HandleHealthChosen(int healAmount)
    {
        StartCoroutine(LerpToFrom(from, to, 0.25f));
    }

    public void HandleWaveOver()
    {
        StartCoroutine(LerpToFrom(to, from, 0.35f));
    }

    private IEnumerator LerpToFrom(Vector2 to, Vector2 from, float duration)
    {
        float timer = 0;
        float t = 0;

        while (t < 1)
        { 
            timer += Time.deltaTime;
            t = timer/ duration;
            rectTransform.anchoredPosition = Vector2.Lerp(from, to, t);
            yield return null;
        }

        rectTransform.anchoredPosition = to;
    }
}
