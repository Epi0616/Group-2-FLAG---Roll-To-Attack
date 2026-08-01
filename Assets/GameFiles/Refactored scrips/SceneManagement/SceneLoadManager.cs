using System.Collections;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Scene = UnityEngine.SceneManagement.Scene;

public class SceneLoadManager : MonoBehaviour
{
    public IEnumerator LoadSceneAsync(SceneType sceneType)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneType.HumanName(), LoadSceneMode.Additive);

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

    public IEnumerator TimeSlicedSceneActivation(SceneType sceneType)
    {
        GameObject[] rootObjects = SceneManager.GetSceneByName(sceneType.HumanName()).GetRootGameObjects();

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

    public IEnumerator UnloadSceneAsync(SceneType sceneType)
    {
        Scene selectedScene = SceneManager.GetSceneByName(sceneType.HumanName());
        if (selectedScene.isLoaded)
        {
            yield return TimeSlicedSceneDeactivation(sceneType);
            yield return SceneManager.UnloadSceneAsync(sceneType.HumanName(), UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }
        yield return null;
    }

    public IEnumerator TimeSlicedSceneDeactivation(SceneType sceneType) //since ive commented out yield return null in the main loop, its not really time sliced anymore. Im just assuming the deactivation cost is drammatically lower than activation
    {
        GameObject[] rootObjects = SceneManager.GetSceneByName(sceneType.HumanName()).GetRootGameObjects();

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
