using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(GameObjectFading))]
public class SphereInteraction : MonoBehaviour, IInteract, ITransitionEffect
{
    [SerializeField] private SpheresHandler spheresHandler;
    [SerializeField] private GameObjectFading gameObjectFading;

    private SceneData _sceneData;
    private Material _sphereMaterial;
    private Color _initialColor;

    public bool IsInteracted { get; set; }

    private void Awake()
    {
        var renderer = GetComponent<Renderer>();
        _sphereMaterial = renderer.material;
        _initialColor = _sphereMaterial.color;
        gameObjectFading = GetComponent<GameObjectFading>();
        _sceneData = spheresHandler.SceneData;
    }
    
    public void Interact()
    {
        SphereSelected();
    }

    private void SphereSelected()
    {
        if (IsInteracted) return;
        IsInteracted = true;

        transform.DetachChildren();
        transform.parent = spheresHandler.transform.parent;

        spheresHandler.SetSphere(this);
        SceneTransitionManager.Instance.GameObjectForNewScene = gameObject;
        SceneTransitionManager.Instance.LoadScene(_sceneData);
    }

    public void LoadNewSceneTransitionEffect(float transitionDuration)
    {
        StartFadingCoroutine(transitionDuration, 0f);
    }

    public void CurrentSceneLoadTransitionEffect(float transitionDuration)
    {
        StartFadingCoroutine(transitionDuration, 1f);
    }
    
    public void StartFadingCoroutine(float fadeDuration, float alpha)
    {
        IsInteracted = true;
        gameObjectFading.SceneTransition(fadeDuration, alpha);
        StartCoroutine(WaitForEndOfTransition(fadeDuration));
    }
    
    private IEnumerator WaitForEndOfTransition(float fadeDuration)
    {
        yield return new WaitForSeconds(fadeDuration);
        IsInteracted = false;
    }
}
