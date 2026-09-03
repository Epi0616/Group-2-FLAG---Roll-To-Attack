using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;

    [SerializeField] private AudioSource source;
    
    private PlayableGraph graph;
    private AudioPlayableOutput output;
    private AudioMixerContainer mainMixer;
    
    private bool graphActive = false;
    private int dampenRequests = 0;
    private Coroutine dampenRoutine;

    private void OnEnable()
    {
        SetupAudioGraph();
    }

    private void OnDisable()
    {
        graphActive = false;
        graph.Destroy();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetupAudioGraph()
    {
        graph = PlayableGraph.Create("MusicGraph");
        graph.SetTimeUpdateMode(DirectorUpdateMode.DSPClock);
        mainMixer = new AudioMixerContainer(
            AudioMixerPlayable.Create(graph, 2, true),
            MusicType.None
            );

        output = AudioPlayableOutput.Create(graph, "MusicOutput", source);

        output.SetSourcePlayable(mainMixer.mixer);
        graph.Play();
        graphActive = true;
    }

    public void DampenMusic()
    {
        dampenRequests++;
        if (dampenRequests > 1) return;
        if (dampenRoutine != null)
        {
            StopCoroutine(dampenRoutine);
        }
        dampenRoutine = StartCoroutine(SoundDampening(0.8f, source.spatialBlend, 0.1f));
    }

    public void UndampenMusic()
    {
        dampenRequests--;
        if (dampenRequests > 0) return;
        if (dampenRoutine != null)
        {
            StopCoroutine(dampenRoutine);
        }
        dampenRoutine = StartCoroutine(SoundDampening(0f, source.spatialBlend, 0.1f));
    }

    private IEnumerator SoundDampening(float to, float from, float duration)
    {
        float timer = 0;
        float t = 0;
        while (t < 1)
        {
            timer += Time.unscaledDeltaTime;
            t = timer / duration;

            source.spatialBlend = Mathf.Lerp(from, to, t);
            yield return null;
        }

        source.spatialBlend = to;
    }

    public void PlayMusic(MusicPackage audioPackage, float window = 0f)
    {
        if (!graphActive) return;
        if (!CheckForCanBePlayed(mainMixer, audioPackage)) return;

        AudioClipPlayable newPlayable = CreatePlayable(audioPackage);

        CancelCurrentCrossFade(mainMixer);
        DestroyMixerPlayable(mainMixer, 0);

        ConnectPlayable(newPlayable, mainMixer);

        mainMixer.currentType = audioPackage.musicType;
        source.volume = audioPackage.volume;
    }

    public void PlayMusicWithFade(MusicPackage audioPackage, float crossFadeDuration = 0.2f, float window = 0f)
    {
        if (!graphActive) return;
        if (!CheckForCanBePlayed(mainMixer, audioPackage)) return;

        CancelCurrentCrossFade(mainMixer);

        AudioClipPlayable newPlayable = CreatePlayable(audioPackage);
        Playable currentPlayable = mainMixer.mixer.GetInput(0);
        mainMixer.mixer.DisconnectInput(0);

        mainMixer.crossFadeRoutine = StartCoroutine(ConnectPlayableCrossFade(newPlayable, currentPlayable, mainMixer, crossFadeDuration));

        mainMixer.currentType = audioPackage.musicType;
        source.volume = audioPackage.volume;
    }

    private void ConnectPlayable(AudioClipPlayable newPlayable, AudioMixerContainer mixer)
    {
        graph.Connect(newPlayable, 0, mixer.mixer, 0);

        newPlayable.SetTime(0);
        mixer.mixer.SetInputWeight(0, 1);
    }

    private IEnumerator ConnectPlayableCrossFade(AudioClipPlayable newPlayable, Playable currentPlayable, AudioMixerContainer mixer, float crossFadeDuration)
    {
        graph.Connect(newPlayable, 0, mixer.mixer, 0);
        graph.Connect(currentPlayable, 0, mixer.mixer, 1);

        newPlayable.SetTime(0);

        float timer = crossFadeDuration;
        float t = 0f;

        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (crossFadeDuration - timer) / crossFadeDuration;

            mixer.mixer.SetInputWeight(0, t);
            mixer.mixer.SetInputWeight(1, 1f - t);
            yield return null;
        }

        mixer.mixer.SetInputWeight(0, 1);
        DestroyMixerPlayable(mixer, 1);
    }

    private void CancelCurrentCrossFade(AudioMixerContainer mixer)
    {
        if (mixer.crossFadeRoutine != null)
        {
            StopCoroutine(mixer.crossFadeRoutine);
            DestroyMixerPlayable(mixer, 1);
            mixer.mixer.SetInputWeight(0, 1);
        }
    }

    private void DestroyMixerPlayable(AudioMixerContainer mixer, int index)
    {
        if (mixer.mixer.GetInput(index).IsValid())
        {
            Playable temp = mixer.mixer.GetInput(index);
            graph.Disconnect(mixer.mixer, index);
            temp.Destroy();
        }
    }

    private AudioClipPlayable CreatePlayable(AudioPackage audioPackage)
    {
        return AudioClipPlayable.Create(graph, audioPackage.audioClips[0], true); //assuming there will only ever be 1 desired piece of music and it will be looping...
    }

    private bool CheckForCanBePlayed(AudioMixerContainer mixer, MusicPackage musicPackage)
    {
        if (musicPackage.musicType == mixer.currentType) return false;
        return true;
    }
}

public class AudioMixerContainer
{
    public AudioMixerPlayable mixer;
    public MusicType currentType;
    public Coroutine crossFadeRoutine;

    public AudioMixerContainer(AudioMixerPlayable mixer, MusicType currentType)
    {
        this.mixer = mixer;
        this.currentType = currentType;
    }
}

public enum MusicType
{
    None,
    Ambient,
    Wave,
    Boss
}