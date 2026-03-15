using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler
{
    public ParticleSystem pSystem;

    public void OnPointerEnter(PointerEventData eventData)
    {
        pSystem.Play();
    }
}
