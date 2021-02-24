using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FoodActivator : MonoBehaviour
{
   public UnityEvent OnTutch ;

   private Rigidbody rb;

   private void Awake()
   {
      rb = GetComponent<Rigidbody>();
   }

   private void OnCollisionEnter(Collision other)
   {
      if (other.collider.CompareTag("Food"))
      {
         rb.constraints = RigidbodyConstraints.None;
         OnTutch.Invoke();
         Destroy(this);
      }
   }
}
