using TMPro;
using UnityEngine;

public class PlayerInterfaceWaveBreakTime : MonoBehaviour
{
    public TextMeshProUGUI Text;
    private int currentBreakTime = 0, totalBreakTime = 5;
    float timer;

    public void SetWaveTime(int breakTime)
    { 
        this.totalBreakTime = breakTime;
        timer = 0;
    }

    public void DisplayWaveTime()
    {
        Text.text = "NEXT WAVE IN " + currentBreakTime;
    }

    public void Hide()
    { 
        gameObject.SetActive(false);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        currentBreakTime = totalBreakTime - Mathf.FloorToInt(timer);

        DisplayWaveTime();
    }
}