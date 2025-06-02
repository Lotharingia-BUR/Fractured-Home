using UnityEngine;

public abstract class Manager<T> : Singleton<T> where T : Manager<T>
{
    protected override void Initialize()
    {
        base.Initialize();

        DontDestroyOnLoad(gameObject);
    }
}
