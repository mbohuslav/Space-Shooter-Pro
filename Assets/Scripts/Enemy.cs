using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
   // private Player _player;
    private Animator _anim;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    public GameObject _enemyLaserPrefabAlt;
    private int _enemyID;
    private GameObject _enemyModel;
    private string _enemyName;
    private int _enemyMoveType;
    private bool _canmove = true;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _shieldVisualizer;
    private bool _shieldsActive = false;
    public int firePattern;
    GameObject _powerUp;
    // player tracking for movetowards
    
    GameObject _targetplayer;

    private Vector3 target;

    //zigzag movement for enemy
    Vector3 pos;
    Vector3 axis;
    float frequency = 2.4f; // Speed of sine movement
    float magnitude = 3f; //  Size of sine movement

    //Ray casting firing speed
    float _FireRate = 2f;
    float _canFire = -1f;
    
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        axis = transform.right;
        _enemyMoveType = Random.Range(0,4);
        transform.position = new Vector3(Random.Range(-11f, 11f), 9f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _enemyID = _spawnManager.EnemyType;
        _targetplayer = GameObject.FindWithTag("Player");
        _powerUp = GameObject.FindWithTag("PowerUp");

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

        if (_enemyID == 2)
        {
            _shieldsActive = true;
            _shieldVisualizer.SetActive(true);
        }

        _enemyModel = this.gameObject;
        _enemyName = _enemyModel.name;

        if (_enemyName == "Enemy0(Clone)")
        { firePattern = 0; }
        if (_enemyName == "Enemy1(Clone)")
        { firePattern = 1; }
        if (_enemyName == "Enemy0b(Clone)")
        { firePattern = 2; }

       StartCoroutine(FireLaser());
      
    }

    // Update is called once per frame
    void Update()
    {
        if (_canmove == true)
        {
           
            

            float distance = Vector3.Distance(target, transform.position);
            float velocity = _speed * Time.deltaTime;
            if(_targetplayer != null)
            {
                target = _targetplayer.transform.position;
            }

            if (_targetplayer != null && _enemyName == "Enemy0b(Clone)" && distance <= 6f)

            { transform.position = Vector3.MoveTowards(transform.position, target, velocity); }


            else
            {
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
            }

            if (transform.position.y < -8.5f || transform.position.x < -14f || transform.position.x > 14f)
            {
                float randomX = Random.Range(-11f, 11f);
                _enemyMoveType = Random.Range(0, 4);
                transform.position = new Vector3(randomX, 9f, 0);
                pos = transform.position;
            }
        }  

    }
    IEnumerator FireLaser()
    {
        if (_canmove == true)
        {


            switch (firePattern)
            {
                case 0:
                    yield return new WaitForSeconds(Random.Range(0.1f, 1.0f));
                    Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.584f, 0), Quaternion.identity);
                    yield return new WaitForSeconds(Random.Range(1.5f, 3.0f));
                    if (_canmove == true)
                    { StartCoroutine(FireLaser()); }
                    break;
                case 1:
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
                case 2:
            
                    yield return new WaitForSeconds(Random.Range(0.1f, 1.0f));
                    Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.584f, 0), Quaternion.identity);
                    yield return new WaitForSeconds(Random.Range(1.5f, 3.0f));
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
    }

    void FixedUpdate()
    {
        if (Time.time > _canFire)
        {
            FireAtPowerUp();
            FireAtPlayer();
        }
    }

    
    
    private void FireAtPowerUp()
    {
        int layerMask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, layerMask);

        if (hit.collider != null && hit.collider.tag == "PowerUp")
        {
            _canFire = Time.time + _FireRate;
            Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.584f, 0), Quaternion.identity);
            Debug.Log("PowerUp Fired at");
        }        
    }

    private void FireAtPlayer()
    {
        int layerMask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, layerMask);

        if (hit.collider != null && hit.collider.tag == "Player" && firePattern == 1)
        {
            _canFire = Time.time + _FireRate;
            Instantiate(_enemyLaserPrefabAlt, transform.position + new Vector3(0, -0.584f, 0), Quaternion.identity);
            Debug.Log("Player Fired at");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {    

        if (other.tag == "Player")
        {

            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();


                if (_enemyID == 2 && _shieldsActive == true)
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
           
            if (_enemyID == 2 && _shieldsActive == true)
            {
                _shieldVisualizer.SetActive(false);
                _shieldsActive = false;
                _audioSource.Play();
            }
            else
            {
               // if (_uiManager != null)
                //{
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
