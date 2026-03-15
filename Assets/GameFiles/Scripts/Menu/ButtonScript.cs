using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ParticleSystem pSystem;
    public void OnPointerEnter(PointerEventData eventData)
    {
        pSystem.Play();
        Debug.Log("Entered");
        transform.localScale = new Vector3(transform.localScale.x + 0.1f, transform.localScale.y + 0.1f, 1);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        transform.localScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f, 1);
    }

}
