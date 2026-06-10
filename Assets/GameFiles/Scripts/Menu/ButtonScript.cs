using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Quaternion originalRotation;
    //public ParticleSystem pSystem;
    public float sizeWidth;
    public float sizeHeight;
    public float sizeWidthHover;
    public float sizeHeightHover;

    void Start()
    {
        originalRotation = Quaternion.Euler(1, 1, 1);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        //pSystem.Play();
        gameObject.transform.localScale = new Vector3(sizeWidthHover, sizeHeightHover, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(sizeWidth, sizeHeight, 1);
    }

}
