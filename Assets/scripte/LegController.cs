using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class LegController : MonoBehaviour
{

    public bool IsDrawDebug;
    public Transform RaycastEmiteur;
    public Transform FoodPosition;
    public float LegMovementSpeed;
    public float DistanceToMoveFood = 1;
    public float leverDePat = 1;
    public GameObject BurstParicule;
    private Vector3 _collisionPose;
    private Vector3 _oldFoodPos;
    private float _timer = 0;
    public bool IsMoving;
    public bool CanMove;
    public UnityEvent PlayAtMove;
    public UnityEvent PlayAtInpact;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(RaycastEmiteur.position, RaycastEmiteur.forward , Color.magenta);
        if (Physics.Raycast(RaycastEmiteur.position, RaycastEmiteur.forward, out var hit, Mathf.Infinity))
        {
            _collisionPose = hit.point;
            if (IsDrawDebug)
            {
                Debug.DrawLine(RaycastEmiteur.position, _collisionPose , Color.red);
                Debug.DrawLine(_collisionPose, FoodPosition.position , Color.green);
            }

            if (Vector3.Distance(FoodPosition.position, _collisionPose) > DistanceToMoveFood&&!IsMoving&&CanMove)
            {
                _oldFoodPos = FoodPosition.position;
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
                    if (BurstParicule != null)Destroy(Instantiate(BurstParicule, _collisionPose, quaternion.identity),5);
                    PlayAtInpact.Invoke();
                }

                float timeInLerp = _timer / LegMovementSpeed;
                Vector3 footPos = new Vector3(
                    Mathf.Lerp(_oldFoodPos.x, _collisionPose.x,timeInLerp),
                    (_oldFoodPos.y+ _collisionPose.y)/2+function(timeInLerp,leverDePat),
                    Mathf.Lerp(_oldFoodPos.z, _collisionPose.z,timeInLerp));
                FoodPosition.position = footPos;
                
            }
        }
        
    }
    private float function(float value, float strength)
    {
        float a = 2 * value - 1;
        float b = 1 - Mathf.Abs(a);
        return Mathf.SmoothStep(0, 1, b) * strength;
    }
}
