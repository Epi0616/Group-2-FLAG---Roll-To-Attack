using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 roomOverviewPosition, arenaPlayPosition, mainMenuPosition, settingsPosition;
    [SerializeField] private Quaternion roomOverviewRotation, arenaPlayRotation, menuRotation;
    [SerializeField] private float roomFov, menuFov;

    [SerializeField] private Vector3 offset;
    [SerializeField] private Camera cam;

    private void OnEnable()
    {
        MoveableProp.GameStart += MoveIntoArena;
        SceneTransitionManager.FadeFromArena += FadeFromArena;
        IntroSceneMenuUI.menuOpened += MoveToMainMenu;
        IntroSceneMenuUI.settingsOpened += MoveToSettings;
        IntroSceneMenuUI.menuClosed += MoveToRoomOverview;
    }

    private void OnDisable()
    {
        MoveableProp.GameStart -= MoveIntoArena;
        SceneTransitionManager.FadeFromArena -= FadeFromArena;
        IntroSceneMenuUI.menuOpened -= MoveToMainMenu;
        IntroSceneMenuUI.settingsOpened -= MoveToSettings;
        IntroSceneMenuUI.menuClosed -= MoveToRoomOverview;
    }
    private void Start()
    {
        cam.fieldOfView = menuFov;
        transform.localPosition = mainMenuPosition;
        transform.localRotation = menuRotation;
    }

    public void FadeFromArena (float transitionLength, Vector3 dicePosition)
    {
        Debug.Log("fading from arena");
        transform.localPosition = dicePosition + offset;
        transform.localRotation = arenaPlayRotation;
        arenaPlayPosition = transform.localPosition;

        StartCoroutine(FovToFrom(transitionLength, roomFov, cam.fieldOfView));
        StartCoroutine(PositionToFrom(transitionLength, roomOverviewPosition, transform.position));
        StartCoroutine(RotationToFrom(transitionLength, roomOverviewRotation, transform.rotation));
    }

    private void MoveToRoomOverview(float transitionLength)
    {
        StartCoroutine(FovToFrom(transitionLength, roomFov, cam.fieldOfView));
        StartCoroutine(EaseOutPositionToFrom(transitionLength, roomOverviewPosition, transform.position));
        StartCoroutine(EaseOutRotationToFrom(transitionLength, roomOverviewRotation, transform.rotation));
    }

    private void MoveIntoArena(GameObject dice, DiceType diceType, float transitionLength)
    {
        arenaPlayPosition = dice.transform.position + offset;

        StartCoroutine(FovToFrom(transitionLength, roomFov, cam.fieldOfView));
        StartCoroutine(PositionToFrom(transitionLength, arenaPlayPosition, transform.position));
        StartCoroutine(RotationToFrom(transitionLength, arenaPlayRotation, transform.rotation));
    }

    private void MoveToMainMenu(float transitionLength)
    {
        StartCoroutine(FovToFrom(transitionLength, menuFov, cam.fieldOfView));
        StartCoroutine(EaseOutPositionToFrom(transitionLength, mainMenuPosition, transform.position));
        StartCoroutine(EaseOutRotationToFrom(transitionLength, menuRotation, transform.rotation));
    }

    private void MoveToSettings(float transitionLength)
    {
        StartCoroutine(FovToFrom(transitionLength, menuFov, cam.fieldOfView));
        StartCoroutine(EaseOutPositionToFrom(transitionLength, settingsPosition, transform.position));
        StartCoroutine(EaseOutRotationToFrom(transitionLength, menuRotation, transform.rotation));
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
}
