using UnityEngine;

public class XelPropertyManager : MonoBehaviour
{

    [Range(0,1)] public float Alpha = 0.5f;
    [Range(0,4)] public float Size = 0.5f;
    [Range(0,1)] public float DepthOffset = 0.5f;
    [Range(0,1)] public Color Color = Color.green;
    
    private void OnValidate()
    {
        SetParameters();
    }

    void SetParameters()
    {
        Shader.SetGlobalFloat("_XelAlpha", Alpha);
        Shader.SetGlobalFloat("_XelSize", Size);
        Shader.SetGlobalFloat("_XelDepthOffset", DepthOffset);
        Shader.SetGlobalColor("_XelColor", Color);
    }

    [ContextMenu("Update Xels")]
    void UpdateXels()
    {
        foreach (var x in FindObjectsOfType<Clayxel>())
        {
            x.SpawnXels();
        }
    }
    
    [ContextMenu("Clear Xels")]
    void ClearXels()
    {
        foreach (var x in FindObjectsOfType<Clayxel>())
        {
            x.ClearXels();
        }
    }
}