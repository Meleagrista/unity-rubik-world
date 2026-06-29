using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T s_instance;

    public static T Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = Resources.Load<T>(typeof(T).Name);

                if (s_instance == null)
                {
                    throw new System.Exception($"No instance of {typeof(T).Name} found in a Resources folder.");
                }
            }

            return s_instance;
        }
    }
}
