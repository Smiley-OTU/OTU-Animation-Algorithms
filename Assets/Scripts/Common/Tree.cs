using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TreeVisitor<T>(T nodeData);

public class Tree<T>
{
    private T data, parent;
    private LinkedList<Tree<T>> children;

    public Tree(T data)
    {
        this.data = data;
        children = new LinkedList<Tree<T>>();
    }

    public Tree(T data, T parent)
    {
        this.data = data;
        this.parent = parent;
        children = new LinkedList<Tree<T>>();
    }

    public T Data()
    {
        return data;
    }

    public T Parent()
    {
        return parent;
    }

    public void AddChild(T data)
    {
        children.AddLast(new Tree<T>(data, Data()));
    }

    public Tree<T> GetChild(int i)
    {
        foreach (Tree<T> n in children)
            if (--i == 0)
                return n;
        return null;
    }

    public bool RemoveChild(Tree<T> node)
    {
        // This orphans all children of the child being removed...
        return children.Remove(node);
    }

    public void Traverse(Tree<T> node, TreeVisitor<T> visitor)
    {
        visitor(node.data);
        foreach (Tree<T> child in node.children)
            Traverse(child, visitor);
    }
}