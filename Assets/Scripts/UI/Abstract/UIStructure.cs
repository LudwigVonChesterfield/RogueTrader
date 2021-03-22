using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIStructure<T> : MonoBehaviour, IUIStructure<T>
{
    private bool visible = false;
    private CanvasGroup my_group;

    void Awake()
    {
        my_group = GetComponent<CanvasGroup>();
        OnAwake();
    }

    protected virtual void OnAwake()
    {
    }

    public void Enable(T data)
    {
        visible = true;

        my_group.alpha = 1.0f;
        my_group.blocksRaycasts = true;

        OnEnable(data);
    }

    public void Disable(T data)
    {
        visible = false;

        my_group.alpha = 0.0f;
        my_group.blocksRaycasts = false;

        OnDisable(data);
    }

    protected virtual void OnEnable(T data)
    {
    }

    protected virtual void OnDisable(T data)
    {
    }
}
