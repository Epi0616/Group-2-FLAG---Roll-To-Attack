using UnityEngine;
using System;
using System.Collections;

public class MoveableProp : MonoBehaviour
{
    public static event Action<GameObject, DiceType, float> GameStart;

    [SerializeField] private Vector3 startScale, targetScale;
    [SerializeField] private DiceType myDiceType;

    private Rigidbody rb;
    private Vector3 startPosition;
    private bool gameStarted;
    private bool returningFromArena = false;

    private void OnEnable()
    {
        SceneTransitionManager.DiceReturnFromArena += HandleReturnFromArena;

        Initialize();
    }

    private void OnDisable()
    {
        SceneTransitionManager.DiceReturnFromArena -= HandleReturnFromArena;
    }

    private void Initialize()
    {
        startPosition = transform.position;
        transform.localScale = startScale;
        rb = GetComponent<Rigidbody>();
    }

    public void MoveToPosition(Vector3 targetPos)
    { 
        Vector3 direciton = targetPos - transform.position;

        rb.linearVelocity = direciton * 40f;
    }

    public void RollToPosition(Vector3 targetPos)
    {
        Vector3 direction = transform.position - targetPos;
        Vector3 explosionPos = transform.position + (direction.normalized * 100);
        explosionPos.y = 0;

        rb.AddExplosionForce(direction.magnitude * 50, explosionPos, 150);
        rb.AddForce(new(0, -1000, 0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (returningFromArena) return;

        if (collision.collider.CompareTag("Ground"))
        {
            if (!gameStarted)
            {
                gameStarted = true;
                StartCoroutine(StartGameAfterDiceSettle());
            }
        }
    }

    private IEnumerator StartGameAfterDiceSettle()
    {
        while (rb.angularVelocity.magnitude > 1f)
        { 
            yield return null;
        }


        //StartCoroutine(RotateToFrom(5, Quaternion.Euler(0, 0, 0), transform.localRotation));
        GameStart?.Invoke(gameObject, myDiceType, 5);
        yield return ScaleToFrom(5, targetScale, startScale);
    }

    public void ObjectDropped()
    { 
        StartCoroutine(ReturnToOriginalPosition(6f));
    }

    private IEnumerator ReturnToOriginalPosition(float waitTime)
    {
        while (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            yield return null;
        }

        if (!gameStarted)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = startPosition;
        }
    }

    //Vector3(69.9728622,1.18526292,42.467205)
    //Vector3(8.1592865,2.78605676,11.5202789)

    private IEnumerator ScaleToFrom(float duration, Vector3 to, Vector3 from)
    {
        float timer = duration;
        float t = 0;
        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;

            transform.localScale = Vector3.Lerp(from, to, t);
            yield return null;
        }

        transform.localScale = to;
    }

    private void HandleReturnFromArena(float transitionLength, Vector3 position, DiceType diceType)
    {
        if (diceType == myDiceType)
        {
            transform.position = position;
            StartCoroutine(ReturnFromArena(transitionLength));
        }
    }

    private IEnumerator ReturnFromArena(float transitionLength)
    {
        returningFromArena = true;
        yield return ScaleToFrom(transitionLength, startScale, targetScale);

        transform.position = startPosition;
        returningFromArena = false;
    }

    private IEnumerator RotateToFrom(float duration, Quaternion to, Quaternion from)
    {
        float timer = duration;
        float t = 0;
        rb.isKinematic = true;
        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;

            transform.localRotation = Quaternion.Lerp(from, to, t);
            yield return null;
        }

        transform.localRotation = to;
        rb.isKinematic = false;
    }
}

