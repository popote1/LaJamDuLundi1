using System;
using UME;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(TurretAI))]
public class EnemyDetector : MonoBehaviour
{
    private TurretAI TurretAI;
    public float MaxDetectionDistance = 40;
    private void Start()
    {
        var SC = GetComponent<SphereCollider>();
        SC.isTrigger = true;
        SC.radius = MaxDetectionDistance;
        TurretAI = GetComponent<TurretAI>();
    }

    private void Update()
    {
        // ResetTargetIfDistanceIsTooFar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TurretAI.shootTarget == null && other.CompareTag("Enemy"))
            TurretAI.shootTarget = other.transform;
    }

    void ResetTargetIfDistanceIsTooFar()
    {
        if (TurretAI.shootTarget == null) return;
        float distance = (transform.position - TurretAI.shootTarget.position).length();
        if (distance > MaxDetectionDistance)
        {
            TurretAI.shootTarget = null;
            //TurretAI.Laser.isHitting = false; // disables laser
        }
    }
}