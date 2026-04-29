using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private Image transitionCover;

    bool isTransitioning = false;

    public static TransitionManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);

        Color c = transitionCover.color;
        c.a = 1;
        transitionCover.color = c;
    }

    public static void LoadScene(string sceneName, float UnCoverTime, float CoverTime)
    {
        if (instance.isTransitioning) { return; }

        instance.StartCoroutine(instance.Transition(sceneName, UnCoverTime, CoverTime));
    }

    public IEnumerator Transition(string sceneName, float UnCoverTime, float CoverTime)
    {
        isTransitioning = true;
        Debug.Log("Fading to Black");
        yield return StartCoroutine(Cover(CoverTime));
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Changing Scene");
        SceneManager.LoadScene(sceneName);
        yield return null;
        Debug.Log("Fading from Black");
        yield return StartCoroutine(UnCover(UnCoverTime));
        isTransitioning = false;
    }

    public IEnumerator UnCover(float duration)
    {
        float t = 0f;
        Color c = transitionCover.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = 1 - (t / duration);
            transitionCover.color = c;
            yield return null;
        }
        c.a = 0;
        transitionCover.color = c;
    }

    public IEnumerator Cover(float duration)
    {
        float t = 0f;
        Color c = transitionCover.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = t / duration;
            transitionCover.color = c;
            yield return null;
        }
        c.a = 1;
        transitionCover.color = c;
    }
}
