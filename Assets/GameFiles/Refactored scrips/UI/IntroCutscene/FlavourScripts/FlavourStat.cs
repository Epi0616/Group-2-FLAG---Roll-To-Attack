using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class FlavourStat : FlavourText
{
    [SerializeField] protected TextMeshProUGUI flavourNumber;

    protected override void GenerateRandomBS()
    {
        flavourText.text = FlavourTextDictionary.GetRandomText(FlavourTextType.Trait);
        flavourNumber.text = FlavourTextDictionary.GetRandomNumberInRange(1,20).ToString();
    }
}
