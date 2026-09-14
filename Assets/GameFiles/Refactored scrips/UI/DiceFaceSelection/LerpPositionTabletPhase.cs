using UnityEngine;
using System.Collections;

public class LerpPositionTabletPhase : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    [SerializeField] Vector2 from, to;

    private void OnEnable()
    {
        HandleEnabled();
        ContinueButton.Continue += HandleContinue;
    }

    private void OnDisable()
    {
        ContinueButton.Continue -= HandleContinue;
    }

    private void HandleContinue()
    {
        StartCoroutine(LerpToFrom(from, to, 0.25f));
    }

    private void HandleEnabled()
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
            t = timer / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(from, to, t);
            yield return null;
        }

        rectTransform.anchoredPosition = to;
    }
}
