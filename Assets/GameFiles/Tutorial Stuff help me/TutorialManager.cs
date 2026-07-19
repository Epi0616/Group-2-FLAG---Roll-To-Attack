using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject TutorialTextBoxObj;
    [SerializeField] private GameObject TutorialPortraitObj;
    [SerializeField] private GameObject TutorialUIBlockerObj;
    [SerializeField] private GameObject TutorialDarkOverlay;
    [SerializeField] private RectTransform TutorialOverlayCutout;
    private TutorialTextBox textBox;
    private RectTransform boxRect;
    private RectTransform portraitRect;
    private Image DarkOverlayImage;
    private Color overlayColour;
    [SerializeField] private List<TutorialStage> stages = new List<TutorialStage>();
    private float boxWidth = 424f;
    private TutorialStage currentStage;
    private Coroutine CurrentStageCO;
    private Coroutine TimeScalingCO;
    public bool restartCurrentStep;
    public bool restartCurrentStage;
    public Player player;
    public bool hasJump, hasMovement;
    public bool stepComplete;
    public ConditionalActionDescriptor jumpSO;
    public ConditionalMovementDescriptor chargeSO;
    public ConditionalMovementDescriptor movementSO;
    private bool nextStepHighlted, isHighlighted;
    public static event Action<int> StartIndexWave;
    public static event Action DisplayDiceUI;
    public void Start()
    {
        textBox = TutorialTextBoxObj.GetComponentInChildren<TutorialTextBox>();
        boxRect = TutorialTextBoxObj.GetComponent<RectTransform>();
        portraitRect = TutorialPortraitObj.GetComponent<RectTransform>();
        DarkOverlayImage = TutorialDarkOverlay.GetComponent<Image>();
        overlayColour = DarkOverlayImage.color;
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
        while (stepIndex < stage.TutorialSteps.Count)
        {
            Debug.Log("Starting Step " + stepIndex);
            restartCurrentStep = false;
            stepComplete = false;
   
            boxRect.anchoredPosition = stage.TutorialSteps[stepIndex].pos;
            HandleText(stage.TutorialSteps[stepIndex]);
            HandlePortrait(stage.TutorialSteps[stepIndex]);
            if (stage.TutorialSteps[stepIndex].highlightElement && stage.TutorialSteps[stepIndex + 1] != null)
            {
                if (stage.TutorialSteps[stepIndex + 1].highlightElement)
                {
                    Debug.Log("Next Step Highlighted");
                    nextStepHighlted = true;
                }
                else
                {
                    nextStepHighlted = false;
                }
            }
            else
            {
                nextStepHighlted = false;
            }
            StartCoroutine(HandleBetterHighlighting(stage.TutorialSteps[stepIndex], 0.5f));
            


            if (stage.TutorialSteps[stepIndex].condition is DummyDeathCondition temp)
            {
                StartIndexWave?.Invoke(temp.waveIndex);
            }
            else if (stage.TutorialSteps[stepIndex].bringUpSelectionUI)
            {
                DisplayDiceUI?.Invoke();
            }

            if (stage.TutorialSteps[stepIndex].pausesGame)
            {
                yield return TimeScalingCO = StartCoroutine(ScaleTimeSmoothly(0f, 1f));
            }
            if (stage.TutorialSteps[stepIndex].blocksUIInteraction)
            {
                TutorialUIBlockerObj.SetActive(true);
            }

            HandleUnlocks(stage.TutorialSteps[stepIndex]);          

            yield return stage.TutorialSteps[stepIndex].condition.Wait(this);
            
            if (stage.TutorialSteps[stepIndex].pausesGame)
            {
                yield return TimeScalingCO = StartCoroutine(ScaleTimeSmoothly(1f, 0.25f));
            }

            TutorialPortraitObj.SetActive(false);
            TutorialUIBlockerObj.SetActive(false);
            StartCoroutine(RemoveHighlighting(stage.TutorialSteps[stepIndex], 0.5f));

            if (!restartCurrentStep)
            {
                stage.TutorialSteps[stepIndex].hasBeenReset = false;
                stepIndex++;
            }
            else
            {
                stage.TutorialSteps[stepIndex].hasBeenReset = true;
            }
            restartCurrentStep = false;
        }
        TutorialTextBoxObj.SetActive(false);
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

    private void HandleUnlocks(TutorialStep step)
    {
        if (step.unlocksJump && !hasJump)
        {
            hasJump = true;
            player.actionController.AddNewAction(jumpSO.Create());
            player.movementController.AddNewMovement(chargeSO.Create());

        }

        if (step.unlocksMovement && !hasMovement)
        {
            player.movementController.AddNewMovement(movementSO.Create());
        }
    }

    private void HandleText(TutorialStep step)
    {
        if (step.hasBeenReset && step.ResetText != null)
        {
            textBox.DisplayText(step.ResetText);
            step.hasBeenReset = false;
        }
        else
        {
            textBox.DisplayText(step.Text);
        }
    }

    private void HandlePortrait(TutorialStep step)
    {
        if (step.usesPortrait)
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
    }

    private IEnumerator HandleHighlighting(TutorialStep step, float duration)
    {
        if (step.highlightElement)
        {
            TutorialOverlayCutout.anchoredPosition = step.HighlightPos;
            //TutorialOverlayCutout.sizeDelta = step.HighlightScale;
            //Vector2 startScale = new Vector2(step.HighlightScale.x * 1.2f, step.HighlightScale.y + 1.2f);
            //float timer = 0;
            //while (timer < duration)
            //{
            //    timer += Time.unscaledDeltaTime;
            //    float t = timer / duration;
            //    t = Mathf.SmoothStep(0f, 1f, t);
            //    TutorialOverlayCutout.sizeDelta = Vector2.Lerp(startScale, step.HighlightScale, easeOutCubic(t));
            //    yield return null;
            //}
            TutorialOverlayCutout.sizeDelta = step.HighlightScale;
            if (!isHighlighted)
            {
                DarkOverlayImage.color = new Color(overlayColour.r, overlayColour.g, overlayColour.b, 0f);
                TutorialDarkOverlay.SetActive(true);
                float startAlpha = 0f;
                float timer = 0;
                while (timer < duration)
                {
                    timer += Time.unscaledDeltaTime;
                    float t = timer / duration;
                    t = Mathf.SmoothStep(0f, 1f, t);
                    float a = Mathf.Lerp(startAlpha, overlayColour.a, easeOutCubic(t));
                    DarkOverlayImage.color = new Color(overlayColour.r, overlayColour.g, overlayColour.b, a);
                    yield return null;
                }
                DarkOverlayImage.color = new Color(overlayColour.r, overlayColour.g, overlayColour.b, 0.8f);
                isHighlighted = true;
            }            
        }
    }

    private IEnumerator HandleBetterHighlighting(TutorialStep step, float duration)
    {
        if (step.highlightElement)
        {
            Vector2 targetSize = step.HighlightScale;          
            
            bool hasMoved = false;
            if (!(TutorialOverlayCutout.anchoredPosition == step.HighlightPos))
            {
                TutorialOverlayCutout.anchoredPosition = step.HighlightPos;
                hasMoved = true;
                TutorialOverlayCutout.sizeDelta = step.HighlightScale * 1.5f;
            }
            Vector2 startSize = TutorialOverlayCutout.sizeDelta;

            float startAlpha = 0f;

            if (!isHighlighted)
            {
                DarkOverlayImage.color = new Color(overlayColour.r, overlayColour.g, overlayColour.b, 0f);
                TutorialDarkOverlay.SetActive(true);
            }
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.unscaledDeltaTime;
                float t = timer / duration;
                t = Mathf.SmoothStep(0f, 1f, t);
                float eased = easeOutCubic(t);

                
                if (!isHighlighted)
                {
                    float a = Mathf.Lerp(startAlpha, overlayColour.a, eased);
                    DarkOverlayImage.color = new Color(overlayColour.r, overlayColour.g, overlayColour.b, a);
                }
                if (hasMoved)
                {
                    TutorialOverlayCutout.sizeDelta = Vector2.Lerp(startSize, targetSize, eased);
                }

                yield return null;
            }

            TutorialOverlayCutout.sizeDelta = targetSize;

            if (!isHighlighted)
            {
                DarkOverlayImage.color = overlayColour;
                isHighlighted = true;
            }
        }
    }

    private IEnumerator RemoveHighlighting(TutorialStep step, float duration)
    {
        if (step.highlightElement && !nextStepHighlted)
        {
            Debug.Log("Removing Overlay");
            float startAlpha = DarkOverlayImage.color.a;
            float timer = 0;
            while (timer < duration)
            {
                timer += Time.unscaledDeltaTime;
                float t = timer / duration;
                t = Mathf.SmoothStep(0f, 1f, t);
                float a = Mathf.Lerp(startAlpha, 0f, easeOutCubic(t));
                DarkOverlayImage.color = new Color(overlayColour.r, overlayColour.g, overlayColour.b, a);
                yield return null;
            }
            DarkOverlayImage.color = new Color(overlayColour.r, overlayColour.g, overlayColour.b, 0f);
            TutorialDarkOverlay.SetActive(false);
            isHighlighted = false;
        }
    }


    public float easeOutCubic(float x)
    {
        return 1 - Mathf.Pow(1f - x, 3f);
    }
}
