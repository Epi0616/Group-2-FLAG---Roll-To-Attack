using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        if (TransitionManager.instance == null)
        {
            SceneManager.LoadScene("MainBuild");
        }
        else
        {
            TransitionManager.LoadScene("MainBuild", 0.5f, 1f);
        }

            
    }

    public void Options()
    { 
        //add options???
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
