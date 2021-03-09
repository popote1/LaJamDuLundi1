using UME;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class LegController : MonoBehaviour
{

    public bool IsDrawDebug;
    
    public Transform RaycastEmitter;
    public Transform FootPosition;
    
    public float LegMovementSpeed;
    public float DistanceToMoveFood = 1;
    public float LeverDePatte = 1;
    
    public GameObject BurstParticle;
    
    private Vector3 _collisionPose;
    private Vector3 _oldFootPos;
    
    private float _timer = 0;
    
    public bool IsMoving;
    public bool CanMove;
    
    public UnityEvent PlayAtMove;
    public UnityEvent PlayAtImpact;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(RaycastEmitter.position, RaycastEmitter.forward , Color.magenta);
        if (Physics.Raycast(RaycastEmitter.position, RaycastEmitter.forward, out var hit, Mathf.Infinity))
        {
            _collisionPose = hit.point;
            if (IsDrawDebug)
            {
                Debug.DrawLine(RaycastEmitter.position, _collisionPose , Color.red);
                Debug.DrawLine(_collisionPose, FootPosition.position , Color.green);
            }

            if (Vector3.Distance(FootPosition.position, _collisionPose) > DistanceToMoveFood&&!IsMoving&&CanMove)
            {
                _oldFootPos = FootPosition.position;
                IsMoving = true;
                _timer = 0;
                PlayAtMove.Invoke();
            }

            if (IsMoving)
            {
                _timer += Time.deltaTime;
                if (_timer >= LegMovementSpeed)
                {
                    _timer =  LegMovementSpeed;
                    IsMoving = false;
                    if (BurstParticle != null)Destroy(Instantiate(BurstParticle, _collisionPose, Quaternion.identity),5);
                    PlayAtImpact.Invoke();
                }

                float timeInLerp = _timer / LegMovementSpeed;
                Vector3 footPos = new Vector3(
                    math.lerp(_oldFootPos.x, _collisionPose.x,timeInLerp),
                    (_oldFootPos.y+ _collisionPose.y) / 2 + function(timeInLerp,LeverDePatte),
                    math.lerp(_oldFootPos.z, _collisionPose.z,timeInLerp));
                FootPosition.position = footPos;
                
            }
        }
        
    }
    private float function(float value, float strength)
    {
        var a = 2 * value - 1;
        var b = 1 - a.abs();
        return b.smoothstep() * strength;
    }
}
