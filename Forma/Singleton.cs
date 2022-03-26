using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<T>();

            return instance;
        }
    }
    static T instance;
}
