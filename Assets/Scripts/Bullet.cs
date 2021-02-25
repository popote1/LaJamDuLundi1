using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject ExplosionEffect;
    public float ExplosionRadius = 10;
    public float ExplosionForce = 100;

    private void OnCollisionEnter(Collision other)
    {
        Instantiate(ExplosionEffect, transform.position, Quaternion.LookRotation(other.GetContact(0).normal));
        var colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach (var c in colliders)
        {
            var r = c.GetComponent<Rigidbody>();
            Vector3 force = (c.transform.position - transform.position).normalized * ExplosionForce;
            if (r != null)
            {
                r.AddForce(force, ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
    }
}
