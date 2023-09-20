using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generic class implementation of Singleton with MonoBehaviour type constraint
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
{
    private static T _instance;
    public static T Instance {
        // Instances must be Read-Only
        get {
            if (_instance == null) {
                // When called, Create GameObject under "Managers" GameObject in hierarchy window
                Transform parentTransform = GameObject.Find("Managers").transform;
                GameObject obj = new GameObject();
                obj.transform.SetParent(parentTransform);

                // Attach script to created GameObject
                _instance = obj.AddComponent<T>();

                // Change object name to class name
                obj.name = typeof(T).ToString();
            }
            return _instance;
        }
        
    }

}