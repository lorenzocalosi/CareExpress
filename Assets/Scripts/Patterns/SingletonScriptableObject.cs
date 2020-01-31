using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
/// <summary>
/// Abstract class for making reload-proof singletons out of ScriptableObjects
/// Returns the asset created on the editor, or null if there is none
/// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
/// </summary>
/// <typeparam name="T">Singleton type</typeparam>

public abstract class SingletonScriptableObject<T> : SerializedScriptableObject where T : SerializedScriptableObject {
    static T instance = null;
    public static T Instance {
        get {
            if (!instance)
                instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
#if UNITY_EDITOR
            if (!instance) {
                string[] configsGUIDs = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
                if (configsGUIDs.Length > 0) {
                    instance = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(configsGUIDs[0]));
                }
            }
#endif
            return instance;
        }
    }
}

