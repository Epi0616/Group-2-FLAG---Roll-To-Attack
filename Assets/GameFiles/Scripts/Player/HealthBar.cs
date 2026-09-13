using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image healthBar, healthBarRed;

    private Coroutine redHealthBarCatchUp;

    private void OnEnable()
    {
        PlayerHealthSystem.UpdateHealthBar += UpdatePlayerHealth;
    }

    private void OnDisable()
    {
        PlayerHealthSystem.UpdateHealthBar -= UpdatePlayerHealth;
    }

    private void Awake()
    {
        AdjustTextAlpha(1, 0, 1);
    }

    private void UpdatePlayerHealth(int currentHealth, int maxHealth)
    {
        text.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;

        if (redHealthBarCatchUp != null)
        { 
            StopCoroutine(redHealthBarCatchUp);
        }
        redHealthBarCatchUp = StartCoroutine(AdjustRedHealthBarFillAmount(healthBar.fillAmount, healthBarRed.fillAmount, 0.65f));
    }

    private IEnumerator AdjustRedHealthBarFillAmount(float to, float from, float duration)
    {
        float timer = 0;
        float t = 0;

        while (t < 1)
        {
            timer += Time.deltaTime;
            t = timer / duration;

            healthBarRed.fillAmount = Mathf.Lerp(from, to, t);
            yield return null;
        }

        healthBarRed.fillAmount = to;
    }

    private IEnumerator AdjustTextAlpha(float to, float from, float duration)
    {
        float timer = 0;
        float t = 0;

        while (t < 1)
        {
            timer += Time.deltaTime;
            t = timer / duration;

            text.alpha = Mathf.Lerp(from, to, t);
            yield return null;  
        }

        text.alpha = to;
    }
}

