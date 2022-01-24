using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DependencyResolver : MonoBehaviour
{
    [Inject] private RootManager _rootManager;
    [Inject] private SignalBus _signalBus;

    private Dictionary<Manager, Node<Manager>> _managerToGraphNode = new Dictionary<Manager, Node<Manager>>();
    private List<Manager> _managers = new List<Manager>();

    private void Start()
    {
        _signalBus.Subscribe<ManagerResolvedEvent>(OnManagerResolvedEvent);
        
        if (CheckDependencyCycle())
        {
            Debug.LogWarning("Cyclic dependency found exiting.");
            Application.Quit();
        }
        else
        {
            _signalBus.Fire<DependencyCycleCheckCompleteEvent>();
        }
    }

    public void Register(Manager manager)
    {
        _managers.Add(manager);
    }

    private bool CheckDependencyCycle()
    {
        Node<Manager> root = new Node<Manager>(_rootManager);
        _managerToGraphNode.Add(_rootManager, root);

        //disconnected graph to connected graph
        for (int i = 0; i < _managers.Count; i++)
        {
            var manager = _managers[i];
            var managerNode = new Node<Manager>(_managers[i]);

            _managerToGraphNode.Add(manager, managerNode);

            root.AddEdge(managerNode);
        }

        //add dependency edges
        for (int i = 0; i < _managers.Count; i++)
        {
            var manager = _managers[i];
            var dependencies = manager.GetDependencyList();
            for (int k = 0; k < dependencies.Count; k++)
            {
                var dependency = dependencies[k];
                _managerToGraphNode[manager].Edges.Add(_managerToGraphNode[dependency]);
            }
        }

        return IsDependencyGraphCyclic(root);
    }

    private bool IsDependencyGraphCyclic(Node<Manager> root)
    {
        Dictionary<Node<Manager>, bool> stack = new Dictionary<Node<Manager>, bool>();
        Dictionary<Node<Manager>, bool> visited = new Dictionary<Node<Manager>, bool>();

        stack.Add(_managerToGraphNode[_rootManager], false);
        visited.Add(_managerToGraphNode[_rootManager], false);

        for (int i = 0; i < _managers.Count; i++)
        {
            stack.Add(_managerToGraphNode[_managers[i]], false);
            visited.Add(_managerToGraphNode[_managers[i]], false);
        }

        return IsDependencyGraphCyclicRecursive(root, stack, visited);
    }

    private bool IsDependencyGraphCyclicRecursive(Node<Manager> node, Dictionary<Node<Manager>, bool> stack,
        Dictionary<Node<Manager>, bool> visited)
    {
        //we already see this on this recursion stack so there must be 
        if (stack[node])
        {
            Debug.LogWarning("Cyclic dependency at manager : " + node.Value.gameObject.name);
            return true;
        }

        //already visited this node in another recursion stack so return false immediately
        if (visited[node])
            return false;

        //set the flags
        visited[node] = true;
        stack[node] = true;

        //check every child recursively
        foreach (Node<Manager> child in node.Edges)
            if (IsDependencyGraphCyclicRecursive(child, stack, visited))
                return true;

        //end of this recursion tree remove this node from the current recursion stack
        stack[node] = false;

        return false;
    }

    private int _resolvedCount = 0;
    private void OnManagerResolvedEvent(ManagerResolvedEvent data)
    {
        _resolvedCount++;
        if (_resolvedCount == _managers.Count)
        {
            Debug.LogWarning("All managers up and ready");
            return;
        }

        if (_resolvedCount > _managers.Count)
        {
            Debug.LogWarning("A manager resolved multiple times");
        }
    }
}