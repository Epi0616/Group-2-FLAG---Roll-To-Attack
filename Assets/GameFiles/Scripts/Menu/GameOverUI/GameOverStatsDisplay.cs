using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class GameOverStatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveCountText, numberOfAttacksText, timeSurvivedText, totalDamageDealtText, totalKillsText;
    [SerializeField] private LocalizedString waveCountLocalizedString, numberOfAttacksLocalizedString, timeSurvivedLocalizedString, totalDamageDealtLocalizedString, totalKillsLocalizedString;

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
        int minutes = Mathf.FloorToInt(RunTimeStatTracker.instance.runTimeStats.totalTimeSurvived / 60f);
        int seconds = Mathf.FloorToInt(RunTimeStatTracker.instance.runTimeStats.totalTimeSurvived % 60f);

        waveCountText.text = waveCountLocalizedString.GetLocalizedString() + ": " + RunTimeStatTracker.instance.runTimeStats.waveNumber;
        numberOfAttacksText.text = numberOfAttacksLocalizedString.GetLocalizedString() + ": " + RunTimeStatTracker.instance.runTimeStats.numberOfAttacks;
        timeSurvivedText.text = timeSurvivedLocalizedString.GetLocalizedString() + ": " + minutes + "m " + seconds + "s";
        totalDamageDealtText.text = totalDamageDealtLocalizedString.GetLocalizedString() + ": " + RunTimeStatTracker.instance.runTimeStats.totalDamageDealt;
        totalKillsText.text = totalKillsLocalizedString.GetLocalizedString() + ": " + RunTimeStatTracker.instance.runTimeStats.totalEnemiesKilled;
    }
}
