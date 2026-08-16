using UnityEngine;
using System;

public class HealthOption : MonoBehaviour
{
    public static event Action<int> HealthChosen;

    [SerializeField] private int healAmount;
    public void HealAmount()
    { 
        HealthChosen?.Invoke(healAmount);
    }
}
