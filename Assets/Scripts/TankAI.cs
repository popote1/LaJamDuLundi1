
using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class TankAI : MonoBehaviour
{
    public NavMeshAgent NA;
    public Transform TurretBase;
    public Transform TurretCannon;
    public Transform ShootTransform;
    public GameObject Bullet;
    public Transform Destination;
    public GameObject DestructePaticule;
    public Transform shootTarget;
    public List<AudioClip> DestructSounds;
    
    public float ShootForce = 10;
    public float Speed = 10;

    [Range(0,1)]public float RotateSpeed = 0.2f;
        
    void Start()
    {
        SetParameters();
    }

    void Update()
    {
        NA.SetDestination(Destination.position);
        NA.updateUpAxis =false;
        OrientCannon(shootTarget.position);
        if(Input.GetKeyDown(KeyCode.Space)) Shoot();

        if (Input.GetKey(KeyCode.T) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, Mathf.Infinity);
            shootTarget.position = hit.point;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, Mathf.Infinity);
            GoTo(hit.point);
        }
    }

    [ContextMenu("Shoot")]
    void Shoot()
    {
        GameObject belette = Instantiate(Bullet, ShootTransform.position, ShootTransform.rotation);
        belette.GetComponent<Rigidbody>().AddForce(ShootTransform.forward * ShootForce, ForceMode.Impulse);
    }

    private void OrientCannon(Vector3 target)
    {
        // var direction =  (target - TurretBase.position).normalized;
        // TurretBase.rotation = Quaternion.Slerp(TurretBase.rotation, Quaternion.Euler(direction), RotateSpeed);
        TurretBase.LookAt(new Vector3(target.x, TurretBase.position.y, target.z));
        TurretCannon.LookAt(target);
    }
    

    void GoTo(Vector3 position) => NA.SetDestination(position);

    void GoToShootTarget() => NA.SetDestination(shootTarget.position);

    void SetParameters()
    {
        NA.speed = Speed;
    }
    [ContextMenu("Death")]
    public void Dead()
    {
        Destroy(NA);
        AudioSource.PlayClipAtPoint(DestructSounds[Random.Range(0, DestructSounds.Count)],transform.position,0.5f);
        Destroy(Instantiate(DestructePaticule, ShootTransform.position, quaternion.identity),5);
        GetComponent<Rigidbody>().AddForce(Vector3.up*10,ForceMode.Impulse);
        GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0.2f,1),Random.Range(0.5f,1),Random.Range(0.3f,1)*1000),ForceMode.Impulse);
        Destroy(this);
    }
}
