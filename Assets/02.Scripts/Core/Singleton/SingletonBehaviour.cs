using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    public static T Instance { get; private set; }

    protected virtual bool IsPersistent => false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = (T)this;

        if (IsPersistent)
        {
            DontDestroyOnLoad(gameObject);
        }

        Init();
    }

    private void OnDestroy()
    {
        Cleanup();

        if (Instance == this)
        {
            Instance = null;
        }
    }

    protected virtual void Init() { }
    protected virtual void Cleanup() { }
}