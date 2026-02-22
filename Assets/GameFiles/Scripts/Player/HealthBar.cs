using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI Text;
    float timer;

    private void OnEnable()
    {
        PlayerStateController.UpdateHealthBar += UpdatePlayerHealth;
    }

    private void OnDisable()
    {
        PlayerStateController.UpdateHealthBar -= UpdatePlayerHealth;
    }

    private void Awake()
    {
        Text.alpha = 0f;
    }

    private void UpdatePlayerHealth(int currentHealth)
    {
        Text.text = "HP: " + currentHealth;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < 1)
        {
            Text.alpha += timer * 1f;
        }
    }
}

