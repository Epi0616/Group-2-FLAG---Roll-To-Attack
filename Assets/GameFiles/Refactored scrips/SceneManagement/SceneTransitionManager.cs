using NUnit.Framework;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Scene = UnityEngine.SceneManagement.Scene;

public class SceneTransitionManager : MonoBehaviour
{
    public static event Action<float, Vector3> FadeFromArena;
    public static event Action<float, Vector3, DiceType> DiceReturnFromArena;

    [SerializeField] private string sandArena;
    [SerializeField] private string introScene;

    public DiceType previouslySelectedDiceType;

    private void OnEnable()
    {
        DiceProp.GameStart += HandleGameStart;
        PauseMenu.ReturnToIntro += HandleIntroStart;
        GameOverMenu.ReturnToIntro += HandleIntroStart;
    }

    private void OnDisable()
    {
        DiceProp.GameStart -= HandleGameStart;
        PauseMenu.ReturnToIntro -= HandleIntroStart;
        GameOverMenu.ReturnToIntro -= HandleIntroStart;
    }

    void Start()
    {
        StartCoroutine(LoadIntroFromMainScene());
    }

    private IEnumerator LoadIntroFromMainScene()
    {
        yield return StartCoroutine(LoadSceneAsync(introScene));
        yield return StartCoroutine(TimeSlicedSceneActivation(introScene));
    }

    private void HandleIntroStart()
    {
        StartCoroutine(IntroStart(2.5f));
    }

    private IEnumerator IntroStart(float transitionLength)
    {
        Coroutine load = StartCoroutine(LoadSceneAsync(introScene));
        //yield return new WaitForSeconds(transitionLength / 2);
        yield return load;

        yield return StartCoroutine(TimeSlicedSceneActivation(introScene));
        SetUpIntroScene(transitionLength / 2);

        yield return UnloadSceneAsync(sandArena);
    }

    private void SetUpIntroScene(float transitionLength)
    {
        Vector3 dicePosition = Vector3.zero;

        Scene scene = SceneManager.GetSceneByName(sandArena);
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

    private void HandleGameStart(GameObject dice, DiceType diceType, float transitionLength)
    {
        previouslySelectedDiceType = diceType;
        StartCoroutine(SandArenaStart(dice, transitionLength));
    }

    private IEnumerator SandArenaStart(GameObject dice, float transitionLength)
    {
        Coroutine load = StartCoroutine(LoadSceneAsync(sandArena));
        yield return new WaitForSeconds(transitionLength);
        yield return load;

        SetUpArena(dice.transform.position);
        yield return StartCoroutine(TimeSlicedSceneActivation(sandArena));

        yield return UnloadSceneAsync(introScene);
    }

    private void SetUpArena(Vector3 dicePosition)
    {
        GameObject[] rootObjects =  SceneManager.GetSceneByName(sandArena).GetRootGameObjects();
        foreach (GameObject rootObject in rootObjects)
        {
            if (rootObject.CompareTag("Player"))
            { 
                rootObject.transform.position = dicePosition;
            }
            //if (rootObject.GetComponent<Camera>())
            //{
            //    rootObject.transform.position = dicePosition + new Vector3(0, 50, -35); //slightly poor coding here as this will have to be mannually updated based on the offset of the player camera.
            //}
        }

    }

    private IEnumerator LoadSceneAsync(string scene)
    { 
        Stopwatch stopwatch = Stopwatch.StartNew();
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        sceneLoad.allowSceneActivation = false;

        while (!sceneLoad.isDone)
        {
            if (sceneLoad.progress >= 0.9f)
            {
                stopwatch.Start();
                sceneLoad.allowSceneActivation = true;
            }

            yield return null;
        }
        stopwatch.Stop();
        Debug.Log("ms elapsed on load frame: " + stopwatch.ElapsedMilliseconds);
    }

    private IEnumerator TimeSlicedSceneActivation(string scene)
    {
        GameObject[] rootObjects = SceneManager.GetSceneByName(scene).GetRootGameObjects();

        Stopwatch stopwatch = Stopwatch.StartNew();
        stopwatch.Start();

        foreach (GameObject rootObject in rootObjects)
        {
            if (rootObject.GetComponent<Camera>())
            {
                Camera camera = rootObject.GetComponent<Camera>();
                CameraManager.instance.AddCamera(camera);
                CameraManager.instance.SetActiveCamera(camera);
            }

            if (rootObject.TryGetComponent<IInitializeable>(out var initializable))
            { 
                yield return StartCoroutine(initializable.InitializeAsync());
            }
            rootObject.SetActive(true);
            yield return null;
        }
        stopwatch.Stop();
        Debug.Log("ms elapsed on activation timeslice: " + stopwatch.ElapsedMilliseconds);
    }

    private IEnumerator UnloadSceneAsync(string scene)
    {
        Scene selectedScene = SceneManager.GetSceneByName(scene);
        if (selectedScene.isLoaded)
        {
            yield return TimeSlicedSceneDeactivation(scene);
            yield return SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }
        yield return null;
    }

    private IEnumerator TimeSlicedSceneDeactivation(string scene) //since ive commented out yield return null in the main loop, its not really time sliced anymore. Im just assuming the deactivation cost is drammatically lower than activation
    {
        GameObject[] rootObjects = SceneManager.GetSceneByName(scene).GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            if (rootObject.GetComponent<Camera>())
            {
                Camera camera = rootObject.GetComponent<Camera>();
                CameraManager.instance.RemoveCamera(camera);
            }

            rootObject.SetActive(false);
            //yield return null; 
        }

        yield return null;
    }
}
