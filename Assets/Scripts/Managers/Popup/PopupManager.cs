using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class PopupManager : Manager
{
    [Inject]
    private AddressableManager _addressableManager;
    [Inject(Id = ZenjectIdConstants.PopupCanvas)]
    public Canvas PopupCanvas;
    
    private List<BasePopup> _activePopups = new List<BasePopup>();
    
    public override void DeclareDependencies()
    {
        _dependencyList.Add(_addressableManager);
    }

    public override void Begin()
    {
        Resolve();
    }

    public async void Show<TPopupData>(TPopupData data) where TPopupData : BasePopupData
    {
       var popupGO = await _addressableManager.LoadAsset<GameObject>(data.Name);
       BasePopup<TPopupData> popup = Instantiate(popupGO, PopupCanvas.transform).GetComponent<BasePopup<TPopupData>>();
       popup.PopupData = data;
       _activePopups.Add(popup);
    }
    
    public void Hide(string name)
    {
        for (int i = 0; i < _activePopups.Count; i++)
        {
            var popup = _activePopups[i];
            if (popup.GetPopupData().Name == name)
            {
                _activePopups.RemoveAt(i);
                DestroyImmediate(popup.gameObject);
            }
        }
    }
}