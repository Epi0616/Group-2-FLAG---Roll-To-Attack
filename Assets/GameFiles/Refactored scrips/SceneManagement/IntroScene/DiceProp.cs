using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceProp : MoveableProp, IIntroRollable
{
    public static event Action<GameObject, DiceType> GameStart;
    public static event Action TransitionStart, TransitionOver, TutorialDicePickedUp, TutorialDiceDropped;

    [SerializeField] private Vector3 startScale, targetScale;
    [SerializeField] private DiceType myDiceType;
    [SerializeField] GameObject highlightObj;
    [SerializeField] GameObject tutorialParticles;
    [SerializeField] Renderer myRenderer;

    private bool isOutlined;
    private bool enteringTutorial;

    private bool gameStarted;
    private bool returningFromArena = false;
    private bool eligableForGameStart = false;
    private bool falling = false;

    private Coroutine expirationCoroutine;

    protected override void OnEnable()
    {
        base.OnEnable();
        eligableForGameStart = false;

        SceneTransitionManager.DiceReturnFromArena += HandleReturnFromArena;
        IntroSceneMenuUI.arenaTypeSelected += SceneChosen;
        IntroSceneMenuUI.menuOpened += ReturnToMenu;
    }

    protected void OnDisable()
    {
        SceneTransitionManager.DiceReturnFromArena -= HandleReturnFromArena;
        IntroSceneMenuUI.arenaTypeSelected -= SceneChosen;
        IntroSceneMenuUI.menuOpened -= ReturnToMenu;
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
        eligableForGameStart = true;
        TransitionStart?.Invoke();
        StartCoroutine(RollToTarget(targetPos));
    }

    public IEnumerator RollToTarget(Vector3 targetPos)
    {
        float distance = (transform.position - targetPos).magnitude;

        Vector3 direction = targetPos - transform.position;

        rb.linearVelocity = direction;
        while (distance >= 10)
        {
            Vector3 temp = rb.linearVelocity;
            temp.y -= 9.8f * Time.deltaTime;
            rb.linearVelocity = temp;

            yield return null;
        }
    }
    public override void ObjectSelected()
    {
        if (enteringTutorial) { TutorialDicePickedUp?.Invoke(); tutorialParticles.SetActive(false); }
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
        if (enteringTutorial) { TutorialDiceDropped?.Invoke(); }
        if (expirationCoroutine != null) { StopCoroutine(expirationCoroutine); }
        expirationCoroutine = StartCoroutine(ReturnToOriginalPosition(4f));
    }

    protected override IEnumerator ReturnToOriginalPosition(float waitTime)
    {
        float x = Random.Range(-10, 10);
        float y = Random.Range(-10, 10);
        float z = Random.Range(-10, 10);
        Vector3 angularVel = new Vector3(x, y, z);

        rb.angularVelocity = angularVel;

        while (waitTime > 0)
        {
            if (!eligableForGameStart)
            {
                float downForce = -9.81f * Time.deltaTime * 1000;
                rb.AddForce(new Vector3(0, downForce, 0), ForceMode.Acceleration);
            }

            if (!myRenderer.isVisible)
            {
                yield return new WaitForSeconds(0.5f);
                break;
            }

            waitTime -= Time.deltaTime;
            yield return null;
        }

        if (!gameStarted)
        {
            eligableForGameStart = false;
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
            if (!gameStarted && eligableForGameStart)
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
        StartCoroutine(ScaleToFrom(SceneTransitionManager.transitionLength, targetScale, startScale));
        yield return new WaitForSeconds(SceneTransitionManager.transitionLength / 2.25f);
        HandleCorrectRotation(transform, 0.3f);
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

    private void SceneChosen(SceneType sceneType)
    {
        if (sceneType == SceneType.TutorialArena)
        {
            enteringTutorial = true;
            tutorialParticles.SetActive(true);
            return;
        }
        tutorialParticles.SetActive(false);
    }

    private void ReturnToMenu(float idk)
    {
        enteringTutorial = false;
        tutorialParticles.SetActive(false);
    }

    private void HandleCorrectRotation(Transform transform, float duration)
    {
        Vector3 currentRotation = transform.eulerAngles;

        float correctedX = currentRotation.x + FloatToTheNearest90(currentRotation.x);
        float correctedY = currentRotation.y + FloatToTheNearest90(currentRotation.y);
        float correctedZ = currentRotation.z + FloatToTheNearest90(currentRotation.z);

        Vector3 correctedRotation = new Vector3(correctedX, correctedY, correctedZ);
        StartCoroutine(CorrectRotation(transform, correctedRotation, duration));
    }

    private IEnumerator CorrectRotation(Transform transform, Vector3 targetRotationEuler, float duration)
    {
        float timer = 0;
        float t = 0;

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetRotationEuler);

        while (t < 1)
        {
            timer += Time.fixedDeltaTime;
            t = timer / duration;

            transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, t);
            yield return new WaitForFixedUpdate();
        }
    }

    private float FloatToTheNearest90(float value)
    {
        float remainder = value % 90f;
        float difference = (90f - remainder);

        return remainder > 45f ? difference : -remainder;
    }
}
