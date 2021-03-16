using System;
using UnityEngine;
using UnityEditor;
using static Utils;
using Object = UnityEngine.Object;

public class NewPrimitives : MonoBehaviour
{
    private const string cone = "Cone";
    private const string smoothCube = "Smooth Cube";
    private const string smoothIcosphere = "Smooth Icosphere";
    private const string smoothCylinder = "Smoth Cylinder";
    private const string torus = "Torus";


    [MenuItem("GameObject/Create Cone")]
    public static void SpawnCone()
    {
        SpawnBaseObject(cone).AddMeshCollider().SetConvex();
    }
    [MenuItem("GameObject/Create Smooth Cube")]
    public static void SpawnSmoothCube()
    {
        SpawnBaseObject(smoothCube).AddMeshCollider().SetConvex();
    }
    [MenuItem("GameObject/Create Smooth Icosphere")]
    public static void SpawnSmoothIcosphere()
    {
        SpawnBaseObject(smoothIcosphere).AddMeshCollider().SetConvex();
    }
    [MenuItem("GameObject/Create Smooth Cylinder")]
    public static void SpawnSmoothCylinder()
    {
        SpawnBaseObject(smoothCylinder).AddMeshCollider().SetConvex();
    }
    [MenuItem("GameObject/Create Torus")]
    public static void SpawnTorus()
    {
        SpawnBaseObject(torus).AddMeshCollider();
    }

    
    private static GameObject SpawnBaseObject(string name)
    {
        return NewGameObject(name).AddMesh(name).AddMaterial("White Default").SetParent();
    }


    public static Type GetColliderType(ColliderType colliderType)
    {
        return colliderType switch
        {
            ColliderType.Sphere => typeof(SphereCollider),
            ColliderType.Box => typeof(BoxCollider),
            ColliderType.Mesh => typeof(MeshCollider),
            ColliderType.Capsule => typeof(CapsuleCollider),
            _ => throw new ArgumentOutOfRangeException(nameof(colliderType), colliderType, null)
        };
    }


    public enum ColliderType
    {
        Sphere,
        Box,
        Mesh,
        Capsule
    };
}

public static class Utils
{


    public static GameObject AddMesh(this GameObject go, string name)
    {
        go.AddComponent<MeshFilter>().mesh = Load<Mesh>(name);//LoadMesh(name);
        return go;
    }
    
    public static GameObject AddMaterial(this GameObject go, string name)
    {
        //Check if Renderer is Already active
        go.AddComponent<MeshRenderer>().material = Load<Material>(name);
        return go;
    }
    
    public static GameObject AddMeshCollider(this GameObject go) => go.Add<MeshCollider>();
    public static GameObject AddBoxCollider(this GameObject go) => go.Add<BoxCollider>();
    public static GameObject AddSphereCollider(this GameObject go) => go.Add<SphereCollider>();
    public static GameObject AddCapsuleCollider(this GameObject go) => go.Add<SphereCollider>();
    
    public static GameObject SetConvex(this GameObject go)
    {
        var g = go.GetComponent<MeshCollider>();
        if(g != null) g.convex = true;
        return go;
    }

    
    // Shortcut For Resources.Load("Primitives" + name)
    private static T Load<T>(string name) where T : Object
    {
        return Resources.Load<T>("Primitives/" + name);
    }
    
    //Creates A New GameObject
    public static GameObject NewGameObject(string name)
    {
        return new GameObject(name);
    }
    
    //adds component and returns the GameObject
    public static GameObject Add<T>(this GameObject go) where T : Component
    {
        go.AddComponent<T>();
        return go;
    }
    
    //Sets The gameObject's parent according to the selection
    public static GameObject SetParent(this GameObject go)
    {
        if(Selection.activeTransform != null)
            go.transform.parent = Selection.activeTransform;
        
        return go;
    }
}