using UME;
using Unity.Mathematics;
using UnityEngine;
using static UME.UnityMathematicsExtensions;
using Random = UnityEngine.Random;

public class TurretAI : MonoBehaviour
{
    [Header("Transforms")]
    public Transform TurretBaseTransform;
    public Transform TurretCannonTransform;
    public Transform ShootTransform;
    public Transform CannonEndTransform;
    
    [Header("Prefabs")]
    public GameObject BulletPrefab;
    public GameObject ShootParticle;

    [Header("If Is Laser")]
    public Laser Laser;

    [Header("Target Transform")]
    public Transform shootTarget;
    public Vector3 CurrentTarget;

    [Header("Variables")]
    public float ShootForce = 10f;
    public float ShootForceRandomness = 1f;
    [Range(0,1)]public float RotateSpeed = 0.05f;

    public bool IsLaser;
    
    
    // Constants for Ballistic Computation
    private float G => Physics.gravity.y;


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
        Destroy(Instantiate(ShootParticle ,CannonEndTransform.position, Quaternion.identity),5);
        var bullet = Instantiate(BulletPrefab, ShootTransform.position, ShootTransform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(ShootTransform.forward * (ShootForce + Random.Range(-ShootForceRandomness, ShootForceRandomness)), ForceMode.Impulse);
    }

    private void OrientCannon(Vector3 target)
    {
        CurrentTarget = Vector3.Lerp(CurrentTarget, shootTarget.position, RotateSpeed);
        TurretBaseTransform.LookAt(new Vector3(CurrentTarget.x, TurretBaseTransform.position.y, CurrentTarget.z));
        if (IsLaser)
            TurretCannonTransform.LookAt(CurrentTarget);
        else
            TurretCannonTransform.rotation = Quaternion.Euler(new float3(ComputeShootAngle(ShootForce), TurretBaseTransform.transform.rotation.eulerAngles.asfloat().yz));
    }

    void SetParameters()
    {
        BulletMass = BulletPrefab.GetComponent<Rigidbody>().mass;
        CurrentTarget = ShootTransform.forward * 3 + ShootTransform.position; // Start target position for smoother lerp
        if (CannonEndTransform == null) CannonEndTransform = ShootTransform; // Prevents Null Reference
    }
    
    float ComputeShootDistance() => (shootTarget.position - ShootTransform.position).magnitude;
    float ComputeInitialVelocity(float impulseForce) => impulseForce / BulletMass;

    float ComputeShootAngle(float impulseForce)
    {
        float d = ComputeShootDistance();
        float v = ComputeInitialVelocity(impulseForce);
        float heightDelta = shootTarget.position.y - ShootTransform.position.y;
        float heightDeltaAngle = (heightDelta / d).tan().atan().degrees();

        float angle =  ((G * d / v.sqr()).asin().degrees() / 2).clamp(-360,360 * PI);
        return angle - heightDeltaAngle;
    }
}