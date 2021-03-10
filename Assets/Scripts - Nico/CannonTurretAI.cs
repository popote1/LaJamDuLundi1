using UME;
using Unity.Mathematics;
using UnityEngine;
using static UME.UnityMathematicsExtensions;
using Random = UnityEngine.Random;

public class CannonTurretAI : Turret
{
    
    // [Header("Variables")]
    public float ShootForce = 10f;
    public float ShootForceRandomness = 1f;

    [Header("Prefabs")]
    public GameObject BulletPrefab;
    public GameObject ShootParticle;

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
        // Debug.Log("Shot");
        if (shootTarget == null) return;
        float3 r = CannonEndTransform.rotation.eulerAngles;
        Destroy(Instantiate(ShootParticle ,CannonEndTransform.position, Quaternion.Euler(new float3(r.x + 90, r.yz))),5);
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
        
        // float maxDistance = ComputeDistanceFromAngle(45, v, heightDelta);

        float angle = 45;
        // if (maxDistance >= d) {
        //     // angle =  ((G * d / v.sqr()).asin().degrees() / 2).clamp(-360, 360 * PI);
        //     angle = (G * d / v.sqr()).asin().degrees() / 2;
        //     angle -= heightDeltaAngle;
        // }
        
        angle = ((G * d / v.sqr()).asin().degrees() / 2 - heightDeltaAngle);
        angle = float.IsNaN(angle) ? -45 : angle;
        // angle = angle == float.NaN ? 45 : ((G * d / v.sqr()).asin().degrees() / 2 - heightDeltaAngle).clamp(-45,0);
        // angle = (G * d / v.sqr()).asin().degrees() / 2;
        // angle -= heightDeltaAngle;
        
        // Debug.Log("Angle : " + angle);
        // Debug.Log("Max Distance : " + maxDistance);
        // Debug.Log("Distance : " + d);
        // Debug.Log(angle);
        
        return angle;
    }

    float ComputeDistanceFromAngle(float angle, float initialVelocity, float startHeight)
    {
        float v = ComputeInitialVelocity(initialVelocity);
        if (startHeight == 0) {
            return (v.sqr() / -G) * (2 * angle).sin();
        }
        var sinCos = angle.sincos();
        var vSin = v * sinCos.x;
        return (v / -G) * sinCos.y * (vSin + (vSin).sqr() + 2 * -G * startHeight);
    }
}