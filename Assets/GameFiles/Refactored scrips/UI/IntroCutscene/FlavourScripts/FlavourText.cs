using TMPro;
using UnityEngine;

public class FlavourText : MonoBehaviour
{
    [SerializeField] protected FlavourTextType type;
    [SerializeField] protected TextMeshProUGUI flavourText;

    protected void OnEnable()
    {
        GenerateRandomBS();
    }

    protected void Start()
    {
        GenerateRandomBS();
    }

    protected virtual void GenerateRandomBS()
    {
        flavourText.text = FlavourTextDictionary.GetRandomText(type);
    }
}
