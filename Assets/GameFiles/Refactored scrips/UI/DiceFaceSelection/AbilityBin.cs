using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBin : AbilityDropZoneParent
{
    [SerializeField] private Image binBackground;
    [SerializeField] private Color normal, highlighted;

    private Coroutine fadeRoutine;
    
    public override bool TryAddChild(DraggableObject newObject)
    {
        Destroy(newObject.gameObject);
        return true;
    }

    public override void OnHighlighted()
    {
        if (fadeRoutine != null)
        { 
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = StartCoroutine(FadeColor(highlighted, 0.05f));
    }

    public override void OnUnhighlighted()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = StartCoroutine(FadeColor(normal, 0.05f));
    }

    private IEnumerator FadeColor(Color to, float duration)
    {
        Color from = binBackground.color;

        float timer = 0;
        float t = 0;

        while (t < 1)
        {
            timer += Time.deltaTime;
            t = timer / duration;
            Color newColor = Color.Lerp(from, to, t);
            AdjustColor(newColor);
            yield return null;
        }

        AdjustColor(to);
    }

    private void AdjustColor(Color to)
    {
        binBackground.color = to;
    }
}
