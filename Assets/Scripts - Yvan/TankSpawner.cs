using UnityEngine;


public class TankSpawner : MonoBehaviour
{
    public TankAI Tank;
    public Transform Destination;
    public Transform Target;
    public float DelayToSpawnMin;
    public float DelayToSpawnMax;

    public float SpawnDelay = 0 ;
    public float InitialDelay = 2;
    private float _timer;
    
    
    void Start()
    {
        SpawnDelay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > SpawnDelay)
        {
            _timer = 0;
            TankAI tank = Instantiate(Tank, transform.position, Tank.transform.rotation);
            tank.Destination = Destination;
            tank.TurretAI.shootTarget = Target;
            SpawnDelay = Random.Range(DelayToSpawnMin, DelayToSpawnMax);
        }
    }
}
