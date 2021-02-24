using UME;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(TurretAI))]
public class EnemyDetector : MonoBehaviour
{
    private TurretAI TurretAI;
    private void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        TurretAI = GetComponent<TurretAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TurretAI.shootTarget == null && other.CompareTag("Enemy"))
            TurretAI.shootTarget = other.transform;
    }

    void ResetTargetIfDistanceIsTooFar()
    {
        float distance = (transform.position - TurretAI.shootTarget.position).length();
    }
}