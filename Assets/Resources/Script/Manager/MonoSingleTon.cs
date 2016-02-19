using UnityEngine;
using System.Collections;

public abstract class MonoSingleTon<T> : MonoBehaviour where T : MonoSingleTon<T> 
{
    public static T Instance = null;

    protected virtual void Awake()
    {
        Instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        Instance = null;
    }
}
