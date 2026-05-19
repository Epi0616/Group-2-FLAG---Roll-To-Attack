using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public static Action creditsOver;

    [SerializeField] private GameObject blackOutScreen;
    [SerializeField] private ScrollRect creditScrollRect;
    [SerializeField] private Image fadeToBlackScreen;

    [SerializeField] private InputActionReference exitCreditsKeyboard, exitCreditsController, creditsSpeedUpController;

    private float scrollFast = 1f;

    private bool isScrolling = false;
    Coroutine fade = null;
    private GameObject previouslySelectedMenuButton;

    private void OnEnable()
    {
        creditsSpeedUpController.action.Enable();
        exitCreditsController.action.performed += HandleExitCredits;
        exitCreditsKeyboard.action.performed += HandleExitCredits;
        MainMenu.rollCredits += StartCredits;
    }

    private void OnDisable()
    {
        creditsSpeedUpController.action.Disable();
        exitCreditsController.action.performed -= HandleExitCredits;
        exitCreditsKeyboard.action.performed -= HandleExitCredits;
        MainMenu.rollCredits -= StartCredits;
    }

    private void Awake()
    {
        fadeToBlackScreen.color = new Color(0, 0, 0, 0);
        fadeToBlackScreen.gameObject.SetActive(false);
        blackOutScreen.SetActive(false);
        creditScrollRect.gameObject.SetActive(false);
        isScrolling = false;
        fade = null;
    }

    private void Update()
    {
        if (!isScrolling) return;

        HandleScrollFast();
        creditScrollRect.content.position += Vector3.up * Time.deltaTime * 75 * scrollFast;
        if (creditScrollRect.content.anchoredPosition.y >= 1900) // needs to be updated depending on size of content
        {
            if (fade != null) return;
            fade = StartCoroutine(FadeOut(2));
        }
    }

    private void HandleScrollFast()
    {
        if (creditsSpeedUpController.action.IsPressed())
        {
            scrollFast = 5;
        }
        else
        {
            scrollFast = 1;
        }
    }

    public void StartCredits()
    {
        StartCoroutine(FadeIn());
        previouslySelectedMenuButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.firstSelectedGameObject = null;
        EventSystem.current.SetSelectedGameObject(null);
    }

    private IEnumerator FadeIn()
    {
        fadeToBlackScreen.gameObject.SetActive(true);
        blackOutScreen.SetActive(true);
        creditScrollRect.gameObject.SetActive(true);

        isScrolling = true;
        creditScrollRect.content.position = new Vector3(creditScrollRect.content.position.x, 0, creditScrollRect.content.position.z);

        fadeToBlackScreen.color = new Color(0, 0, 0, 1);
        float timer = 2;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            

            fadeToBlackScreen.color = new Color(0, 0, 0, (timer / 2) + 0.1f);
            yield return null;
        }

        fadeToBlackScreen.color = new Color(0, 0, 0, 0);
    }

    private void HandleExitCredits(InputAction.CallbackContext context)
    {
        if (!isScrolling) return;
        StartCoroutine(FadeOut(1));
    }    

    private IEnumerator FadeOut(float fadeTime)
    {
        float timer = fadeTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            fadeToBlackScreen.color = new Color(0, 0, 0, (fadeTime - timer)/fadeTime);
            yield return null;
        }

        fadeToBlackScreen.color = new Color(0, 0, 0, 0);
        fadeToBlackScreen.gameObject.SetActive(false);
        blackOutScreen.SetActive(false);
        creditScrollRect.gameObject.SetActive(false);
        isScrolling = false;
        fade = null;

        EventSystem.current.firstSelectedGameObject = previouslySelectedMenuButton;
        UISelectionManager.instance.TrySetSelectedGameObject(previouslySelectedMenuButton);
        creditsOver?.Invoke();
    }
}
