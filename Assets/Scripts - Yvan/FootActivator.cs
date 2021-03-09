using UnityEngine;
using UnityEngine.Events;

public class FootActivator : MonoBehaviour
{
   public UnityEvent OnTouch ;

   private Rigidbody rb;

   private void Awake()
   {
      rb = GetComponent<Rigidbody>();
   }

   private void OnCollisionEnter(Collision other)
   {
      if (other.collider.CompareTag("Foot"))
      {
         rb.constraints = RigidbodyConstraints.None;
         OnTouch.Invoke();
         Destroy(this);
      }
   }
}
