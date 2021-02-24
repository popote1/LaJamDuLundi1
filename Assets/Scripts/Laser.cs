
using UnityEngine;

[ExecuteAlways]
public class Laser : MonoBehaviour
{
    [Header("References")]
    public Transform Origin;
    public LineRenderer LR;
    
    [Header("Parameters")]
    public float LaserWidth = 0.1f;
    public float MaxDistance = 20;
    public float ImpulseForce = 10;
    
    //private
    private Vector3[] Positions = new Vector3[2];
    private Vector3 TargetPosition;
    private ParticleSystem PS;

    public bool isHitting;

    [Header("Prefabs")] public GameObject ParticleEffect;
    
    
    
    private void OnValidate()
    {
        LR.widthMultiplier = LaserWidth;
        SetPositions();
    }

    void Update()
    { 
        SetPositions();
        
        ParticleSystem PS = ParticleEffect.GetComponent<ParticleSystem>();

        if (isHitting & PS.isStopped) PS.Play();
        else if(!isHitting & !PS.isStopped) PS.Stop();
        
        if (isHitting) ParticleEffect.transform.position = TargetPosition;

        if (isHitting)
        {
            var colliders = Physics.OverlapSphere(TargetPosition, LaserWidth);
            foreach (var c in colliders)
            {
                var direction = TargetPosition - c.transform.position;
                var length = direction.magnitude;
                Rigidbody rb = c.GetComponent<Rigidbody>();
                if(rb != null) rb.AddForce((ImpulseForce * (- direction.normalized) + Vector3.up * 3) / (length), ForceMode.Impulse);
            }
        }

        // LR.enabled = isHitting;

    }

    void SetTargetPosition()
    {
        Physics.Raycast(Origin.position, Origin.forward, out var hit);
        if (hit.collider != null) 
        {
            TargetPosition = hit.point;
            isHitting = true;
        }
        else {
            TargetPosition = (Origin.position + Origin.forward * MaxDistance);
            isHitting = false;
        }
    }

    void SetPositions()
    {
        SetTargetPosition();
        Positions[0] = Origin.position;
        Positions[1] = TargetPosition;
        LR.SetPositions(Positions);
    }

    void IsHittingEqualsHasTarget()
    {
        
    }
}
