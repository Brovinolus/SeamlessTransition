using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private SceneData sceneData;

    public SceneData Data => sceneData;

    public void LoadNewScene()
    {
        if (sceneData.sceneNameToLoad != null)
        {
            SceneTransitionManager.Instance.LoadScene(sceneData);
        }
        else
        {
            Debug.Log("sceneNameToLoad is empty");
        }
    }
}
