using UnityEngine;


public class TankSpawner : MonoBehaviour
{
    public TankAI Tank;
    public Transform Destination;
    public Transform Target;
    public float DelayToSpawnMin;
    public float DelayToSpawnMax;

    public float _delay = 2;
    private float _timer;
    
    
    void Start()
    {
        _delay = Random.Range(DelayToSpawnMin, DelayToSpawnMax);
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _delay)
        {
            _timer = 0;
            TankAI tank = Instantiate(Tank, transform.position, Tank.transform.rotation);
            tank.Destination = Destination;
            tank.TurretAI.shootTarget = Target;
            _delay = Random.Range(DelayToSpawnMin, DelayToSpawnMax);
        }
    }
}
