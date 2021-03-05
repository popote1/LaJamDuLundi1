using UnityEngine;

[RequireComponent(typeof(EnemyDetector))]
public class LaserTurretAI : Turret
{

    [Header("If Is Laser")]
    public Laser Laser;


    void Start() => SetParameters();

    void Update()
    {
        if(shootTarget != null) OrientCannon(shootTarget.position);
        if(Input.GetKeyDown(KeyCode.Space)) Shoot();
    }

    public override void Shoot()
    {
        //throw new System.NotImplementedException();
    }

    protected override void OrientCannon(Vector3 target)
    {
        CurrentTarget = Vector3.Lerp(CurrentTarget, shootTarget.position, RotateSpeed);
        TurretBaseTransform.LookAt(new Vector3(CurrentTarget.x, TurretBaseTransform.position.y, CurrentTarget.z));
        TurretCannonTransform.LookAt(CurrentTarget);
    }
}