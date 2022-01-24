using System;
using System.Collections.Generic;using UnityEngine;
using Zenject;

public abstract class Manager : MonoBehaviour, IDisposable
{
    [Inject] private SignalBus _signalBus;
    [Inject] private DependencyResolver _dependencyResolver;
    
    protected List<Manager> _dependencyList = new List<Manager>();
    private bool _resolved;
    public virtual void Awake()
    {
        Init();
        _dependencyResolver.Register(this);
        _signalBus.Subscribe<DependencyCycleCheckCompleteEvent>(OnDependencyCycleCheckComplete);
        _signalBus.Subscribe<ManagerResolvedEvent>(OnManagerResolvedEvent);
    }

    public abstract void Init();

    public abstract void Begin();

    protected void Resolve()
    {
        _resolved = true;
        _signalBus.Fire(new ManagerResolvedEvent(this));
    }

    private void TryResolve()
    {
        if (_dependencyList.IsEmpty())
        {
            _signalBus.TryUnsubscribe<ManagerResolvedEvent>(OnManagerResolvedEvent);
            Begin();
        }
    }
    
    private void OnManagerResolvedEvent(ManagerResolvedEvent data)
    {
        _dependencyList.Remove(data.Manager);

        TryResolve();
    }

    private void OnDependencyCycleCheckComplete(DependencyCycleCheckCompleteEvent data)
    {
        if(_resolved) return;

        TryResolve();
    }

    public List<Manager> GetDependencyList()
    {
        return _dependencyList;
    }
    
    public void Dispose()
    {
        _signalBus.Unsubscribe<DependencyCycleCheckCompleteEvent>(OnDependencyCycleCheckComplete);
    }
}