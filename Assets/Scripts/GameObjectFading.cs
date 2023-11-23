using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectFading : MonoBehaviour, ITransitionEffect
{
    private Material _sphereMaterial;
    private Color _initialColor;

    void Awake()
    {
        var renderer = GetComponent<Renderer>();
        _sphereMaterial = renderer.material;
        _initialColor = _sphereMaterial.color;
        Color newColor = new Color(_initialColor.r, _initialColor.g, _initialColor.b, 0);
        _sphereMaterial.color = newColor;
    }
    
    public void LoadNewSceneTransitionEffect(float transitionDuration)
    {
        StartCoroutine(FadingGameObject(transitionDuration, 0f));
    }

    public void CurrentSceneLoadTransitionEffect(float transitionDuration)
    {
        SceneTransition(transitionDuration, 1f);
    }

    public void SceneTransition(float transitionDuration, float alpha)
    {
        StartCoroutine(FadingGameObject(transitionDuration, alpha));
    }

    private IEnumerator FadingGameObject(float fadeDuration, float alpha)
    {
        float startTime = Time.time;

        while (Time.time - startTime <= fadeDuration)
        {
            float elapsedTime = Time.time - startTime;
            float normalizedTime = elapsedTime / fadeDuration;
            float newAlpha = Mathf.Lerp(_initialColor.a, alpha, normalizedTime);
            Color newColor = new Color(_initialColor.r, _initialColor.g, _initialColor.b, newAlpha);
            _sphereMaterial.color = newColor;

            yield return null;
        }
        
        Color finalColor = new Color(_initialColor.r, _initialColor.g, _initialColor.b, alpha);
        _sphereMaterial.color = finalColor;
        _initialColor = _sphereMaterial.color;
    }
}
