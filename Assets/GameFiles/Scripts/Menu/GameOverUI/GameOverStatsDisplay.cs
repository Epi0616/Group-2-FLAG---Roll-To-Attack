using TMPro;
using UnityEngine;

public class GameOverStatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveCountText, numberOfAttacksText, timeSurvivedText, totalDamageDealtText, totalKillsText, totalAbilitiesEquipped;

    public void UpdateStatsDisplay()
    { 
        waveCountText.text = $"Wave Reached: {RunTimeStatTracker.waveNumber}";
        numberOfAttacksText.text = $"Number of Attacks: {RunTimeStatTracker.numberOfAttacks}";
        timeSurvivedText.text = $"Time Survived: {RunTimeStatTracker.totalTimeSurvived.ToString("F2")}s";
        totalDamageDealtText.text = $"Total Damage Dealt: {RunTimeStatTracker.totalDamageDealt}";
        totalKillsText.text = $"Total Kills: {RunTimeStatTracker.totalEnemiesKilled}";
        totalAbilitiesEquipped.text = $"Total Abilities Equipped: {RunTimeStatTracker.totalAbilitiesEquipped}";
    }
}
