using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public abstract class AbilityDescriptor : ScriptableObject
{
    public int abilityIndex;
    public int pipNumber;
    public int weight;
    public LocalizedString abilityName;
    public LocalizedString abilityDescription;
    public Sprite sprite;
    public Color color;

    public abstract PlayerBaseState Create();
}
