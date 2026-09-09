using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class TypedLettersTMP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBox;
    private List<Coroutine> characterCoroutines = new List<Coroutine>();
    private int currentVisibleCharacters;
    private Coroutine typingCoroutine;
    private WaitForSecondsRealtime simpleDelay;
    [SerializeField] private float charactersPerSecond = 10f;
    [SerializeField] private float fixedTypingDuration = 2.0f;
    [SerializeField] private float animationDuration = 0.15f, scaleMax = 1.25f, scaleMin = 0.5f;
    
    public bool isSkipping;
    public bool finishedTyping;
    public void Awake()
    {
        //simpleDelay = new WaitForSecondsRealtime(1 / charactersPerSecond);
    }

    public void SetText(string text)
    {
        if (typingCoroutine != null) { StopCoroutine(typingCoroutine); }
       // Debug.Log("Finished Set to false from SetText()");
        simpleDelay = new WaitForSecondsRealtime((fixedTypingDuration / text.Length));
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
            yield return simpleDelay;
            if (!PauseMenu.isGamePaused) 
            {
                textBox.maxVisibleCharacters++;
                currentVisibleCharacters++;
                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.ForceRebuildLayoutImmediate(textBox.rectTransform);
                textBox.ForceMeshUpdate();
                characterCoroutines.Add(StartCoroutine(AnimateTypedLetter(currentVisibleCharacters - 1)));
            }
            
        }
       // Debug.Log("Finished Set to true from Typing()");
        finishedTyping = true;
    }

    public void Skip()
    {
        if (isSkipping) { return; }
        if (PauseMenu.isGamePaused) { return; }
        isSkipping = true;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        StopCharacterAnims();
        textBox.maxVisibleCharacters = textBox.text.Length;
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(textBox.rectTransform);
        textBox.ForceMeshUpdate();

    }
    public IEnumerator skipCompleteDelay()
    {
        yield return new WaitForSeconds(0.2f);
       // Debug.Log("Finished Set to true from SkipCoroutine()");
        finishedTyping = true;
    }

    private void StopCharacterAnims()
    {
        foreach (Coroutine coroutine in characterCoroutines)
        {
            if (coroutine != null) { StopCoroutine(coroutine); }
        }

        characterCoroutines.Clear();       
    }
    private IEnumerator AnimateTypedLetter(int index)
    {
        TMP_CharacterInfo characterInfo = textBox.textInfo.characterInfo[index];
        if (!characterInfo.isVisible) { yield break; }

        int vertexIndex = characterInfo.vertexIndex;

        textBox.ForceMeshUpdate();

        Vector3[] vertices = textBox.textInfo.meshInfo[characterInfo.materialReferenceIndex].vertices;
        Vector3[] initialVertices = new Vector3[4];

        for (int i = 0; i < 4; i++)
        {
            initialVertices[i] = vertices[vertexIndex + i];
        }

        Vector3 characterCentre = Vector3.zero;

        for (int i = 0;i < 4; i++)
        {
            characterCentre += initialVertices[i];
        }

        characterCentre /= 4;

        float timer = 0;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            float t = timer / animationDuration;
            float scale;

            if (t < 0.5f)
            {
                float p = t / 0.5f;
                scale = Mathf.Lerp(scaleMin, scaleMax, p);
            }
            else
            {
                float p = (t - 0.5f) / 0.5f;
                scale = Mathf.Lerp(scaleMax, 1, p);
            }


           for (int i = 0; i < 4; i++)
           {
                Vector3 vertex = initialVertices[i];
                vertex = characterCentre + (vertex - characterCentre) * scale;
   
                vertices[vertexIndex + i] = vertex;
           }

            textBox.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            yield return null;

        }

        for (int i = 0; i < 4; i++)
        {
            vertices[vertexIndex + i] = initialVertices[i];
        }

        textBox.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}
