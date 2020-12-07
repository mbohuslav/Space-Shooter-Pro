using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
   // private Player _player;
    private Animator _anim;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    private int _enemyID;
    private int _enemyMoveType;
    private bool _canmove = true;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _shieldVisualizer;
    private bool _shieldsActive = false;
    
    //zigzag movement for enemy
    Vector3 pos;
    Vector3 axis;
    float frequency = 2.4f; // Speed of sine movement
    float magnitude = 3f; //  Size of sine movement


    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        axis = transform.right;
        _enemyMoveType = Random.Range(0,4);
        transform.position = new Vector3(Random.Range(-8f, 8f), 7f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _enemyID = _spawnManager.EnemyType;

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL");
        }
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if (_enemyID == 1)
        {
            _shieldsActive = true;
            _shieldVisualizer.SetActive(true);
        }



        StartCoroutine(FireLaser()); 

    }

    // Update is called once per frame
    void Update()
    {
        if (_canmove == true)
        {
            float velocity = _speed * Time.deltaTime;
            

            switch (_enemyMoveType)
            {

                case 0:
                    transform.Translate(Vector3.down * velocity);
                    break;
                case 1:
                    transform.Translate(-velocity * 0.75f, -velocity * 0.75f, 0);
                    break;
                case 2:
                    transform.Translate(velocity * 0.75f, -velocity * 0.75f, 0);
                    break;
                case 3:
                    pos += Vector3.down * velocity;
                    transform.position = pos + axis * Mathf.Cos(Time.time * frequency) * magnitude;
                    break;
                /* case 4:
                     _timeCount -= Time.deltaTime;
                     transform.Translate(x * velocity, y * velocity, z);
                     break; */

                default:
                    Debug.Log("Not a valid Shield Strength");
                    break;
            }


            if (transform.position.y < -5.5f || transform.position.x < -14f || transform.position.x > 14f)
            {
                float randomX = Random.Range(-8f, 8f);
                _enemyMoveType = Random.Range(0, 4);
                transform.position = new Vector3(randomX, 7f, 0);
                pos = transform.position;
            }
        }  

    }

    IEnumerator FireLaser()
    {
        if (_canmove == true)
        {
            switch (_enemyID)
            {
                case 0:
                    yield return new WaitForSeconds(Random.Range(0.1f, 1.0f));
                    Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.584f, 0), Quaternion.identity);
                    yield return new WaitForSeconds(Random.Range(1.5f, 3.0f));
                    if (_canmove == true)
                    { StartCoroutine(FireLaser()); }
                    break;
                case 1:
                    yield return new WaitForSeconds(Random.Range(0.1f, 1.0f));
                    Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.584f, 0), Quaternion.identity);
                    yield return new WaitForSeconds(Random.Range(1.5f, 3.0f));
                    if (_canmove == true)
                    { StartCoroutine(FireLaser()); }
                    break;
                case 2:
                    yield return new WaitForSeconds(Random.Range(0.01f, 1.0f));
                    Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.584f, 0), Quaternion.identity);
                    yield return new WaitForSeconds(0.2f);
                    Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.584f, 0), Quaternion.identity);
                    yield return new WaitForSeconds(0.2f);
                    Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.584f, 0), Quaternion.identity);
                    yield return new WaitForSeconds(Random.Range(2.0f, 3.0f));
                    if (_canmove == true)
                    { StartCoroutine(FireLaser()); }
                    break;
                default:
                    Debug.Log("Not a valid Enemytype");
                    break;
            }
        }
        else
        { 
            Debug.Log("Lasers are disabled"); 
        }
           
        

    //    yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
    //    Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -1.05f, 0), Quaternion.identity);
    //    yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
    } 
    private void OnTriggerEnter2D(Collider2D other)
    {    

        if (other.tag == "Player")
        {
            // other.transform.GetComponent<Player>().Damage();  Not Optimized for Null Check

            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();


                if (_shieldsActive == true)
                {
                    _shieldVisualizer.SetActive(false);
                    _shieldsActive = false;
                    _audioSource.Play();
                }
                else
                {
                    _canmove = false;
                    _anim.SetTrigger("OnEnemyDeath");
                    _audioSource.Play();
                    transform.gameObject.tag = "Dead Enemy";
                    Destroy(GetComponent<Collider2D>());
                    Destroy(this.gameObject, 2.5f);
                }
            }
        } 
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
           
            if (_shieldsActive == true)
            {
                _shieldVisualizer.SetActive(false);
                _shieldsActive = false;
                _audioSource.Play();
            }
            else
            {
                if (_uiManager != null)
                {
                  //  if (_anim.GetCurrentAnimatorStateInfo(1).IsName("Enemy_Destroyed_anim"))
                  //  {
                   //     _audioSource.Play();
                 //   }
                    
                        _uiManager.UpdateScore(10);
                        _canmove = false;
                        _anim.SetTrigger("OnEnemyDeath");
                        _audioSource.Play();
                        transform.gameObject.tag = "Dead Enemy";
                        Destroy(GetComponent<Collider2D>());
                        Destroy(this.gameObject, 2.5f);
                    

                }

              
            }
        }
    
    }
}
