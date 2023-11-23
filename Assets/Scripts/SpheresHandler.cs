using System.Collections.Generic;
using UnityEngine;

public class SpheresHandler : MonoBehaviour, ITransitionEffect
{
    [SerializeField] private List<GameObject> spheres;
    [SerializeField] private SceneData sceneData;
    private List<SphereInteraction> _spheresInteractions = new List<SphereInteraction>();
    private List<SphereMovement> _spheresMovements = new List<SphereMovement>();
    private SphereInteraction _sphere;

    public SceneData SceneData => sceneData;

    private void Awake()
    {
        foreach (var sphere in spheres)
        {
            _spheresMovements.Add(sphere.gameObject.GetComponent<SphereMovement>());
        }
        
        foreach (var sphere in spheres)
        {
            _spheresInteractions.Add(sphere.gameObject.GetComponent<SphereInteraction>());
        }
    }

    public void SetSphere(SphereInteraction sphere)
    {
        _sphere = sphere;
    }

    public void LoadNewSceneTransitionEffect(float transitionDuration)
    {
        if (_sphere) _spheresInteractions.Remove(_sphere);

        foreach (var spheresMovement in _spheresMovements)
        {
            spheresMovement.IsSphereClicked = true;
        }

        foreach (var spheresInteraction in _spheresInteractions)
        {
            spheresInteraction.StartFadingCoroutine(transitionDuration, 0f);
            spheresInteraction.IsInteracted = true;
        }
        
        if(_sphere) _sphere.GetComponent<SphereMovement>().StartMoveToCenterCoroutine();
    }

    public void CurrentSceneLoadTransitionEffect(float transitionDuration)
    {
        foreach (var spheresInteraction in _spheresInteractions)
        {
            spheresInteraction.StartFadingCoroutine(transitionDuration, 1f);
            spheresInteraction.IsInteracted = true;
        }
    }
}
