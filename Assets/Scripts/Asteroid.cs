using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 19.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _uiManager.InitiateEnemyWave();
            _spawnManager.StartSpawning();
            Destroy(GetComponent<SpriteRenderer>(), 0.25f);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 3.018f);
        }

        if (other.tag =="Laser")
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _uiManager.InitiateEnemyWave();
            _spawnManager.StartSpawning();
            Destroy(GetComponent<SpriteRenderer>(), 0.25f);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 3.018f);
        }
        
        if (other.tag == "SuperPower")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _uiManager.InitiateEnemyWave();
            _spawnManager.StartSpawning();
            Destroy(GetComponent<SpriteRenderer>(), 0.25f);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 3.018f);
        }
    }
   
}
