using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIController : MonoBehaviour
{
    [SerializeField] //TODO make upgrade path names case consistent, implement money, implement path limitations, implement requirments, add a cap to upgrades
    float UISlideSpeed;

    [SerializeField]
    private GameObject handsOverlay,
        exitButton,
        enterButton;
    [SerializeField]
    private CanvasGroup FadeInTransitionAlpha;

    private RectTransform handsOverlayRectTransform;

    private void Awake() {
        handsOverlayRectTransform = handsOverlay.GetComponent<RectTransform>();
    }
    private void Start() {
        StartCoroutine(Fade(true));
    }
    private IEnumerator Fade(bool doIn) {
        if (doIn) {
            FadeInTransitionAlpha.alpha = 1;
            while (FadeInTransitionAlpha.alpha > 0) {
                FadeInTransitionAlpha.alpha -= 1 * Time.deltaTime;
                yield return null;
            }
            FadeInTransitionAlpha.alpha = 0;
        } else {
            FadeInTransitionAlpha.alpha = 0;
            while (FadeInTransitionAlpha.alpha < 1) {
                FadeInTransitionAlpha.alpha += 1 * Time.deltaTime;
                yield return null;
            }
            FadeInTransitionAlpha.alpha = 1;
        }
    }
    public void ExitButton() {
        exitButton.SetActive(false);
        enterButton.SetActive(true);
        StopCoroutine(nameof(SlideTowersOverlay));
        StartCoroutine(nameof(SlideTowersOverlay), false);
    }
    public void EnterButton() {
        enterButton.SetActive(false);
        exitButton.SetActive(true);
        StopCoroutine(nameof(SlideTowersOverlay));
        StartCoroutine(nameof(SlideTowersOverlay), true);
    }
    private IEnumerator SlideTowersOverlay(bool isEnter) {
        float finalXPosition;
        if (!isEnter)
            finalXPosition = -handsOverlayRectTransform.sizeDelta.x;
        else
            finalXPosition = 0;
        while (Mathf.Round(handsOverlayRectTransform.anchoredPosition.x) != finalXPosition) {
            handsOverlayRectTransform.anchoredPosition = new Vector2(
            Mathf.Lerp(handsOverlayRectTransform.anchoredPosition.x, finalXPosition, UISlideSpeed * Time.deltaTime),
            handsOverlayRectTransform.anchoredPosition.y);
            yield return null;
        }
        handsOverlayRectTransform.anchoredPosition = new Vector2(finalXPosition,
            handsOverlayRectTransform.anchoredPosition.y);
    }
}
