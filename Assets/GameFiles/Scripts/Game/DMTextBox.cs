using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DMTextBox : MonoBehaviour
{
    [SerializeField] private TypedLettersTMP TypedLetters;
    [SerializeField] private GameObject PopUpObj;
    [SerializeField] private List<Image> PortraitImages;
    [SerializeField] private Image TextBG;
    [SerializeField] private TextMeshProUGUI tmp;
    public static DMTextBox Instance;
    //private bool isVisible;
    private float currentBoxAlpha = 0;
    private float currentPortraitAlpha = 0;
    private Coroutine RevealRoutine;
    private Coroutine BoxRevealRoutine;
    private Coroutine ScaleRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //isVisible = false;
        //foreach (Image image in PortraitImages)
        //{
        //    image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        //}
        //TextBG.color = new Color(TextBG.color.r, TextBG.color.g, TextBG.color.b, 0);
        //tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0);
        TypedLetters.SetTextOverride("");
    }
    
    private IEnumerator AdjustOpacityRoutine(float to, float from, float duration)
    {
        float timer = 0;
        float t = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            t = timer / duration;

            float alpha = Mathf.Lerp(from, to, t);
            currentPortraitAlpha = alpha;
            foreach (Image image in PortraitImages)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            }
            yield return null;
        }

    }

    private IEnumerator AdjustBoxOpacityRoutine(float to, float from, float duration)
    {
        float timer = 0;
        float t = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            t = timer / duration;

            float alpha = Mathf.Lerp(from, to, t);
            currentBoxAlpha = alpha;
            
            TextBG.color = new Color(TextBG.color.r, TextBG.color.g, TextBG.color.b, alpha);         
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alpha);
            yield return null;
        }
    }

    private IEnumerator AdjustScaleRoutine(Vector3 to, Vector3 from, float duration)
    {
        Debug.Log("StartingScale: " + PopUpObj.transform.localScale);
        float timer = 0;
        float t = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            t = timer / duration;

            Vector3 scale = Vector3.Lerp(from, to, easeOutBounce(t));
            PopUpObj.transform.localScale = scale;
            yield return null;
        }
        //timer = 0;
        //while (timer < duration * 0.1f)
        //{
        //    timer += Time.deltaTime;
        //    t = timer / duration;

        //    Vector3 scale = Vector3.Lerp(PopUpObj.transform.localScale, to, t);
        //    PopUpObj.transform.localScale = scale;
        //    yield return null;
        //}
        PopUpObj.transform.localScale = to;
        Debug.Log("EndingScale: " + PopUpObj.transform.localScale);
    }

    private IEnumerator DisplayNewText(string text, float duration)
    {
        //if (RevealRoutine != null) { StopCoroutine(RevealRoutine); }
        //if (BoxRevealRoutine != null) { StopCoroutine(BoxRevealRoutine); }
        //yield return RevealRoutine = StartCoroutine(AdjustOpacityRoutine(0.8f, currentPortraitAlpha, 0.75f));
        
        TypedLetters.SetText(text);
        yield return ScaleRoutine = StartCoroutine(AdjustScaleRoutine(Vector3.one, PopUpObj.transform.localScale, 0.7f));
        //BoxRevealRoutine = StartCoroutine(AdjustBoxOpacityRoutine(1, currentBoxAlpha, 0.75f));
        yield return new WaitForSecondsRealtime(duration + 0.5f);
        StartCoroutine(RemoveText());
    }

    public void DisplayText(string text, float visibleDuration)
    {
        StartCoroutine(DisplayNewText(text, visibleDuration));
    }

    public IEnumerator RemoveText()
    {
        if (RevealRoutine != null) { StopCoroutine(RevealRoutine); }
        if (BoxRevealRoutine != null) { StopCoroutine(BoxRevealRoutine); }
        if (ScaleRoutine != null) { StopCoroutine(ScaleRoutine); }
        //BoxRevealRoutine = StartCoroutine(AdjustBoxOpacityRoutine(0, currentBoxAlpha, 0.5f));
        //yield return RevealRoutine = StartCoroutine(AdjustOpacityRoutine(0, currentPortraitAlpha, 0.5f));
        yield return ScaleRoutine = StartCoroutine(AdjustScaleRoutine(Vector3.zero, PopUpObj.transform.localScale, 0.75f));
        TypedLetters.SetTextOverride("");
    }

    protected float easeOutBack(float x)
    {
        const float c1 = 2.70158f;
        const float c3 = c1 + 1f;
        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }
    public float EaseInOutBack(float t)
    {
        const float c1 = 2.70158f;
        const float c2 = c1 * 1.525f;
        float t2 = t - 1f;
        return t < 0.5
            ? t * t * 2 * ((c2 + 1) * t * 2 - c2)
            : t2 * t2 * 2 * ((c2 + 1) * t2 * 2 + c2) + 1;
    }
    public float easeOutBounce(float x)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;
        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.8f;
        }
        //else if (x < 2.5 / d1)
        //{
        //    return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        //}
        else
        {
            return n1 * (x -= 1.5f / d1) * x + 0.984375f;
        }

    }
}
