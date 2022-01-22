using Zenject;

public class LoadingFinishEvent
{
    public int value;

    public LoadingFinishEvent(int status)
    {
        value = status;
    }
}