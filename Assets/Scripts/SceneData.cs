using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SceneData", menuName = "ScriptableObjects/ScenesData/SceneData")]
public class SceneData : ScriptableObject
{
    public string sceneNameToLoad;
    public float transitionNewSceneLoadDuration;
    public float transitionCurrentSceneLoadDuration;
    [FormerlySerializedAs("canDestroy")] public bool canDestroypersistentGameObject = true;
}
