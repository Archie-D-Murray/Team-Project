using UnityEngine;

public static class GameObjectExtensions {
    public static bool HasComponent<T>(this GameObject gameObject) where T : Component {
        return gameObject.GetComponent<T>() ? true : false;
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
        T component = gameObject.GetComponent<T>();
        return component.OrNull() == null ? component : gameObject.AddComponent<T>();
    }
    

    /// <summary>
    /// Allows null coalescing on Unity Objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T OrNull<T>(this T component) where T : Component {
        return component ? component : null;
    }
}