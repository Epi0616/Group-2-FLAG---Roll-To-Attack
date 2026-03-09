using UnityEngine;
using UnityEngine.UI;

public abstract class AbilityDescriptor : ScriptableObject
{
    public int pipNumber;
    public int weight;
    public string abilityName;
    public string abilityDescription;
    public Image sprite;
    public Color color;

    public abstract PlayerBaseState Create();
}
