using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class LoadingPopup : BasePopup<LoadingPopup.Data>
{
    [Inject] private SceneChangeManager _sceneChangeManager;
    [Inject] private PopupManager _popupManager;

    private void Start()
    {
        DOVirtual.DelayedCall(PopupData.WaitDuration, () =>
        {
            _popupManager.Hide(PopupData.Name);
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