using UnityEngine;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject _managerParent;
    
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
    }
}