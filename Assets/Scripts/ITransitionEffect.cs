using System.Collections.Generic;
using UnityEngine;

public interface ITransitionEffect {
    void LoadNewSceneTransitionEffect(float transitionDuration);
    void CurrentSceneLoadTransitionEffect(float transitionDuration);
}
