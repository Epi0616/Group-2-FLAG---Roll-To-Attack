using UnityEngine;

public class ArenaMusicManager : MonoBehaviour
{
    [SerializeField] MusicPackage defaultMusic, bossMusic;

    private void OnEnable()
    {
        WaveManager.UpdateWaveBar += HandleWaveType;
        WaveManager.WaveOver += HandleWaveOver;
    }

    private void OnDisable()
    {
        WaveManager.UpdateWaveBar -= HandleWaveType;
        WaveManager.WaveOver -= HandleWaveOver;
    }

    private void Start()
    {
        StartDefaultMusic();
    }

    private void HandleWaveType(WaveType waveType)
    {
        if (waveType != WaveType.normal)
        {
            StartBossMusic();
        }
        else 
        {
            StartDefaultMusic();
        }
    }

    private void HandleWaveOver(float delayBetweenWaves)
    { 
        StartDefaultMusic();
    }

    private void StartDefaultMusic()
    {
        MusicPlayer.instance?.PlayMusicWithFade(defaultMusic, 5);
    }

    private void StartBossMusic()
    {
        MusicPlayer.instance?.PlayMusicWithFade(bossMusic, 2.5f);
    }
}
