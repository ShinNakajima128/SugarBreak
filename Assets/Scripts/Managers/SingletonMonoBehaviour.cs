using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    Debug.LogError(typeof(T) + "Ç™ÉVÅ[ÉìÇ…ë∂ç›ÇµÇ‹ÇπÇÒÅB");
                }
            }

            return instance;
        }
    }

}