using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AstroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _astroidPrefabs;
    [SerializeField] private float _secondBetweenSpawn;
    [SerializeField] private Vector2 _forceRange;

    private Camera _mainCamera;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;

        if(_timer <= 0)
        {
            SpawnAstroid();
            _timer = _secondBetweenSpawn;
        }
    }

    private void SpawnAstroid()
    {
        int spawnSide = Random.Range(0, 4);
        int prefabIndex = Random.Range(0, _astroidPrefabs.Length);

        Vector2 spawnPoint = Vector2.zero;
        Vector2 direction = Vector2.zero;

        switch(spawnSide)
        {
            case 0: // Left
                spawnPoint = new Vector2(0, Random.value);
                direction = new Vector2(1f, Random.Range(-1f, 1f));
                break;
            case 1: // Right
                spawnPoint = new Vector2(1, Random.value);
                direction = new Vector2(-1f, Random.Range(-1f, 1f));
                break;
            case 2: // Bottom
                spawnPoint = new Vector2(Random.value, 0);
                direction = new Vector2(Random.Range(-1f, 1f), 1f);
                break;
            case 3: // Up
                spawnPoint = new Vector2(Random.value, 1);
                direction = new Vector2(Random.Range(-1f, 1f), -1f);
                break;
        }
        // transfer spawn point to world point
        Vector3 worldSpawnPoint = _mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = 0;

        // Spawn new astroid
        GameObject astroid = Instantiate(
            _astroidPrefabs[prefabIndex], 
            worldSpawnPoint, 
            Quaternion.Euler(0f, 0f, Random.Range(0, 360)));
        
        // Change the astroid velocity
        astroid.GetComponent<Rigidbody>().velocity = direction.normalized * Random.Range(_forceRange.x, _forceRange.y);
    }
}
