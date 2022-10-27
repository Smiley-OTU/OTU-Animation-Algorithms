using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hierarchy : MonoBehaviour
{
    [SerializeField]  private Transform editorParent;
    [SerializeField]  private List<Transform> editorChildren;

    private Hierarchy parent;
    private List<Hierarchy> children = new List<Hierarchy>();

    private Vector3 localTranslation;
    private Vector3 localRotation;
    private Vector3 localScale;

    void Start()
    {
        parent = editorParent != null ? editorParent.GetComponent<Hierarchy>() : null;
        for (int i = 0; i < editorChildren.Count; i++)
        {
            Hierarchy child = editorChildren[i].GetComponent<Hierarchy>();
            if (child)
                children.Add(child);
            else
                Debug.Log("Runtime mis-match: Hierarchy does not exist within editor child");
        }
    }

    void Update()
    {
        
    }

    public Matrix4x4 World()
    {
        transform.localPosition = localTranslation;
        transform.localRotation = Quaternion.Euler(localRotation);
        transform.localScale = localScale;

        if (parent != null)
        {
            return parent.World() * transform.localToWorldMatrix;
        }

        return transform.localToWorldMatrix;
    }

    public void TranslateLocal(Vector3 translation)
    {
        localTranslation = translation;
    }

    public void RotateLocal(Vector3 eulers)
    {
        localRotation = eulers;
    }

    public void ScaleLocal(Vector3 scale)
    {
        localScale = scale;
    }
}
