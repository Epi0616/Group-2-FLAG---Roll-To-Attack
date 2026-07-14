using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition, targetPosition;
    [SerializeField] private Quaternion startRotation, targetRotation;

    [SerializeField] private Vector3 offset;
    private void OnEnable()
    {
        MoveableProp.GameStart += StartAnimation;
        SceneTransitionManager.FadeFromArena += FadeFromArena;
    }

    private void OnDisable()
    {
        MoveableProp.GameStart -= StartAnimation;
        SceneTransitionManager.FadeFromArena -= FadeFromArena;
    }
    private void Start()
    {
        transform.localPosition = startPosition;
    }

    public void FadeFromArena (float transitionLength, Vector3 dicePosition)
    {
        Debug.Log("fading from arena");
        transform.localPosition = dicePosition + offset;
        transform.localRotation = targetRotation;
        targetPosition = transform.localPosition;

        StartCoroutine(PositionToFrom(transitionLength, startPosition, targetPosition));
        StartCoroutine(RotationToFrom(transitionLength, startRotation, targetRotation));
    }

    private void StartAnimation(GameObject dice, DiceType diceType, float transitionLength)
    {
        targetPosition = dice.transform.position + offset;

        StartCoroutine(PositionToFrom(transitionLength, targetPosition, startPosition));
        StartCoroutine(RotationToFrom(transitionLength, targetRotation, startRotation));
    }

    private IEnumerator PositionToFrom(float duration, Vector3 to, Vector3 from)
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

    private IEnumerator RotationToFrom(float duration, Quaternion to, Quaternion from)
    {
        float timer = duration;
        float t = 0;
        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;

            transform.localRotation = Quaternion.Lerp(from, to, t);
            yield return null;
        }

        transform.localRotation = to;
    }
}
