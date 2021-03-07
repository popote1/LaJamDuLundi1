using System;
using UME;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Turret))]
public class EnemyDetector : MonoBehaviour
{
    private Turret Turret;
    public float MaxDetectionDistance = 40;
    private void Start()
    {
        var SC = GetComponent<SphereCollider>();
        SC.isTrigger = true;
        SC.radius = MaxDetectionDistance;
        Turret = GetComponent<Turret>();
    }

    private void Update()
    {
        ResetTargetIfDistanceIsTooFar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Turret.shootTarget == null && other.CompareTag("Enemy"))
            Turret.shootTarget = other.transform;
    }

    void ResetTargetIfDistanceIsTooFar()
    {
        if (Turret.shootTarget == null) return;
        float distance = (transform.position - Turret.shootTarget.position).length();
        if (distance > MaxDetectionDistance)
        {
            Turret.shootTarget = null;
            //TurretAI.Laser.isHitting = false; // disables laser
        }
    }
}