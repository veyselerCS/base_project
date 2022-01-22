using Zenject;

public class LoadingPopup : BasePopup<LoadingPopup.Data>
{
    [Inject] private LoadingManager _loadingManager;
    
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