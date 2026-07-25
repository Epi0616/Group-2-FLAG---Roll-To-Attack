using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDisplayUI : MonoBehaviour
{
    [SerializeField] private Image uiImage;

    public void StartDisplay(Sprite sprite)
    {
        StartCoroutine(DisplaySprite(sprite));
    }

    public IEnumerator DisplaySprite(Sprite sprite)
    {
        Debug.Log("Displaying");
        yield return StartCoroutine(SmoothFadeOut());
        uiImage.color = new Color(255, 255, 255, 0);
        uiImage.sprite = sprite;
        StartCoroutine(SmoothFadeIn());
    }

    public IEnumerator SmoothFadeIn()
    {
        Debug.Log("Fade In");
        float timer = 0;
        while (timer < 0.1f)
        {
            timer += Time.deltaTime;
            uiImage.color = new Color(255, 255, 255, Mathf.Lerp(0f, 1f, (timer / 0.5f)));
            yield return null;
        }
        uiImage.color = new Color(255, 255, 255, 1);
    }

    public IEnumerator SmoothFadeOut()
    {       
        Debug.Log("Fade Out");
        float timer = 0;
        while (timer < 0.1f && !(uiImage.color.a == 0))
        {
            timer += Time.deltaTime;
            uiImage.color = new Color(255, 255, 255, Mathf.Lerp(1f, 0f, (timer / 0.25f)));
            yield return null;
        }
        uiImage.color = new Color(255, 255, 255, 0);
    }
}   
