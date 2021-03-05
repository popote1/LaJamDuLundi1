using UME;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CannonTurretAI : Turret
{

    [Header("Prefabs")]
    public GameObject BulletPrefab;
    public GameObject ShootParticle;
    

    [Header("Variables")]
    public float ShootForce = 10f;
    public float ShootForceRandomness = 1f;

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
    public override void Shoot()
    {
        Debug.Log("Shooted");
        if (shootTarget == null) return;
        Destroy(Instantiate(ShootParticle ,CannonEndTransform.position, Quaternion.identity),5);
        var bullet = Instantiate(BulletPrefab, ShootTransform.position, ShootTransform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(ShootTransform.forward * (ShootForce + Random.Range(-ShootForceRandomness, ShootForceRandomness)), ForceMode.Impulse);
    }

    protected override void OrientCannon(Vector3 target)
    {
        CurrentTarget = Vector3.Lerp(CurrentTarget, shootTarget.position, RotateSpeed);
        TurretBaseTransform.LookAt(new Vector3(CurrentTarget.x, TurretBaseTransform.position.y, CurrentTarget.z));
        TurretCannonTransform.rotation = Quaternion.Euler(new float3(ComputeShootAngle(ShootForce), TurretBaseTransform.transform.rotation.eulerAngles.asfloat().yz));
    }

    protected override void SetParameters()
    {
        base.SetParameters();
        BulletMass = BulletPrefab.GetComponent<Rigidbody>().mass;
    }
    float ComputeInitialVelocity(float impulseForce) => impulseForce / BulletMass;

    float ComputeShootAngle(float impulseForce)
    {
        float d = ComputeShootDistance();
        float v = ComputeInitialVelocity(impulseForce);
        float heightDelta = shootTarget.position.y - ShootTransform.position.y;
        float heightDeltaAngle = (heightDelta / d).tan().atan().degrees();

        float angle =  ((G * d / v.sqr()).asin().degrees() / 2).clamp(-360,360 * UnityMathematicsExtensions.PI);
        return angle - heightDeltaAngle;
    }
}