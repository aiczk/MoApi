using System.Linq;
using UnityEngine;

namespace _Script.Application.Utility.Base
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour 
    {
        private static T instance;
    
        public static T Instance 
        {
            get 
            {
                if (instance != null) 
                    return instance;
            
                instance = FindObjectOfType<T>();

                if (FindObjectsOfType<T>().Length > 1)
                    return instance;

                if (instance != null) 
                    return instance;
            
                var singleton = new GameObject
                {
                    name = $"{typeof(T)} (singleton)"
                };
                
                instance = singleton.AddComponent<T>();
                DontDestroyOnLoad(singleton);
            
                return instance;
            }
        
            private set => instance = value;
        }

        private void OnDestroy () => Instance = null;
    }

    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject 
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (!instance)
                    instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                return instance;
            }
        }
    }
}