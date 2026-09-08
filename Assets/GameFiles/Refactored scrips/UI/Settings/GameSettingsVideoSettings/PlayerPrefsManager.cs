using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager instance;

    [SerializeField] private InputActionAsset Actions;
    public InputActionAsset actions { get => Actions; private set => Actions = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SetUpDefaultSettings();
            LoadBindings();

            return;
        }

        Destroy(gameObject);
    }

    private void SetUpDefaultSettings() //not sure of a better way to do this atm :/ (probably a per instance setup?? so i dont set somthing up that doesnt get used???)
    {
        if (!CheckForSetKey(PlayerValues.PostProcessing))
        {
            Debug.Log("setting defualt");
            SetBool(PlayerValues.PostProcessing, true);
        }

        if (!CheckForSetKey(PlayerValues.Language))
        {
            Debug.Log("setting defualt");
            SetString(PlayerValues.Language, "en");
        }

        if (!CheckForSetKey(PlayerValues.FullScreen))
        {
            Debug.Log("setting defualt");
            SetBool(PlayerValues.FullScreen, true);
        }

        if (!CheckForSetKey(PlayerValues.VSync))
        {
            Debug.Log("setting defualt");
            SetBool(PlayerValues.VSync, false);
        }

        if (!CheckForSetKey(PlayerValues.FPS))
        {
            Debug.Log("setting defualt");
            SetBool(PlayerValues.FPS, false);
        }
    }

    //custom set functions
    public void SetString(PlayerValues setting, string input)
    {
        PlayerPrefs.SetString(setting.ToBinaryString(), input);
    }
    public void SetInt(PlayerValues setting, int input)
    {
        PlayerPrefs.SetInt(setting.ToBinaryString(), input);
    }
    public void SetFloat(PlayerValues setting, float input)
    {
        PlayerPrefs.SetFloat(setting.ToBinaryString(), input);
    }
    public void SetBool(PlayerValues setting, bool input)
    {
        int boolean = input ? 1 : 2; 
        PlayerPrefs.SetInt(setting.ToBinaryString(), boolean);
    }


    //custom get functions
    public string GetString(PlayerValues setting)
    {
        return PlayerPrefs.GetString(setting.ToBinaryString());
    }
    public int GetInt(PlayerValues setting)
    {
        return PlayerPrefs.GetInt(setting.ToBinaryString());
    }

    public float GetFloat(PlayerValues setting)
    {
        return PlayerPrefs.GetFloat(setting.ToBinaryString());
    }

    public bool GetBool(PlayerValues setting, out bool outBool)
    {
        int boolean = PlayerPrefs.GetInt(setting.ToBinaryString());

        switch (boolean)
        {
            case 1:
                outBool = true;
                return true;
            case 2: 
                outBool = false;
                return true;
            default:
                outBool = false;
                return false;
        }
    }

    public bool CheckForSetKey(PlayerValues setting)
    {
        return PlayerPrefs.HasKey(setting.ToBinaryString());
    }

    public void SaveInputBindings()
    {
        string json = actions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString(PlayerValues.InputRebinds.ToBinaryString(), json);
        PlayerPrefs.Save();
    }

    public void LoadBindings()
    {
        string inputRebinds = PlayerValues.InputRebinds.ToBinaryString();

        if (!PlayerPrefs.HasKey(inputRebinds)) return;
        string json = PlayerPrefs.GetString(inputRebinds);

        if (string.IsNullOrWhiteSpace(json)) return;
        actions.LoadBindingOverridesFromJson(json);
    }
}

public enum PlayerValues
{
    //game
    Language,
    FullScreen,
    //video
    PostProcessing,
    FPS,
    VSync,
    //audio
    MasterVolumer,
    SFXVolume,
    MusicVolume,
    //data
    Name,
    HighScore,
    //controls
    InputRebinds
}
