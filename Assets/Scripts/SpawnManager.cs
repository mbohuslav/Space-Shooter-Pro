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
    private bool _stopSpawning = false;
    
   


    // Start is called before the first frame update
    void Start()
    {
  
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
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
            yield return new WaitForSeconds(Random.Range(2.0f, 6.0f));
        }
    }
    IEnumerator SpawnPowerUpRoutine()

    {
        yield return new WaitForSeconds(Random.Range(4.0f,8.0f));
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            int randomPowerUp = Random.Range(0, 5);
            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(6.0f, 12.0f));

            /*  _powerUpID = Random.Range(0,4);
            
            if (_powerUpID == 0)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
                GameObject newTripleShotPowerUp = Instantiate(_TripleShotPowerUpPrefab, posToSpawn, Quaternion.identity);
            }
            else if (_powerUpID ==1)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
                GameObject newSpeedBoostPowerUp = Instantiate(_SpeedBoostPowerUpPrefab, posToSpawn, Quaternion.identity);
            }
            else if (_powerUpID ==2)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
                GameObject newShieldPowerup = Instantiate(_ShieldPowerUpPrefab, posToSpawn, Quaternion.identity);
            }

            yield return new WaitForSeconds(Random.Range(4.0f, 8.0f));    */
        }

    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
