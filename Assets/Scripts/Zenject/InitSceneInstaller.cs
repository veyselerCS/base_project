using UnityEngine;
using Zenject;

public class InitSceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject _managerParent;
    
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        
        //Manager Bindings
        Container.Bind<RootManager>().FromNewComponentOnNewGameObject ().UnderTransform(_managerParent.transform).AsSingle().NonLazy();
        Container.Bind<PopupManager>().FromNewComponentOnNewGameObject ().UnderTransform(_managerParent.transform).AsSingle().NonLazy();
        Container.Bind<SceneChangeManager>().FromNewComponentOnNewGameObject ().UnderTransform(_managerParent.transform).AsSingle().NonLazy();
        Container.Bind<LoadingManager>().FromNewComponentOnNewGameObject().UnderTransform(_managerParent.transform).AsSingle().NonLazy();
        Container.Bind<AddressableManager>().FromNewComponentOnNewGameObject().UnderTransform(_managerParent.transform).AsSingle().NonLazy();
        Container.Bind<DependencyResolver>().FromNewComponentOnNewGameObject().UnderTransform(_managerParent.transform).AsSingle().NonLazy();
        
        //Signals
        Container.DeclareSignal<LoadingFinishEvent>();
        Container.DeclareSignal<DependencyCycleCheckCompleteEvent>();
        Container.DeclareSignal<ManagerResolvedEvent>().OptionalSubscriber();
    }
}