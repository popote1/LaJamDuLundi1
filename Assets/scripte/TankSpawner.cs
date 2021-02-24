using UnityEngine;

public class TankSpawner : MonoBehaviour
{
    public TankAI Tank;
    public Transform Destination;
    public Transform Target;
    public float DelayToSpawnMin;
    public float DelayToSpawnMax;

    private float _delay;
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
            TankAI tank = Instantiate(Tank, transform.position, Quaternion.identity);
            tank.Destination = Destination;
            tank.shootTarget = Target;
            _timer = 0;
            _delay = Random.Range(DelayToSpawnMin, DelayToSpawnMax);
        }
    }
}
