using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ParticleSystem pSystem;

    public void OnPointerEnter(PointerEventData eventData)
    {
        pSystem.Play();
        gameObject.transform.localScale = new Vector3(1f, 1f, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        {
            gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 1);
        }
    }
}
