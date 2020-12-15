using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _asteroidPrefab;
    [SerializeField]
    private GameObject[] _enemyPrefab;
    private GameObject[] _wave;
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
    public UIManager _uiManager;
    private int _enemyWave;
    public int  EnemyType;
    bool[] spawned;
    public float[]  spawnTime;
    public bool BossSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _wave = new GameObject[8];
        spawned = new bool[3];
        spawnTime = new float[8];

        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        if (_uiManager == null)
        {
            Debug.LogError("Player is NULL");
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(AsteroidSpawnRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
      //  StartCoroutine(SpawnRarePowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        _enemyWave = _uiManager.EnemyWave;
    }

    IEnumerator AsteroidSpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(10.0f, 16.0f));
            Vector3 asteroidSpawnPos = new Vector3(Random.Range(-7f, 7f), 7f, 0);
            Instantiate(_asteroidPrefab, asteroidSpawnPos, Quaternion.identity);
        }
    }
    IEnumerator SpawnRoutine()
      {

          while (_stopSpawning == false && BossSpawned == false)
          {
             //  yield return new WaitForSeconds(Random.Range(2.0f, 6.0f));

            Vector3 posToSpawn = new Vector3(Random.Range(-11f, 11f), 9f, 0);
            Vector3 BossSpawn = new Vector3(0, 10.7f, 0);
           

            _enemyWave = _uiManager.EnemyWave;
          switch (_enemyWave)
                  {     
                  case 0:
                      Debug.Log("0 wave playing");
                      yield return new WaitForSeconds(Random.Range(4.0f, 7.0f));
                      for (int i = 0; i < 1; i++)
                      {
                          GameObject newEnemy = Instantiate(_enemyPrefab[EnemyType], posToSpawn, Quaternion.identity);
                        _wave[i] = newEnemy;
                          newEnemy.transform.parent = _enemyContainer.transform;
                      }
                      break;
                  case 1:
                      yield return new WaitForSeconds(Random.Range(3.0f, 6.0f));
                      for (int i = 0; i < 1; i++)
                      {
                        EnemyType = Random.Range(0, 2);
                        GameObject newEnemy = Instantiate(_enemyPrefab[EnemyType], posToSpawn, Quaternion.identity);
                          _wave[i] = newEnemy;
                          _wave[i].transform.parent = _enemyContainer.transform;
                          Debug.Log("1st wave playing");
                      }
                       break;
                  case 2:
                      yield return new WaitForSeconds(Random.Range(4.0f, 7.0f));
                      for (int i = 0; i <= 1; i++)
                      {
                        EnemyType = Random.Range(0, 3);
                          GameObject newEnemy = Instantiate(_enemyPrefab[EnemyType], posToSpawn, Quaternion.identity);
                          _wave[i] = newEnemy;
                          _wave[i].transform.parent = _enemyContainer.transform;
                          Debug.Log("2nd wave playing");
                      }
                      break;
                  case 3:
                      Debug.Log("3rd wave playing");
                   
                    //Spawn 1 elite unit before regular wave
                    if (spawned[0] == false)
                    {
                        EnemyType = 3;
                        Instantiate(_enemyPrefab[EnemyType], posToSpawn, Quaternion.identity);
                        spawned[0] = true;
                    }

                    yield return new WaitForSeconds(Random.Range(4.0f, 7.0f));
                      for (int i = 0; i <= 1; i++)
                      {
                        EnemyType = Random.Range(0, 3);
                          GameObject newEnemy = Instantiate(_enemyPrefab[EnemyType], posToSpawn, Quaternion.identity);
                          _wave[i] = newEnemy;
                          _wave[i].transform.parent = _enemyContainer.transform;
                      }
                      break;
                  case 4:
                    Debug.Log("4th wave playing");
                   
                    yield return new WaitForSeconds(Random.Range(3.0f, 6.0f));
                      for (int i = 0; i <= 1; i++)
                      {
                        EnemyType = Random.Range(0, 3);
                          GameObject newEnemy = Instantiate(_enemyPrefab[EnemyType], posToSpawn, Quaternion.identity);
                          _wave[i] = newEnemy;
                          _wave[i].transform.parent = _enemyContainer.transform;
                      }
                      break;
                  case 5:
                      Debug.Log("5th wave playing");
                    
                    //Spawn 1 elite unit before regular wave
                    if (spawned[1] == false)
                    {
                        EnemyType = 3;
                        Instantiate(_enemyPrefab[EnemyType], posToSpawn, Quaternion.identity);
                        spawned[1] = true;
                    }

                    yield return new WaitForSeconds(Random.Range(5.0f, 8.0f));
                      for (int i = 0; i <= 2; i++)
                      {
                        EnemyType = Random.Range(0, 3);
                          GameObject newEnemy = Instantiate(_enemyPrefab[EnemyType], posToSpawn, Quaternion.identity);
                          _wave[i] = newEnemy;
                          _wave[i].transform.parent = _enemyContainer.transform;
                      }
                      break;
                  case 6:
                    Debug.Log("6th wave playing");
                 
                      yield return new WaitForSeconds(Random.Range(3.0f, 6f));
                      for (int i = 0; i <= 2; i++)
                      {
                        EnemyType = Random.Range(0, 3);
                          GameObject newEnemy = Instantiate(_enemyPrefab[EnemyType], posToSpawn, Quaternion.identity);
                          _wave[i] = newEnemy;
                          _wave[i].transform.parent = _enemyContainer.transform;
                      }
                      break;
                  case 7:

                        BossSpawned = true;
                        Debug.Log("Boss Spawning");
                        yield return new WaitForSeconds(3f);
                        EnemyType = 4;
                        GameObject boss = Instantiate(_enemyPrefab[EnemyType], BossSpawn, Quaternion.Euler(0, 0, 180));
                        yield return new WaitForSeconds(7f);
                    break;
                  default:
                     Debug.Log("Not a valid Wave");
                     break;
                  }

              if(_enemySpawnDetection == false && (_player._homingMissileActive == true))
              {
               _enemySpawnDetection = true;
              }
          }
        
    }

    public void MinionsSpawn()
    {
        StartCoroutine(MinionsSpawnRoutine());
    }

    IEnumerator MinionsSpawnRoutine()
    {
        for (int i = 0; i <= 2; i++)
        {
            Debug.Log("enemy minions left spawned");
            EnemyType = 5;
            Vector3 MinionsSpawnLeft = new Vector3(Random.Range(-12f, -3f), 11f, 0);
            GameObject minionsleft = Instantiate(_enemyPrefab[EnemyType], MinionsSpawnLeft, Quaternion.identity);
            _wave[i] = minionsleft;
            _wave[i].transform.parent = _enemyContainer.transform;
        }
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i <= 2; i++)
        {
            Debug.Log("enemy minions right spawned");
            EnemyType = 5;
            Vector3 MinionsSpawnRight = new Vector3(Random.Range(12f, 3f), 11f, 0);
            GameObject minionsright = Instantiate(_enemyPrefab[EnemyType], MinionsSpawnRight, Quaternion.identity);
            _wave[i] = minionsright;
            _wave[i].transform.parent = _enemyContainer.transform;
        }
    }
    IEnumerator SpawnPowerUpRoutine()
    {
        _enemyWave = _uiManager.EnemyWave;

        while (_stopSpawning == false)
        {
            switch (_enemyWave)
            {
                case 0:
                    spawnTime[_enemyWave] = Random.Range(9f, 13f);
                    Debug.Log("PowerUp Wave 0");
                    break;
                case 1:
                    spawnTime[_enemyWave] = Random.Range(8f, 12f);
                    Debug.Log("PowerUp Wave 1");
                    break;
                case 2:
                    spawnTime[_enemyWave] = Random.Range(7f, 11f);
                    Debug.Log("PowerUp Wave 2");
                    break;
                case 3:
                    spawnTime[_enemyWave] = Random.Range(5f, 8f);
                    Debug.Log("PowerUp Wave 3");
                    break;
                case 4:
                    spawnTime[_enemyWave] = Random.Range(5f, 7f);
                    Debug.Log("PowerUp Wave 4");
                    break;
                case 5:
                    spawnTime[_enemyWave] = Random.Range(4f, 6f);
                    Debug.Log("PowerUp Wave 5");
                    break;
                case 6:
                    spawnTime[_enemyWave] = Random.Range(3f, 6f);
                    Debug.Log("PowerUp Wave 6");
                    break;
                case 7:
                    spawnTime[_enemyWave] = Random.Range(3f, 5f);
                    Debug.Log("PowerUp Wave 7");
                    break;
                default:
                    Debug.Log("Invalid Wave");
                    break;
            }

            yield return new WaitForSeconds(spawnTime[_enemyWave]);

            Vector3 posToSpawn = new Vector3(Random.Range(-10f, 10f), 9f, 0);

            int randomPowerUp = Random.Range(0, 130);
            int powerupID = 0;

            if (randomPowerUp >= 0 && randomPowerUp <= 19)
            {
                powerupID = 0; //tripleShot
            }
            if (randomPowerUp >= 20 && randomPowerUp <= 39)
            {
                powerupID = 1; //Speed
            }
            if (randomPowerUp >= 40 && randomPowerUp <= 59)
            {
                powerupID = 2; //Shields
            }
            if (randomPowerUp >= 60 && randomPowerUp <= 94)
            {
                powerupID = 3; //Ammo
            }
            if (randomPowerUp >= 95 && randomPowerUp <= 109)
            {
                powerupID = 4; //Health
            }
            if (randomPowerUp >= 110 && randomPowerUp <= 119)
            {
                powerupID = 5; //Homing Missile
            }
            if (randomPowerUp >= 120 && randomPowerUp <= 129)
            {
                powerupID = 6; //Monkey Debuff
            }

            Instantiate(powerups[powerupID], posToSpawn, Quaternion.identity);

        }
    }    

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
