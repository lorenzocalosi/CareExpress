using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:MonoBehaviour {

    private static T instance;
    public static T Instance {
        get {           
            return instance;
        }
    }

    public void Awake() {
        if (!instance) {
            instance = this as T;
            DontDestroyOnLoad(this);
        }else {
            Destroy(this.gameObject);
        }
    }

}
