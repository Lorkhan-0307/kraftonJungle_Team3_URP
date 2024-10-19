using UnityEngine;

public class Singleton<T> : ScriptableObject where T : ScriptableObject
{
    private static T Instance;

    public static T instance
    {
        get
        {
            if (Instance == null)
            {
                Instance = Resources.FindObjectsOfTypeAll<T>()[0];
            }
            return Instance;
        }
    }
}
