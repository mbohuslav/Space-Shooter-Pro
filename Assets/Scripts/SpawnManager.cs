using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private GameObject[] rarepowerups;

    [SerializeField]
    private bool _stopSpawning = false;
   
    public bool _enemySpawnDetection = false;
    private  Player _player;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        StartCoroutine(SpawnRarePowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {
    
    }
   
    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
           if(_enemySpawnDetection == false && (_player._homingMissileActive == true))
            {
             _enemySpawnDetection = true;
            }
        yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
        }
    }
    IEnumerator SpawnPowerUpRoutine()

    {
        yield return new WaitForSeconds(Random.Range(4.0f, 8.0f));
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            int randomPowerUp = Random.Range(0, 5);
            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(4.0f, 9.0f));
        }
    }
    IEnumerator SpawnRarePowerUpRoutine()

    {
        yield return new WaitForSeconds(Random.Range(12.0f, 24.0f));
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            int randomRarePowerUp = Random.Range(0, 2);
            Instantiate(rarepowerups[randomRarePowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(15.0f, 25.0f));
        }
    }
            

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
