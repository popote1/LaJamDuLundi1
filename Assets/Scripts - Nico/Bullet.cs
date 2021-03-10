using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject ExplosionEffect;
    public float ExplosionRadius = 5;
    public float ExplosionForce = 10;


    
    public GameObject Decal;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        var velocity = rb.velocity;
        transform.forward = velocity != Vector3.zero ? velocity.normalized : Vector3.forward;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Name = " + other.gameObject);
        Destroy(Instantiate(ExplosionEffect, other.contacts[0].point,Quaternion.LookRotation(other.GetContact(0).normal)), 5);
        Instantiate(Decal, other.contacts[0].point + Vector3.up, Quaternion.Euler(new Vector3(90,0,0)));
        
        if (!other.transform.CompareTag("Enemy"))
        {
            var colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
            foreach (var c in colliders)
            {
                var r = c.GetComponent<Rigidbody>();
                if (r != null) {
                    var force = (c.transform.position - transform.position).normalized * ExplosionForce;
                    r.AddForce(force, ForceMode.Impulse);
                }
            }
            Destroy(gameObject);
        }
    }
    
    // private void OnCollisionEnter(Collision other)
    // {
    //     Instantiate(ExplosionEffect, transform.position, Quaternion.LookRotation(other.GetContact(0).normal));
    //     var colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
    //     foreach (var c in colliders)
    //     {
    //         var r = c.GetComponent<Rigidbody>();
    //         Vector3 force = (c.transform.position - transform.position).normalized * ExplosionForce;
    //         if (r != null)
    //         {
    //             r.AddForce(force, ForceMode.Impulse);
    //         }
    //     }
    //     Destroy(gameObject);
    // }
}
