using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    //Public interface to the singleton instance
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                //Set the instance
                _instance = FindAnyObjectByType<T>();
                _instance?.Initialize();
            }
            return _instance;
        }
    }

    //The singleton instance itself
    protected static T _instance;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            //Set the instance
            _instance = (T)this;
            Initialize();
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"Cannot have multiple {this.GetType().Name} objects in one scene. Deleting newest object");
            Destroy(gameObject);
        }
    }

    ///<summary>
    ///Override for initialization instead of using Start() or Awake()
    ///</summary>
    protected virtual void Initialize()
    {

    }
}
