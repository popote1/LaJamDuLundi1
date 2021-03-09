using UnityEngine;

public class GroundSetter : MonoBehaviour
{
    public Vector3 Offset;
    void Start()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out var hit))
        {
            transform.position = hit.point + Offset;
            Destroy(this);
        }
    }

   
}
