using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LerpAlphaTabletPhase : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private Color to, from;

    private void OnEnable()
    {
        ContinueButton.Continue += HandleContinuePressed;
    }

    private void OnDisable()
    {
        ContinueButton.Continue -= HandleContinuePressed;
    }

    private void HandleContinuePressed() 
    {
        StartCoroutine(LerpToFrom(from, to, 0.25f));
    }

    private IEnumerator LerpToFrom(Color to, Color from, float duration)
    {
        float timer = 0;
        float t = 0;

        while (t < 1)
        {
            timer += Time.deltaTime;
            t = timer / duration;
            image.color = Color.Lerp(from, to, t);
            yield return null;
        }

        image.color = to;
    }
}
