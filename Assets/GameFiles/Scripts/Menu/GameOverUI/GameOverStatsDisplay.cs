using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class GameOverStatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveCountText, numberOfAttacksText, timeSurvivedText, totalDamageDealtText, totalKillsText, totalAbilitiesEquipped;
    [SerializeField] private LocalizedString waveCountLocalizedString, numberOfAttacksLocalizedString, timeSurvivedLocalizedString, totalDamageDealtLocalizedString, totalKillsLocalizedString, totalAbilitiesEquippedLocalizedString;

    private void OnEnable()
    {
        waveCountLocalizedString.StringChanged += UpdateStatsDisplay;
    }

    private void OnDisable()
    {
        waveCountLocalizedString.StringChanged -= UpdateStatsDisplay;
    }

    private void Awake()
    {
        //UpdateStatsDisplay("null");
    }

    public void UpdateStatsDisplay(string newText)
    { 
        waveCountText.text = waveCountLocalizedString.GetLocalizedString() + ": " + RunTimeStatTracker.waveNumber;
        numberOfAttacksText.text = numberOfAttacksLocalizedString.GetLocalizedString() + ": " + RunTimeStatTracker.numberOfAttacks;
        timeSurvivedText.text = timeSurvivedLocalizedString.GetLocalizedString() + ": " + RunTimeStatTracker.totalTimeSurvived.ToString("F2");
        totalDamageDealtText.text = totalDamageDealtLocalizedString.GetLocalizedString() + ": " + RunTimeStatTracker.totalDamageDealt;
        totalKillsText.text = totalKillsLocalizedString.GetLocalizedString() + ": " + RunTimeStatTracker.totalEnemiesKilled;
        totalAbilitiesEquipped.text = totalAbilitiesEquippedLocalizedString.GetLocalizedString() + ": " + RunTimeStatTracker.totalAbilitiesEquipped;
    }
}
