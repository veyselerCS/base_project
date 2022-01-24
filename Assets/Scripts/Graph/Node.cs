using System.Collections.Generic;

public class Node<T>
{
    public T Value;
    public List<Node<T>> Edges = new List<Node<T>>();
    
    public Node(T value)
    {
        Value = value;
    }

    public void AddEdge(Node<T> edge)
    {
        Edges.Add(edge);
    }
}