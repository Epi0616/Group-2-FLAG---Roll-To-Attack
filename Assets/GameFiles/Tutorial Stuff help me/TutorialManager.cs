using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject TutorialTextBoxObj;
    [SerializeField] private string wewa;
    private TutorialTextBox textBox;
    private RectTransform boxRect;
    [SerializeField] private List<TutorialStage> stages = new List<TutorialStage>();

    public void Start()
    {
        textBox = TutorialTextBoxObj.GetComponentInChildren<TutorialTextBox>();
        boxRect = TutorialTextBoxObj.GetComponent<RectTransform>();
        StartCoroutine(StartTutorialDisplay());
    }

    public void Update()
    {
        //timer += Time.deltaTime;
        //if (timer > interval)
        //{
        //    StartStage(stages[index]);
        //    if (index < stages.Count - 1)
        //    {
        //        Debug.Log(index);
        //        index++;
        //    }
        //    timer = 0;
        //}
    }

    public void StartStage(TutorialStage stage)
    {
        textBox.DisplayText(stage.TextLines[0].Text);
        boxRect.anchoredPosition = stage.TextLines[0].pos;
    }

    public IEnumerator StartTutorialDisplay()
    {
        yield return new WaitUntil(() => !Input.GetMouseButton(0));
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        foreach (TutorialStage stage in stages)
        {
            yield return DisplayText(stage);
        }
        Debug.Log("Tutorial finished");
    }

    public IEnumerator DisplayText(TutorialStage stage)
    {
        TutorialTextBoxObj.SetActive(true);
        foreach (TutorialText text in stage.TextLines)
        {
            textBox.DisplayText(text.Text);
            boxRect.anchoredPosition = text.pos;

            yield return new WaitUntil(() => !Input.GetMouseButton(0));
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }
        TutorialTextBoxObj.SetActive(false);
        Debug.Log("Display for Stage Finished");
    }
}
