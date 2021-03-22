using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIStructure<T>
{
    void Enable(T data);
    void Disable(T data);
}
