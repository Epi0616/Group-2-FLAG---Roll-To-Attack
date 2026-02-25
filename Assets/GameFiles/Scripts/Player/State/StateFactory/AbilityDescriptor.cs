using UnityEngine;

public abstract class AbilityDescriptor : ScriptableObject
{
    public int pipNumber;
    public int weight;
    public string abilityName;
    public Sprite sprite;
    public abstract PlayerBaseState Create();
}
