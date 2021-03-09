using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject Particle;
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
            Destroy(Instantiate(Particle, other.contacts[0].point, Quaternion.identity),5);
            Instantiate(Decal, other.contacts[0].point + Vector3.up, Quaternion.Euler(new Vector3(90,0,0)));
            Destroy(gameObject);
        }
    }
}
