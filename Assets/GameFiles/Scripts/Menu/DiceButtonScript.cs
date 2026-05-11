using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiceButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Quaternion originalRotation;
    public GameObject dice;
    //public ParticleSystem pSystem;
    public float sizeWidth;
    public float sizeHeight;
    public float sizeWidthHover;
    public float sizeHeightHover;

    void Start()
    {
        originalRotation = Quaternion.Euler(1, 1, 1);
        dice.SetActive(false);
    }

    void Update()
    {
        ShakeDiceBody(20);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //pSystem.Play();
        gameObject.transform.localScale = new Vector3(sizeWidthHover, sizeHeightHover, 1);
        dice.SetActive(true);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(sizeWidth, sizeHeight, 1);
        dice.SetActive(false);
    }


    public void ShakeDiceBody(float magnitude)
    {
        float x = Time.time * 50f + magnitude;
        float y = Time.time * 50f + magnitude;
        float z = Time.time * 50f + magnitude;
        dice.transform.rotation = originalRotation * Quaternion.Euler(x, y, z);
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(sizeWidthHover, sizeHeightHover, 1);
        dice.SetActive(true);
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(sizeWidth, sizeHeight, 1);
        dice.SetActive(false);
    }
}
