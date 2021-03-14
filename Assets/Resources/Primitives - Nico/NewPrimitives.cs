using UnityEngine;
using UnityEditor;

public class NewPrimitives : MonoBehaviour
{

    // [MenuItem("GameObject/Create Shit")]
    public static void BringMeBackFromTheEdgeOfMadness()
    {
        Resources.Load("Primitives - Nico/Cone.fbx");
        Mesh mesh = Resources.Load<Mesh>("Primitives - Nico/Cone");
        GameObject go = new GameObject("Cone");
        go.AddComponent<MeshFilter>().mesh = mesh;
        go.AddComponent<MeshRenderer>().material = new Material(Resources.Load<Material>("Primitives - Nico/White Default"));
        
        if(Selection.activeTransform != null)
            go.transform.parent = Selection.activeTransform;
    }
}