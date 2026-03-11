using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject healthBar;
    float timer;

    private void OnEnable()
    {
        HealthSystem.UpdateHealthBar += UpdatePlayerHealth;
    }

    private void OnDisable()
    {
        HealthSystem.UpdateHealthBar -= UpdatePlayerHealth;
    }

    private void Awake()
    {
        text.alpha = 0f;
    }

    private void UpdatePlayerHealth(int currentHealth, int maxHealth)
    {
        text.text = currentHealth.ToString();
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(1000 * ((float)currentHealth / maxHealth), healthBar.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < 1)
        {
            text.alpha += timer * 1f;
        }
    }
}

