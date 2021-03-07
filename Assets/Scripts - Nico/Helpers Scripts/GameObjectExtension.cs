using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Helpers
{
    public static class GameObjectExtension // Testing
    {
        
        /// <summary>
        /// Another way of writing Instantiate(GameObject gameObject, ....)
        /// Suitable for Null Propagator --> gameObject?.Instantiate(position, rotation)
        /// </summary>
        /// <param name="gameObject">The GameObject to instantiate</param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public static void Instantiate(this GameObject gameObject, Vector3 position, Quaternion rotation)
        {
            Object.Instantiate(gameObject, position, rotation);
        }
        
        public static void Instantiate(this GameObject go, Transform transform)
        {
            Object.Instantiate(go, transform);
        }

        /// <summary>
        /// Extension Method equivalent to --> if(gameObject != null) { gameObject.RandomMethod() }   else { Debug.Log("message") }
        /// </summary>
        /// <param name="message">Debug.Log message</param>
        /// <returns></returns>
        [Pure, ContractAnnotation("null => null")]
        public static T NullMessage<T>([CanBeNull] this T unityObject, string message = "") where T : Object
        {
            if (unityObject != null) return unityObject;
            Debug.Log(message);
            return null;
        }
        
        /// <summary>
        /// Extension Method equivalent to --> Enables use of Null Propagation
        /// </summary>
        /// <param name="message">Debug.Log message</param>
        /// <returns></returns>
        [Pure, ContractAnnotation("null => null")]
        public static T Null<T>([CanBeNull] this T unityObject) where T : Object
        {
            return unityObject != null ? unityObject : null;
        }
        
        
        public static void NullSetActive(this GameObject gameObject, bool state)
        {
            gameObject.Null()?.SetActive(state);
        }
        public static void NullSetActiveAll(this IEnumerable<GameObject> gameObjects, bool state)
        {
            foreach (var g in gameObjects)
            {
                g.Null()?.SetActive(state);
            }
        }
    }
}