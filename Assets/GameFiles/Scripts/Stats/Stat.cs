using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    float totalMultiplier = 1;
    float totalAdditive;

    public void ResetModifiers()
    { 
        totalAdditive = 0;
        totalMultiplier = 1;
    }

    public float GetFinalValue()
    { 
        return (baseValue + totalAdditive) * totalMultiplier;
    }
    public float GetBaseValue()
    { 
        return baseValue;
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
