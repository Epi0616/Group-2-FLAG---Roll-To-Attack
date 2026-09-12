using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroContinueButton : InteractableWritingScript
{
    [SerializeField] protected Button button;
    [SerializeField] protected GameObject greyOut;
    [SerializeField] protected ContinueInfo continueInfo;

    protected override void OnEnable()
    {
        CheckForValidSave();
        base.OnEnable();
    }

    private void CheckForValidSave()
    { 
        GameSaveData saveData = SaveSystem.instance.LoadSaveGameData();
        if (saveData == null)
        {
            SetInactive();
            return;
        }

        if (saveData.waveNumber <= 1 && !saveData.waveCleared)
        {
            SetInactive();
            return;
        }

        SetActive(saveData);
    }

    private void SetInactive()
    { 
        button.interactable = false;
        greyOut.SetActive(true);
        continueInfo.gameObject.SetActive(false);
    }

    private void SetActive(GameSaveData saveData)
    { 
        button.interactable = true;
        greyOut.SetActive(false);
        continueInfo.gameObject.SetActive(true);

        continueInfo.SetInfo(saveData.currentHp, saveData.waveNumber, saveData.runTimeStats.totalTimeSurvived);
    }

    protected override void OnPointerEnter()
    {
        if (!button.interactable) return;
        base.OnPointerEnter();
    }

    protected override void OnPointerExit()
    {
        if (!button.interactable) return;
        base.OnPointerExit();
    }

    protected override void OnPointerDown()
    {
        if (!button.interactable) return;
        base.OnPointerDown();
    }
}
