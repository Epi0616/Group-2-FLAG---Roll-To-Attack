using UnityEngine;
using System.Collections.Generic;
using System.Collections;
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

        foreach (TutorialStage stage in stages)
        {
            yield return StartStage(stage);
        }
        Debug.Log("Tutorial finished");
    }

    public IEnumerator StartStage(TutorialStage stage)
    {
        currentStage = stage;
        TutorialTextBoxObj.SetActive(true);
        foreach (TutorialStep step in stage.TextLines)
        {
            textBox.DisplayText(step.Text);
            boxRect.anchoredPosition = step.pos;

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
                //Vector2 pos = Vector2.zero;
                //if (boxRect.anchoredPosition.x > 0)
                //{
                //    if (boxRect.anchoredPosition.x > 800)
                //    {
                //        pos.x = -boxWidth;
                //    }
                //    else
                //    {
                //        pos.x = boxWidth;
                //    }                       
                //}
                //else
                //{
                //    if (boxRect.anchoredPosition.x < -800)
                //    {
                //        pos.x = boxWidth;
                //    }
                //    else
                //    {
                //        pos.x = -boxWidth;
                //    }                       
                //}
                //portraitRect.anchoredPosition = pos;
            }

            if (step.pausesGame)
            {
                //ToggleGameplayPause(true);
                Time.timeScale = 0;
            }

            yield return step.condition.Wait(this);

            //ToggleGameplayPause(false);
            Time.timeScale = 1;
            TutorialPortraitObj.SetActive(false);
        }
        TutorialTextBoxObj.SetActive(false);
        Debug.Log("Display for Stage Finished");
    }

}
