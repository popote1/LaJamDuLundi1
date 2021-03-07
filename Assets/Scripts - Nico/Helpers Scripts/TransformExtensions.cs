using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TransformExtensions
{
    public static List<Transform> GetFamily(this Transform tr)
    {
        var trs = new List<Transform> {tr};
        trs.AddRange(tr.GetComponentsInChildren<Transform>());
        return trs;
    }
    public static List<Transform> GetChildren(this Transform tr) => tr.GetComponentsInChildren<Transform>().ToList();

    public static List<GameObject> GetChildrenGameObjects(this Transform tr) => tr.GetChildren().Select(t =>t.gameObject).ToList();

    /// <summary>
    /// Changes Layer For This Transform And Every Children
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="layer"></param>
    public static void SetFamilyLayer(this Transform transform, int layer)
    {
        foreach (var t in transform.GetFamily())
            t.gameObject.layer = layer;
    }
}