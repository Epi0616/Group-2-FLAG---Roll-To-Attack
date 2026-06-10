using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth, currentHealth;
    [SerializeField] AudioClip[] playerHitSounds;
    [SerializeField] AudioClip[] playerHealSounds;
    public static event Action<int, int> UpdateHealthBar;
    public static event Action GameOver;
    public static event Action<float> IFrames;
    private float iFrameTimer = 0;
    private bool isDead = false;

    private void OnEnable()
    {
        DiceFaceSelectionUIManager.DiceFaceSelectionOver += HealToFull;
    }
    private void OnDisable()
    {
        DiceFaceSelectionUIManager.DiceFaceSelectionOver -= HealToFull;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar?.Invoke(currentHealth, maxHealth);
    }

    private void Update()
    {
        iFrameTimer -= Time.deltaTime;
    }

    public void OnTakeDamage(int damage)
    {
        if (iFrameTimer > 0) return;

        PlayerIFrames();
        currentHealth -= damage;
        AudioManager.instance.PlayRandomSoundClip(playerHitSounds, transform.position, 0.6f);
        UpdateHealthBar?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }
    public void HealToFull(float waveNumber)
    {
        currentHealth = maxHealth;
        AudioManager.instance.PlayRandomSoundClip(playerHealSounds, transform.position, 0.6f);
        UpdateHealthBar?.Invoke(currentHealth, maxHealth);
    }
    public void OnDeath()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Game Over");
        GameOver?.Invoke();
    }

    private void PlayerIFrames()
    {
        iFrameTimer = 1;
        IFrames?.Invoke(iFrameTimer);
    }
}
