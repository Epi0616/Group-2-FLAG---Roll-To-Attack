using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Timers;
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject TutorialTextBoxObj;
    [SerializeField] private GameObject TutorialPortraitObj;
    private TutorialTextBox textBox;
    private RectTransform boxRect;
    private RectTransform portraitRect;
    [SerializeField] private List<TutorialStage> stages = new List<TutorialStage>();
    private float boxWidth = 424f;
    private TutorialStage currentStage;
    private Coroutine CurrentStageCO;
    private Coroutine TimeScalingCO;
    public bool restartCurrentStep;
    public bool restartCurrentStage;
    public Player player;
    public bool hasJump, hasMovement;
    public ConditionalActionDescriptor jumpSO;
    public ConditionalMovementDescriptor chargeSO;
    public ConditionalMovementDescriptor movementSO;

    public static event Action<int> StartIndexWave;
    public void Start()
    {
        textBox = TutorialTextBoxObj.GetComponentInChildren<TutorialTextBox>();
        boxRect = TutorialTextBoxObj.GetComponent<RectTransform>();
        portraitRect = TutorialPortraitObj.GetComponent<RectTransform>();
        StartCoroutine(StartTutorialDisplay());
    }

    public IEnumerator StartTutorialDisplay()
    {
        yield return new WaitUntil(() => !Input.GetMouseButton(0));
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        int stageIndex = 0;
        while (stageIndex < stages.Count)
        {
            CurrentStageCO = StartCoroutine(StartStage(stages[stageIndex]));
            yield return CurrentStageCO;
            stageIndex++;
        }
        Debug.Log("Tutorial finished");
    }

    public IEnumerator StartStage(TutorialStage stage)
    {
        currentStage = stage;
        TutorialTextBoxObj.SetActive(true);
        int stepIndex = 0;
        while (stepIndex < stage.TextLines.Count)
        {
            Debug.Log("Starting Step " + stepIndex);
            restartCurrentStep = false;
            textBox.DisplayText(stage.TextLines[stepIndex].Text);
            boxRect.anchoredPosition = stage.TextLines[stepIndex].pos;

            if (stage.TextLines[stepIndex].condition is DummyDeathCondition temp)
            {
                StartIndexWave?.Invoke(temp.waveIndex);
            }
            if (stage.TextLines[stepIndex].unlocksJump && !hasJump)
            {
                hasJump = true;
                player.actionController.AddNewAction(jumpSO.Create());
                player.movementController.AddNewMovement(chargeSO.Create());
                
            }
            if (stage.TextLines[stepIndex].unlocksMovement && !hasMovement)
            {
                player.movementController.AddNewMovement(movementSO.Create());
            }
            

            if (stage.TextLines[stepIndex].usesPortrait)
            {
                TutorialPortraitObj.SetActive(true);
                float x = boxRect.anchoredPosition.x;
                float dir = Mathf.Sign(x);
                if (Mathf.Abs(x) > 800)
                {
                    dir *= -1;
                }
                portraitRect.anchoredPosition = new Vector2(dir * boxWidth, 0);
            }

            if (stage.TextLines[stepIndex].pausesGame)
            {               
                TimeScalingCO = StartCoroutine(ScaleTimeSmoothly(0f, 1f));               
            }

            yield return stage.TextLines[stepIndex].condition.Wait(this);

            if (!restartCurrentStep)
            {
                Debug.Log("Condition Fufilled");
                stepIndex++;
            }
            restartCurrentStep = false;
            //Time.timeScale = 1;
            TimeScalingCO = StartCoroutine(ScaleTimeSmoothly(1f, 0.25f));
            TutorialPortraitObj.SetActive(false);
        }
        TutorialTextBoxObj.SetActive(false);
        Debug.Log("Display for Stage Finished");
    }

    public IEnumerator ScaleTimeSmoothly(float scale, float duration)
    {
        float startScale = Time.timeScale;
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / duration;
            t = Mathf.SmoothStep(0f, 1f, t);
            Time.timeScale = Mathf.Lerp(startScale, scale, easeOutCubic(t));
            yield return null;
        }
        Time.timeScale = scale;
    }

    public float easeOutCubic(float x)
    {
        return 1 - Mathf.Pow(1f - x, 3f);
    }
}
