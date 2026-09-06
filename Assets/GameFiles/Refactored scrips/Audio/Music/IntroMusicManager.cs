using UnityEngine;

public class IntroMusicManager : MonoBehaviour
{
    [SerializeField] MusicPackage ambientMusic;

    private void Start()
    {
        StartDefaultMusic();
    }

    private void StartDefaultMusic()
    {
        MusicPlayer.instance?.PlayMusicWithFade(ambientMusic, 2);
    }
}
