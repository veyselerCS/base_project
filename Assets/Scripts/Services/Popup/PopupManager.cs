using System;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class PopupManager : Manager
{
    [Inject]
    private AddressableManager _addressableManager;
    [Inject(Id = ZenjectIdConstants.PopupCanvas)]
    public Canvas PopupCanvas;
    
    public override void Init()
    {
        _dependencyList.Add(_addressableManager);
    }

    public override void Begin()
    {
        Resolve();
    }

    public async void Show(BasePopupData data)
    {
       var popupGO = await _addressableManager.LoadAsset<GameObject>(data.Name);
       var popup = Instantiate(popupGO, PopupCanvas.transform);
    }
}