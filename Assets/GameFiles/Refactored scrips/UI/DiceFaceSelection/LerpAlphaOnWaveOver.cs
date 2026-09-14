using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LerpAlphaOnWaveOver : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private Color to, from;
    private void OnEnable()
    {
        DiceFaceSelectionUIManager.DiceFaceSelectionStart += HandleWaveOver;
    }

    private void OnDisable()
    {
        DiceFaceSelectionUIManager.DiceFaceSelectionStart -= HandleWaveOver;
    }

    public void HandleWaveOver()
    {
        StartCoroutine(LerpToFrom(to, from, 0.35f));
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
