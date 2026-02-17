using TMPro;
using UnityEngine;

public class PlayerInterfaceEnemiesRemaining : MonoBehaviour
{
    public TextMeshProUGUI Text;

    private int waveCount = 0;


    public void SetWaveCount(int waveCount)
    { 
        this.waveCount = waveCount;
    }

    public void DisplayWaveCount()
    {
        Text.text = "WAVE " + waveCount;
    }
}
