using System;
using UnityEngine;

public abstract class BasePopup : MonoBehaviour, IDisposable
{
    public BasePopupData PopupData;


    public BasePopup(BasePopupData data)
    {
        PopupData = data;
    }
    
    public void Dispose()
    {
    }
    
    protected BasePopup()
    {
        Debug.LogWarning("Popup created without it's data");
    }
}

public class BasePopup<TPopupData> : BasePopup where TPopupData : BasePopupData
{
    public BasePopup(TPopupData data) : base(data)
    {
    }

    protected BasePopup()
    {
        
    }
}