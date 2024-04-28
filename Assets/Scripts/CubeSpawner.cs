using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Collider))]
public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private FallingCube _cubePrefab;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 15;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _spawnRate = 0.2f;
    [SerializeField] private float _minDestroyDelay = 2;
    [SerializeField] private float _maxDestroyDelay = 5;

    private ObjectPool<FallingCube> _pool;

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void Awake()
    {
        _pool = new ObjectPool<FallingCube>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: GetCube,
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: Destroy,
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnCube), 0, _spawnRate);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FallingCube cube) == false)
            return;

        ReleaseCube(cube);
    }

    private void SpawnCube()
    {
        _pool.Get(); 
    }

    private void GetCube(FallingCube cube)
    {
        cube.SetPosition(GetRandomSpawnPosition());
        cube.gameObject.SetActive(true);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomPointInCircle = Random.insideUnitCircle * _spawnRadius;

        return new Vector3(randomPointInCircle.x, transform.position.y, randomPointInCircle.y);
    }

    private void ReleaseCube(FallingCube cube)
    {
        if (cube.HasTouchedFloor)
            return;
        
        cube.CollideWithFloor();

        StartCoroutine(nameof(DelayedDestroy), cube);
    }
    
    private IEnumerator DelayedDestroy(FallingCube cube)
    {
        float delay = Random.Range(_minDestroyDelay, _maxDestroyDelay);

        yield return new WaitForSeconds(delay);
        
        cube.gameObject.SetActive(false);
        _pool.Release(cube);
    }
}
