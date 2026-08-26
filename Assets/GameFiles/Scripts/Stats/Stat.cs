using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    float totalMultiplier = 1;
    float totalAdditive;
    rangePair clampValues;
    bool isClamped = false;

    public Stat(float baseValue)
    {
        this.baseValue = baseValue;
        isClamped = false;
    }
    public Stat(float baseValue, rangePair clampValues)
    {
        this.baseValue = baseValue;
        this.clampValues = clampValues;
        isClamped = true;
    }

    public void ResetModifiers()
    { 
        totalAdditive = 0;
        totalMultiplier = 1;
    }

    public float GetFinalValue()
    { 
        if (isClamped) { return Mathf.Clamp(((baseValue + totalAdditive) * totalMultiplier), baseValue * clampValues.min, baseValue * clampValues.max); }
        return (baseValue + totalAdditive) * totalMultiplier;
    }
    public float GetBaseValue()
    { 
        return baseValue;
    }

    public void SetBaseValue(float value)
    { 
        baseValue = value;
    }

    public void AddAdditive(float additive)
    { 
        totalAdditive += additive;
    }

    public void SetAdditive(float additive)
    { 
        totalAdditive = additive;
    }

    public void AddMultiplier(float multiplier)
    { 
        totalMultiplier *= multiplier;
    }

    public void AddMultiplierFlat(float multiplier)
    {
        totalMultiplier += multiplier;
    }

    public void SetMultiplier(float multiplier)
    { 
        totalMultiplier = multiplier;
    }
}
