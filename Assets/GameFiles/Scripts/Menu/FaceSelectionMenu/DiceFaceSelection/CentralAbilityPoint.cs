using UnityEngine;

public class CentralAbilityPoint : MonoBehaviour
{
    public static GameObject instance;
    public static RectTransform rectTransform;

    private void Awake()
    {
        if (instance == null)
        {
            instance = gameObject;
            rectTransform = GetComponent<RectTransform>();

            return;
        }

        Destroy(gameObject);
    }
}
