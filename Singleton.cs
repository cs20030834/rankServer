using UnityEngine;

public class Singleton<T>: MonoBehaviour where T : MonoBehaviour {

    protected static T instance = null;

    public static T GetInstance {
        get {
            if (instance == null) {
                instance = FindObjectOfType (typeof (T)) as T;
                DontDestroyOnLoad (instance.gameObject);
                if (instance == null) {
                    Debug.Log ("Nothing" + instance.ToString ());
                    return null;
                }
            } 
            return instance;
        }
    }

    public bool isInstanceNull () {
        if (instance == null)
            return true;
        else
            return false;
    }
}