using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    [Header("Transforms")]
    public Transform TurretBaseTransform;
    public Transform TurretCannonTransform;
    public Transform ShootTransform;
    public Transform CannonEndTransform;

    [Header("Target Transform")]
    public Transform shootTarget;
    public Vector3 CurrentTarget;

    [Header("Variables")]
    [Range(0,1)] public float RotateSpeed = 0.05f;


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

    public abstract void Shoot();

    protected abstract void OrientCannon(Vector3 target);

    protected virtual void SetParameters()
    {
        CurrentTarget = ShootTransform.forward * 3 + ShootTransform.position; // Start target position for smoother lerp
        if (CannonEndTransform == null) CannonEndTransform = ShootTransform; // Prevents Null Reference
    }

    protected float ComputeShootDistance() => (shootTarget.position - ShootTransform.position).magnitude;
}