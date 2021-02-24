using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSetter : MonoBehaviour
{
    public Vector3 Offset;
    void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            transform.position = hit.point + Offset;
            Destroy(this);
        }
    }

   
}
