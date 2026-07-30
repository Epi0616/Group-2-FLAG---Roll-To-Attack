using System.Collections;
using UnityEngine;

public class ArenaRise : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 targetPosition;

    private void OnEnable()
    {
        DiceProp.GameStart += StartAnimation;
        SceneTransitionManager.FadeFromArena += RevertAnimation;
    }

    private void OnDisable()
    {
        DiceProp.GameStart -= StartAnimation;
        SceneTransitionManager.FadeFromArena -= RevertAnimation;
    }
    private void Start()
    {
        transform.localPosition = startPosition;
    }

    private void RevertAnimation(float transitionLength, Vector3 dicePosition)
    {
        StartCoroutine(ArenaToFrom(transitionLength, startPosition, targetPosition));
    }

    private void StartAnimation(GameObject dice, DiceType diceType, float transitionLength)
    {
        StartCoroutine(ArenaToFrom(transitionLength, targetPosition, startPosition));
    }

    private IEnumerator ArenaToFrom(float duration, Vector3 to, Vector3 from)
    {
        float timer = duration;
        float t = 0;
        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            yield return null;
        }

        transform.localPosition = to;
    }
}
