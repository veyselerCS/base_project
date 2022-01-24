using System;
using UnityEngine.SceneManagement;

public class SceneChangeManager : Manager
{
    public override void Init()
    {
    }

    public override void Begin()
    {
        Resolve();
    }

    public void ChangeScene(string sceneName, Action onComplete = null)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += (asyncOperation) =>
        {
            onComplete?.Invoke();
        };
    }
}