using System;
using System.Collections.Generic;using UnityEngine;
using Zenject;

public abstract class Manager : MonoBehaviour, IDisposable
{
    [Inject] private SignalBus _signalBus;
    
    protected List<Manager> _dependencyList = new List<Manager>();
    private bool _resolved;
    private void Awake()
    {
        Init();
        
        if (_dependencyList.IsEmpty())
        {
            _resolved = true;
            Begin();
            return;
        }
        
        _signalBus.Subscribe<ManagerResolvedEvent>(OnManagerResolvedEvent);

    }

    private void Start()
    {
        //Managers with empty dependencies can resolve at this point since all the dependency lists are filled
        if(_resolved)
        {
            _signalBus.Fire(new ManagerResolvedEvent(this));
        }
    }

    public abstract void Init();

    public abstract void Begin();

    protected void Resolve()
    {
        if(_resolved) return;
        
        _signalBus.TryUnsubscribe<ManagerResolvedEvent>(OnManagerResolvedEvent);
        _signalBus.Fire(new ManagerResolvedEvent(this));
    }
    
    private void OnManagerResolvedEvent(ManagerResolvedEvent data)
    {
        _dependencyList.Remove(data.Manager);

        if (_dependencyList.IsEmpty())
        {
            Begin();
        }
    }
    
    public void Dispose()
    {
    }
}