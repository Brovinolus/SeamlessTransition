using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextFading: MonoBehaviour, ITransitionEffect
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float fadeInDuration = 2;
    private Color _initialColor;

    private void Awake()
    {
        _initialColor = text.color;
    }

    public void LoadNewSceneTransitionEffect(float transitionDuration)
    {
        StartCoroutine(FadingText(transitionDuration, 0f));
    }

    public void CurrentSceneLoadTransitionEffect(float transitionDuration)
    {
        OnSceneLoad(transitionDuration);
    }
    
    private void OnSceneLoad(float transitionDuration)
    {
        StartCoroutine(FadingText(transitionDuration, 1f));
    }

    private IEnumerator FadingText(float fadeDuration, float alpha)
    {
        float startTime = Time.time;

        while (Time.time - startTime <= fadeDuration)
        {
            float elapsedTime = Time.time - startTime;
            float normalizedTime = elapsedTime / fadeDuration;
            float newAlpha = Mathf.Lerp(_initialColor.a, alpha, normalizedTime);
            Color newColor = new Color(_initialColor.r, _initialColor.g, _initialColor.b, newAlpha);
            text.color = newColor;

            yield return null;
        }
        
        text.color = new Color(_initialColor.r, _initialColor.g, _initialColor.b, alpha);
        _initialColor = text.color;
    }
}
