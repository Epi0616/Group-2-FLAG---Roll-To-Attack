using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceProp : MoveableProp, IIntroRollable
{
    public static event Action<GameObject, DiceType> GameStart;
    public static event Action TransitionStart, TransitionOver;

    [SerializeField] private Vector3 startScale, targetScale;
    [SerializeField] private DiceType myDiceType;
    [SerializeField] GameObject highlightObj;
    private bool isOutlined;

    private bool gameStarted;
    private bool returningFromArena = false;

    private Coroutine expirationCoroutine;

    protected override void OnEnable()
    {
        base.OnEnable();

        SceneTransitionManager.DiceReturnFromArena += HandleReturnFromArena;
    }

    protected void OnDisable()
    {
        SceneTransitionManager.DiceReturnFromArena -= HandleReturnFromArena;
    }

    protected override void Initialize()
    {
        base.Initialize();
        UpdateOutline(false);
        canBeMoved = true;
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
        TransitionStart?.Invoke();
        canBeMoved = false;
        returningFromArena = true;
        yield return ScaleToFrom(transitionLength, startScale, targetScale);

        TransitionOver?.Invoke();
        transform.position = startPosition;
        returningFromArena = false;
        canBeMoved = true;
    }

    public void RollToPosition(Vector3 targetPos)
    {
        TransitionStart?.Invoke();
        StartCoroutine(RollToTarget(targetPos));
    }

    public IEnumerator RollToTarget(Vector3 targetPos)
    {
        float distance = (transform.position - targetPos).magnitude;

        Vector3 direction = targetPos - transform.position;
        float x = Random.Range(-10, 10);
        float y = Random.Range(-10, 10);
        float z = Random.Range(-10, 10);
        Vector3 angularVel = new Vector3(x, y, z);

        rb.angularVelocity = angularVel;
        rb.linearVelocity = direction;
        while (distance >= 10)
        {
            Vector3 temp = rb.linearVelocity;
            temp.y -= 9.8f * Time.deltaTime;
            rb.linearVelocity = temp;

            yield return null;
        }
    }

    public override void ObjectHovered()
    {
        UpdateOutline(true);
    }

    public override void ObjectUnHovered()
    {
        UpdateOutline(false);
    }

    public override void ObjectDropped()
    {
        UpdateOutline(false);
        if(expirationCoroutine != null) { StopCoroutine(expirationCoroutine); }
        expirationCoroutine = StartCoroutine(ReturnToOriginalPosition(6f));
    }

    protected override IEnumerator ReturnToOriginalPosition(float waitTime)
    {
        while (waitTime > 0)
        {
            //rb.AddForce(new Vector3(0, -10, 0), ForceMode.Acceleration);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (returningFromArena) return;

        if (collision.collider.CompareTag("Ground"))
        {
            if (!gameStarted)
            {
                gameStarted = true;
                canBeMoved = false;
                StartCoroutine(StartGameAfterDiceSettle());
            }

            return;
        }

        TransitionOver?.Invoke();
    }

    private IEnumerator StartGameAfterDiceSettle()
    {
        while (rb.angularVelocity.magnitude > 1f)
        {
            yield return null;
        }

        //StartCoroutine(RotateToFrom(5, Quaternion.Euler(0, 0, 0), transform.localRotation));
        GameStart?.Invoke(gameObject, myDiceType);
        yield return ScaleToFrom(SceneTransitionManager.transitionLength, targetScale, startScale);
    }

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

    private void UpdateOutline(bool state)
    {
        if (state == isOutlined) { return; }
        isOutlined = state;
        if (state)
        {
            highlightObj.SetActive(true);
            return;
        }
        highlightObj.SetActive(false);
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
