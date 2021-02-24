using System;
using UME;
using Unity.Mathematics;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurretAI : MonoBehaviour
{
    public Transform TurretBase;
    public Transform TurretCannon;
    public Transform ShootTransform;
    public GameObject Bullet;

    [Header("If Is Laser")] public Laser laser;

    public Transform shootTarget;
    public Vector3 CurrentTarget;

    public float ShootForce = 10f;
    public float ShootForceRandomness = 1f;

    [Range(0,1)]public float RotateSpeed = 0.2f;

    public bool IsLaser;


    //Ballistic
    private float BulletMass;
        
    void Start() => SetParameters();

    void Update()
    {
       
        if(shootTarget != null) OrientCannon(shootTarget.position);
        if(Input.GetKeyDown(KeyCode.Space)) Shoot();
    }

    [ContextMenu("Shoot")]
    public void Shoot()
    {
        if (shootTarget == null) return;
        var bullet = Instantiate(Bullet, ShootTransform.position, ShootTransform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(ShootTransform.forward * (ShootForce + Random.Range(-ShootForceRandomness, ShootForceRandomness)), ForceMode.Impulse);
    }

    private void OrientCannon(Vector3 target)
    {
        // var direction =  (target - TurretBase.position).normalized;
        // TurretBase.rotation = Quaternion.Slerp(TurretBasse.rotation, Quaternion.Euler(direction), RotateSpeed);
        CurrentTarget = Vector3.Lerp(CurrentTarget, shootTarget.position, 0.05f);
        TurretBase.LookAt(new Vector3(CurrentTarget.x, TurretBase.position.y, CurrentTarget.z));
        if (IsLaser)
            TurretCannon.LookAt(CurrentTarget);
        else
            TurretCannon.rotation = Quaternion.Euler(new float3(ComputeShootAngle(ShootForce), TurretBase.transform.rotation.eulerAngles.asfloat().yz));
        
    }

    void SetParameters()
    {
        BulletMass = Bullet.GetComponent<Rigidbody>().mass;
        CurrentTarget = ShootTransform.forward * 3 + ShootTransform.position; // Start target position for smoother lerp
    }
    
    float ComputeShootDistance() => (shootTarget.position - ShootTransform.position).magnitude;
    float ComputeInitialVelocity(float impulseForce) => impulseForce / BulletMass;

    private float G => Physics.gravity.y;
    
    float ComputeShootAngle(float impulseForce)
    {
        float d = ComputeShootDistance(); // works !
        float v = ComputeInitialVelocity(impulseForce);
        float heightDelta = shootTarget.position.y - ShootTransform.position.y;
        float heightDeltaAngle = (heightDelta / d).tan().atan().degrees();

        float angle =  ((G * d / v.sqr()).asin().degrees() / 2).clamp(-360,360);
        Debug.Log(BulletMass);
        return angle - heightDeltaAngle;
    }
}