using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartUpSequence : MonoBehaviour
{
    [SerializeField] private Image devLogo;
    [SerializeField] private Image portLogo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartSequence());
    }

    private IEnumerator StartSequence()
    {
        Color c = devLogo.color;
        c.a = 1f;
        devLogo.color = c;
        c = portLogo.color;
        c.a = 0f;
        portLogo.color = c;

        yield return new WaitForSeconds(1f);

        yield return TransitionManager.instance.UnCover(1f);

        yield return new WaitForSeconds(2.5f);

        yield return TransitionManager.instance.Cover(1f);

        c = devLogo.color;
        c.a = 0f;
        devLogo.color = c;
        c = portLogo.color;
        c.a = 1f;
        portLogo.color = c;

        yield return new WaitForSeconds(1f);

        yield return TransitionManager.instance.UnCover(1f);

        yield return new WaitForSeconds(2.5f);

        TransitionManager.LoadScene("Menu", 3f, 1f);

    }

   
}
