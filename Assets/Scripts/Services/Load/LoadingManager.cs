using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class LoadingManager : Manager
{
    [Inject] private PopupManager _popupManager;
    public override void Init()
    {
        _dependencyList.Add(_popupManager);
    }

    public override void Begin()
    {
        _popupManager.Show(new LoadingPopup.Data(2));
        Resolve();
    }
}