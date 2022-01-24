using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class LoadingPopup : BasePopup<LoadingPopup.Data>
{
    [Inject] private SceneChangeManager _sceneChangeManager;

    private void Start()
    {
        Debug.LogWarning("Started");
        DOVirtual.DelayedCall(3F, () =>
        {
            _sceneChangeManager.ChangeScene(SceneNameConstants.MainScene);
        });
    }

    public class Data : BasePopupData
    {
        public float WaitDuration;

        public Data(float waitDuration)
        {
            Name = nameof(LoadingPopup);
            WaitDuration = waitDuration;
        }
    }
}