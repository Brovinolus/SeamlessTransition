using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [SerializeField] private SceneData currentSceneData;
    [SerializeField] private List<GameObject> buttons;
    [SerializeField] private GameObject transitionEffectGameObject;
    private Dictionary<string, Outline> _buttonsData = new Dictionary<string, Outline>();
    private ITransitionEffect _transitionEffect;
    private GameObject _persistentGameObject;

    private bool _isTransitionInProgress;

    public GameObject GameObjectForNewScene
    {
        set => _persistentGameObject = value;
    }

    private void Awake()
    {
        Singleton();

        foreach (var button in buttons)
        {
            _buttonsData.Add(button.GetComponent<SceneTransition>().Data.sceneNameToLoad, button.GetComponent<Outline>());
        }
        
        LocateTransitionEffectInNewScene();
        
        HandleButtons(false);
        OnNewSceneLoadComplete(currentSceneData);
    }

    private void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(SceneData sceneData)
    {
        if (_isTransitionInProgress) return;
        _isTransitionInProgress = true;

        HandleButtons(false);
        
        if(sceneData.sceneNameToLoad != null) OnLoadStart(sceneData);
    }

    private void HandleButtons(bool buttonState)
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Button>().enabled = buttonState;
        }
    }

    private void OnLoadStart(SceneData sceneData)
    {
        _transitionEffect?.LoadNewSceneTransitionEffect(sceneData.transitionNewSceneLoadDuration);
        HandleGameObjectsForNewScene(sceneData.canDestroypersistentGameObject, sceneData.transitionNewSceneLoadDuration);
        
        StartCoroutine(WaitForEndOfLoading(sceneData));
    }
    
    private IEnumerator WaitForEndOfLoading(SceneData sceneData)
    {
        yield return new WaitForSeconds(sceneData.transitionNewSceneLoadDuration);

        var sceneToLoad = SceneManager.LoadSceneAsync(sceneData.sceneNameToLoad, LoadSceneMode.Additive);
        
        while (!sceneToLoad.isDone)
        {
            yield return null;
            transitionEffectGameObject = null;
            _transitionEffect = null;
        }

        var sceneToUnload = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        
        while (!sceneToUnload.isDone)
        {
            yield return null;
        }

        OnNewSceneLoadComplete(sceneData);
    }

    private void HandleButtonsOutline(string sceneNameToLoad)
    {
        foreach (var buttonData in _buttonsData)
        {
            if (buttonData.Key == sceneNameToLoad)
            {
                buttonData.Value.effectDistance = new Vector2(4, -4);
            }
            else
            {
                buttonData.Value.effectDistance = new Vector2(0, 0);
            }
        }
    }
    
    private void HandleGameObjectsForNewScene(bool canDestroy, float transitionDuration)
    {
        if(!_persistentGameObject) return;
        
        if (canDestroy)
        {
            _persistentGameObject.GetComponent<ITransitionEffect>()?.LoadNewSceneTransitionEffect(transitionDuration);
        }
        else
        {
            DontDestroyOnLoad(_persistentGameObject);
        }
    }

    private void OnNewSceneLoadComplete(SceneData sceneData)
    {
        _isTransitionInProgress = true;
        
        LocateTransitionEffectInNewScene();
        
        if(sceneData.canDestroypersistentGameObject) Destroy(_persistentGameObject);
        HandleButtonsOutline(sceneData.sceneNameToLoad);

        currentSceneData = sceneData;

        if (_transitionEffect != null)
        {
            _transitionEffect.CurrentSceneLoadTransitionEffect(sceneData.transitionCurrentSceneLoadDuration);
            StartCoroutine(WaitForCurrentSceneTransition(sceneData.transitionCurrentSceneLoadDuration));
        }
        else
        {
            HandleButtons(true);
        
            _isTransitionInProgress = false;
        }

    }

    private IEnumerator WaitForCurrentSceneTransition(float transitionCurrentSceneLoadDuration)
    {
        yield return new WaitForSeconds(transitionCurrentSceneLoadDuration);
        
        HandleButtons(true);
        
        _isTransitionInProgress = false;
    }

    private void LocateTransitionEffectInNewScene()
    {
        transitionEffectGameObject = GameObject.Find("TransitionEffect");
        if (transitionEffectGameObject)
        {
            _transitionEffect = transitionEffectGameObject.GetComponent<ITransitionEffect>();
        }
        else
        {
            Debug.Log("There is no Transition");
        }
    }
}
