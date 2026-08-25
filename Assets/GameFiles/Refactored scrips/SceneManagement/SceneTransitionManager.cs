using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class SceneTransitionManager : MonoBehaviour
{
    public static event Action<float, Vector3> FadeFromArena;
    public static event Action<float, Vector3, DiceType> DiceReturnFromArena;
    public static float transitionLength = 2.5f;

    [SerializeField] private SceneLoadManager sceneLoadManager;

    public DiceType previouslySelectedDiceType;
    private SceneType selectedArena;

    private void OnEnable()
    {
        IntroSceneMenuUI.arenaTypeSelected += HandleArenaTypeChange;
        DiceProp.GameStart += HandleArenaStart;
        PauseMenu.ReturnToIntro += HandleIntroStart;
        GameOverMenu.ReturnToIntro += HandleIntroStart;
        TutorialManager.TutorialOver += HandleIntroStart;
    }

    private void OnDisable()
    {
        IntroSceneMenuUI.arenaTypeSelected -= HandleArenaTypeChange;
        DiceProp.GameStart -= HandleArenaStart;
        PauseMenu.ReturnToIntro -= HandleIntroStart;
        GameOverMenu.ReturnToIntro -= HandleIntroStart;
        TutorialManager.TutorialOver -= HandleIntroStart;
    }

    void Start()
    {
        selectedArena = SceneType.SandArena;
        StartCoroutine(LoadIntroFromMainScene());
    }

    private IEnumerator LoadIntroFromMainScene()
    {
        yield return StartCoroutine(sceneLoadManager.LoadSceneAsync(SceneType.IntroScene));
        yield return StartCoroutine(sceneLoadManager.TimeSlicedSceneActivation(SceneType.IntroScene));
    }

    private void HandleArenaTypeChange(SceneType sceneType)
    { 
        selectedArena = sceneType;
    }

    private void HandleIntroStart()
    {
        StartCoroutine(IntroStart(2.5f));
    }

    private IEnumerator IntroStart(float transitionLength)
    {
        Coroutine load = StartCoroutine(sceneLoadManager.LoadSceneAsync(SceneType.IntroScene));
        //yield return new WaitForSeconds(transitionLength / 2);
        yield return load;

        yield return StartCoroutine(sceneLoadManager.TimeSlicedSceneActivation(SceneType.IntroScene));
        SetUpIntroScene(transitionLength / 2);

        yield return sceneLoadManager.UnloadSceneAsync(selectedArena);

        selectedArena = SceneType.SandArena;
    }

    private void SetUpIntroScene(float transitionLength)
    {
        Vector3 dicePosition = Vector3.zero;

        Scene scene = SceneManager.GetSceneByName(selectedArena.HumanName());
        if (scene.IsValid())
        {
            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObject in rootObjects)
            {
                if (rootObject.CompareTag("Player"))
                {
                    dicePosition = rootObject.transform.position;
                }
            }
        }
        DiceReturnFromArena?.Invoke(transitionLength, dicePosition, previouslySelectedDiceType);
        FadeFromArena?.Invoke(transitionLength, dicePosition);
    }

    private void HandleArenaStart(GameObject dice, DiceType diceType)
    {
        previouslySelectedDiceType = diceType;
        StartCoroutine(ArenaStart(dice, transitionLength));
    }

    private IEnumerator ArenaStart(GameObject dice, float transitionLength)
    {
        Coroutine load = StartCoroutine(sceneLoadManager.LoadSceneAsync(selectedArena));
        yield return new WaitForSeconds(transitionLength);
        yield return load;

        SetUpArena(dice.transform.position, selectedArena);
        yield return StartCoroutine(sceneLoadManager.TimeSlicedSceneActivation(selectedArena));

        yield return sceneLoadManager.UnloadSceneAsync(SceneType.IntroScene);
    }

    private void SetUpArena(Vector3 dicePosition, SceneType arenaType)
    {
        GameObject[] rootObjects =  SceneManager.GetSceneByName(arenaType.HumanName()).GetRootGameObjects();
        foreach (GameObject rootObject in rootObjects)
        {
            if (rootObject.CompareTag("Player"))
            { 
                rootObject.transform.position = dicePosition;
            }
        }
    }
}

public enum SceneType
{ 
    IntroScene,
    TutorialArena,
    SandArena
}
