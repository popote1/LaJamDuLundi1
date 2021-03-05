using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScripte : MonoBehaviour
{
    public GameObject Particule;
    public GameObject Decal;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.forward = rb.velocity.normalized;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag("Enemy"))
        {
            Destroy(Instantiate(Particule, other.contacts[0].point, Quaternion.identity),5);
            Instantiate(Decal, other.contacts[0].point+Vector3.up, Quaternion.Euler(new Vector3(90,0,0)));
            Destroy(gameObject);
            
            
        }
    }
}
