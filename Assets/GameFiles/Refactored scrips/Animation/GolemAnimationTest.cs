using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GolemAnimationTest : MonoBehaviour
{
    [SerializeField] AnimationClip waddleClip;
    [SerializeField] AnimationClip attackClip;
    [SerializeField] Animator animator;

    private PlayableGraph graph;
    private AnimationMixerPlayable mixer;
    private AnimationPlayableOutput output;
    private AnimationClipPlayable currentClip;

    private void OnDisable()
    {
        graph.Destroy();
    }

    private void Start()
    {
        graph = PlayableGraph.Create("GolemAnimationGraph");
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        mixer = AnimationMixerPlayable.Create(graph, 2);

        output = AnimationPlayableOutput.Create(graph, "Animation", animator);

        AnimationClipPlayable playableWaddle = AnimationClipPlayable.Create(graph, waddleClip);


        graph.Play();

        PlayAnimation(playableWaddle);
        StartCoroutine(AnimationLoop());
    }

    private IEnumerator AnimationLoop()
    {
        yield return new WaitForSeconds(2);

        AnimationClipPlayable playableAttack = AnimationClipPlayable.Create(graph, attackClip);
        AnimationClipPlayable playableWaddle = AnimationClipPlayable.Create(graph, waddleClip);
        while (true)
        {
            yield return StartCoroutine(CrossFade(playableAttack));
            yield return new WaitForSeconds(attackClip.length);
            yield return StartCoroutine(CrossFade(playableWaddle));
            yield return new WaitForSeconds(1);
        }
    }

    private void PlayAnimation(AnimationClipPlayable newAnimation)
    { 
        newAnimation.SetTime(0);
        output.SetSourcePlayable(newAnimation);
        currentClip = newAnimation;
    }

    private IEnumerator CrossFade(AnimationClipPlayable newAnimation, float duration = 0.2f)
    {
        graph.Connect(currentClip, 0, mixer, 0);
        graph.Connect(newAnimation, 0, mixer, 1);
        output.SetSourcePlayable(mixer);

        newAnimation.SetTime(0);

        float timer = duration;
        float t = 0f;

        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;

            mixer.SetInputWeight(0, 1f - t);
            mixer.SetInputWeight(1, t);
            yield return null;
        }

        currentClip = newAnimation;
        output.SetSourcePlayable(newAnimation);

        graph.Disconnect(mixer, 0);
        graph.Disconnect(mixer, 1);
    }
}
