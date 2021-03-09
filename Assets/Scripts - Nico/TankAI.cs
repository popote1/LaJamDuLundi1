using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class TankAI : MonoBehaviour
{
    public NavMeshAgent NA;
    public GameObject DestructionParticle;
    public GameObject SmokeParticle;
    public Transform Destination;
    public List<AudioClip> DestructSounds;
    

    public float Speed = 10;
    public float MinDistanceToShoot = 80;

    public CannonTurretAI TurretAI;

    private float _timer;
    public float shootDelay = 2;

    public bool EnableShoot = true;
    
    void Start()
    {
        SetParameters();
        StartCoroutine(TankCoroutine(5));
    }

    void Update()
    {
        NA.updateUpAxis = false;
        if (Input.GetKey(KeyCode.T) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, Mathf.Infinity);
            TurretAI.shootTarget.position = hit.point;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, Mathf.Infinity);
            GoTo(hit.point);
        }

        if (EnableShoot)
        {
            _timer += Time.deltaTime;
            
            if (TurretAI.shootTarget == null) return;
            if (TurretAI.ComputeShootDistance() > MinDistanceToShoot) return;
            if (_timer <= shootDelay) return;
            
            TurretAI.Shoot();
            _timer = 0;
        }
    }

    void GoTo(Vector3 position) => NA.SetDestination(position);

    void SetParameters()
    {
        NA.speed = Speed;
        TurretAI = GetComponent<CannonTurretAI>();
    }
    
    public void Dead()
    {
        // Destroy(NA);
        NA.enabled = false;
        var pos = transform.position;
        
        AudioSource.PlayClipAtPoint(DestructSounds[Random.Range(0, DestructSounds.Count)],pos,0.5f);
        Destroy(Instantiate(DestructionParticle, pos, quaternion.identity),5);
        Instantiate(SmokeParticle, pos, Quaternion.identity, transform);
        GetComponent<Rigidbody>().AddForce(Vector3.up*10,ForceMode.Impulse);
        GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0.2f,1),Random.Range(0.5f,1),Random.Range(0.3f,1)*1000),ForceMode.Impulse);
        Destroy(this);
    }

    IEnumerator TankCoroutine(float delay = 5)
    {
        NA.SetDestination(Destination.position);
        yield return new WaitForSeconds(delay);
    }
}
