using TMPro;
using UnityEngine;

public class ContinueInfo : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI hp;
    [SerializeField] protected TextMeshProUGUI waveNumber;
    [SerializeField] protected TextMeshProUGUI timePlayed;

    public void SetInfo(int hp, int waveNumber, float timePlayed)
    { 
        this.hp.text = hp.ToString() + " HP";
        this.waveNumber.text = "Wave " + waveNumber.ToString();
        this.timePlayed .text = "Time Played: " + timePlayed.ToString("F2") + "s";
    }
}
