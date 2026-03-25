using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image healthBar;
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
        text.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        //healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(1000 * ((float)currentHealth / maxHealth), healthBar.GetComponent<RectTransform>().sizeDelta.y);
        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
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

