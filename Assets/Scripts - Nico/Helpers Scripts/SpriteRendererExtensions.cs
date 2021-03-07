using UnityEngine;

public static class SpriteRendererExtensions
{
    public static void SetMaterial(this SpriteRenderer sr, Material m) => sr.material = m;
}