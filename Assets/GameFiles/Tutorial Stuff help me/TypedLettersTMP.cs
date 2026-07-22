using UnityEngine;
using System;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

public class TypedLettersTMP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBox;

    private int currentVisibleCharacters;
    private Coroutine typingCoroutine;
    private WaitForSecondsRealtime simpleDelay;
    [SerializeField] private float charactersPerSecond = 10f;
    public bool isSkipping;
    public bool finishedTyping;
    public void Awake()
    {
        simpleDelay = new WaitForSecondsRealtime(1 / charactersPerSecond);
    }

    public void SetText(string text)
    {
        if (typingCoroutine != null) { StopCoroutine(typingCoroutine); }
       // Debug.Log("Finished Set to false from SetText()");
        finishedTyping = false;
        textBox.text = text;
        textBox.maxVisibleCharacters = 0;
        currentVisibleCharacters = 0;    
        isSkipping = false;
        typingCoroutine = StartCoroutine(Typing());
    }

    public IEnumerator Typing()
    {
        TMP_TextInfo textInfo = textBox.textInfo;

        while (currentVisibleCharacters < textBox.text.Length)
        {
            char character = textInfo.characterInfo[currentVisibleCharacters].character;
            textBox.maxVisibleCharacters++;
            yield return simpleDelay;
            currentVisibleCharacters++;
        }
       // Debug.Log("Finished Set to true from Typing()");
        finishedTyping = true;
    }

    public void Skip()
    {
        if (isSkipping) { return; }
        isSkipping = true;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        textBox.maxVisibleCharacters = textBox.text.Length;
        
    }
    public IEnumerator skipCompleteDelay()
    {
        yield return new WaitForSeconds(0.2f);
       // Debug.Log("Finished Set to true from SkipCoroutine()");
        finishedTyping = true;
    }
}
