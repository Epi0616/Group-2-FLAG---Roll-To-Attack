using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 roomOverviewPosition, arenaPlayPosition, mainMenuPosition, settingsPosition, arenaHighOverviewPosition;
    [SerializeField] private Quaternion roomOverviewRotation, arenaPlayRotation, menuRotation, arenaHighOverviewRotation;
    [SerializeField] private float roomFov, menuFov;

    [SerializeField] private Vector3 offset;
    [SerializeField] private Camera cam;

    private Coroutine breathingRoutine;

    private void OnEnable()
    {
        DiceProp.GameStart += HandleMoveIntoArena;
        SceneTransitionManager.FadeFromArena += HandleFadeFromArena;
        IntroSceneMenuUI.menuOpened += HandleMoveToMainMenu;
        IntroSceneMenuUI.settingsOpened += HandleMoveToSettings;
        IntroSceneMenuUI.menuClosed += HandleMoveToRoomOverview;
    }

    private void OnDisable()
    {
        DiceProp.GameStart -= HandleMoveIntoArena;
        SceneTransitionManager.FadeFromArena -= HandleFadeFromArena;
        IntroSceneMenuUI.menuOpened -= HandleMoveToMainMenu;
        IntroSceneMenuUI.settingsOpened -= HandleMoveToSettings;
        IntroSceneMenuUI.menuClosed -= HandleMoveToRoomOverview;
    }
    private void Start()
    {
        cam.fieldOfView = menuFov;
        transform.localPosition = mainMenuPosition;
        transform.localRotation = menuRotation;
    }

    public void HandleFadeFromArena (float transitionLength, Vector3 dicePosition)
    {
        transform.localPosition = dicePosition + offset;
        transform.localRotation = arenaPlayRotation;
        arenaPlayPosition = transform.localPosition;

        StartCoroutine(FadeFromArena(transitionLength));
    }

    private IEnumerator FadeFromArena(float transitionLength)
    {        
        StartCoroutine(EaseOutPositionToFrom(transitionLength, arenaHighOverviewPosition, transform.position));
        StartCoroutine(EaseOutRotationToFrom(transitionLength, arenaHighOverviewRotation, transform.rotation));


        yield return new WaitForSeconds(transitionLength * 1.25f);
        StartCoroutine(EaseOutPositionToFrom(transitionLength * 1.5f, mainMenuPosition, transform.position));
        StartCoroutine(EaseOutRotationToFrom(transitionLength * 1.5f, menuRotation, transform.rotation));
        StartCoroutine(FovToFrom(transitionLength * 1.5f, menuFov, cam.fieldOfView));
        //yield return MoveToMainMenu(transitionLength * 2);
        //breathingRoutine = StartCoroutine(StartBreathingRoutine(transitionLength));
    }

    private void HandleMoveToRoomOverview(float transitionLength)
    {
        StartCoroutine(FovToFrom(transitionLength, roomFov, cam.fieldOfView));
        StartCoroutine(EaseOutPositionToFrom(transitionLength, roomOverviewPosition, transform.position));
        StartCoroutine(EaseOutRotationToFrom(transitionLength, roomOverviewRotation, transform.rotation));
        breathingRoutine = StartCoroutine(StartBreathingRoutine(transitionLength));
    }

    private void HandleMoveIntoArena(GameObject dice, DiceType diceType)
    {
        arenaPlayPosition = dice.transform.position + offset;

        StartCoroutine(FovToFrom(SceneTransitionManager.transitionLength, roomFov, cam.fieldOfView));
        StartCoroutine(PositionToFrom(SceneTransitionManager.transitionLength, arenaPlayPosition, transform.position));
        StartCoroutine(RotationToFrom(SceneTransitionManager.transitionLength, arenaPlayRotation, transform.rotation));
        EndBreathingRoutine();
    }

    private void HandleMoveToMainMenu(float transitionLength)
    {
        StartCoroutine(MoveToMainMenu(transitionLength));
        EndBreathingRoutine();
    }

    private IEnumerator MoveToMainMenu(float transitionLength)
    {
        StartCoroutine(FovToFrom(transitionLength, menuFov, cam.fieldOfView));
        StartCoroutine(EaseOutPositionToFrom(transitionLength, mainMenuPosition, transform.position));
        yield return EaseOutRotationToFrom(transitionLength, menuRotation, transform.rotation);
    }

    private void HandleMoveToSettings(float transitionLength)
    {
        StartCoroutine(FovToFrom(transitionLength, menuFov, cam.fieldOfView));
        StartCoroutine(EaseOutPositionToFrom(transitionLength, settingsPosition, transform.position));
        StartCoroutine(EaseOutRotationToFrom(transitionLength, menuRotation, transform.rotation));
        EndBreathingRoutine();
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

    private IEnumerator EaseOutPositionToFrom(float duration, Vector3 to, Vector3 from)
    {
        float timer = duration;
        float t = 0;
        float easeOutT = 0;
        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;
            easeOutT = 1 - Mathf.Pow((1 - t), 2);

            transform.localPosition = Vector3.Lerp(from, to, easeOutT);
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

    private IEnumerator EaseOutRotationToFrom(float duration, Quaternion to, Quaternion from)
    {
        float timer = duration;
        float t = 0;
        float easeOutT = 0;
        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;
            easeOutT = 1 - Mathf.Pow((1 - t), 2);

            transform.localRotation = Quaternion.Lerp(from, to, easeOutT);
            yield return null;
        }

        transform.localRotation = to;
    }

    private IEnumerator FovToFrom(float duration, float to, float from)
    {
        float timer = duration;
        float t = 0;
        float easeOutT = 0;
        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;
            easeOutT = 1 - Mathf.Pow((1 - t), 2);

            cam.fieldOfView = Mathf.Lerp(from, to, easeOutT);
            yield return null;
        }
    }

    private void EndBreathingRoutine()
    {
        if (breathingRoutine != null)
        {
            StopCoroutine(breathingRoutine);
            breathingRoutine = null;
        }
    }

    private IEnumerator StartBreathingRoutine(float delay)
    {
        EndBreathingRoutine();
        yield return new WaitForSeconds(delay);
        yield return BreathingRoutine();
    }

    private IEnumerator BreathingRoutine()
    {
        Vector3 startPosition = transform.position;
        Vector3 topBreath = startPosition + new Vector3(0, 1f, 0);
        Vector3 bottomBreath = startPosition + new Vector3(0, -1, 0);

        yield return new WaitForSeconds(1);
        yield return PositionToFrom(2.5f, topBreath, startPosition);

        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            yield return PositionToFrom(5, bottomBreath, topBreath);
            yield return new WaitForSeconds(1);
            yield return PositionToFrom(5, topBreath, bottomBreath);
        }
    }
}
