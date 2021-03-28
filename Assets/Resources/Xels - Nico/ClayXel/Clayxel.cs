using UnityEngine;

[ExecuteInEditMode]
public class Clayxel : MonoBehaviour
{
    
    public MeshFilter meshFilter;
    public GameObject Xel;
    private static readonly int Normal = Shader.PropertyToID("_NormalVec");

    private void OnEnable()
    {
        meshFilter = GetComponent<MeshFilter>();
        SpawnXels();
    }

    private void OnValidate()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SpawnXels();
        }
        if (Input.GetKey(KeyCode.X))
        {
            ClearXels();
        }
    }

    [ContextMenu("Spawn Xels")]
    public void SpawnXels()
    {
        ClearXels();
        var vertices = meshFilter.sharedMesh.vertices;
        var normals = meshFilter.sharedMesh.normals;
        var pos = transform.position;
        // var mat = Xel.GetComponent<MeshRenderer>().material;
        
        for (int i = 0; i < vertices.Length; i++)
        {
            var xel = Instantiate(Xel, vertices[i] + transform.position, Quaternion.identity, transform);
            Material mat = xel.GetComponent<MeshRenderer>().material;
            mat.SetVector(Normal, normals[i]);
        }

        GetComponent<MeshRenderer>().enabled = false;
    }

    [ContextMenu("Clear Xels")]
    public void ClearXels()
    {
        var children = transform.GetChildren();
        children.Remove(transform);
        foreach (var t in children)
        {
            DestroyImmediate(t.gameObject);
        }
        GetComponent<MeshRenderer>().enabled = true;
    }
}