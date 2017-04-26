using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 单例管理类
public class Singleton : MonoBehaviour {

    private static GameObject mContainer = null;
    private static string mName = "Singleton";
    private static Dictionary<string, object> mSingletonMap = new Dictionary<string, object>();
    private static bool mIsDestroying = false;

    public static bool IsDestroying {
        get {
            return mIsDestroying;
        }
    }

    public static bool IsCreatedInstance(string name) {
        if (mContainer == null) {
            return false;
        }
        if (mSingletonMap != null && mSingletonMap.ContainsKey(name)) {
            return true;
        }
        return false;

    }

    public static object getInstance(string name) {
        if (mContainer == null) {
            Debug.Log("Create Singleton.");
            mContainer = new GameObject();
            mContainer.name = mName;
            mContainer.AddComponent(typeof(Singleton));
        }
        if (!mSingletonMap.ContainsKey(name)) {
            if (System.Type.GetType(name) != null)
            {
                mSingletonMap.Add(name, mContainer.AddComponent(System.Type.GetType(name)));
            }
            else {
                Debug.LogWarning("Singleton Type Error!(" + name + ")");
            }
        }
        return mSingletonMap[name];
    }


    public static void RemoveInstance(string name) {
        if (mContainer != null && mSingletonMap.ContainsKey(name)) {
            UnityEngine.Object.Destroy((UnityEngine.Object)(mSingletonMap[name]));
            mSingletonMap.Remove(name);
            Debug.LogWarning("Singleton Remove!(" + name + ")");
        }
    }

    public void Awake() {
        Debug.Log("Awake Singleton.");
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
        Debug.Log("Start Singleton.");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnApplicationQuit() {
        Debug.Log("Destroy Singleton.");
        if (mContainer != null) {
            GameObject.Destroy(mContainer);
            mContainer = null;
            mIsDestroying = true;
        }

    }
}
