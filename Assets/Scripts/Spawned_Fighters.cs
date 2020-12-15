using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawned_Fighters : MonoBehaviour
{

    private GameObject _targetplayer;
    private Boss _boss;
    private Vector3 target;
    public Transform PlayerTarget;

    static int num;
    public AudioSource _audioSource;
    [SerializeField]
    public AudioClip ExplosionSound;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private GameObject _explosionPrefab;
    //public bool FirstWaveAttack;
   // public bool SecondWaveAttack;
    private UIManager _uiManager;
    public GameObject ChildHolder;
    private float _ImmunityStart = 0f;
    private float ImmunityDuration = 5f;


    float _speed = 4.5f;
    float _canFire = -1f;
    bool _canmove = true;
    Vector3  StagePosition1, StagePosition2, StagePosition3, StagePosition4, StagePosition5, StagePosition6;

    Vector3 pos;
    Vector3 axis;
    float frequency = 2.4f; // Speed of sine movement
    float magnitude = 4f; //  Size of sine movement
    int _enemyMoveType;


    void Awake()
    {

        name += num.ToString();
        ++num;
    }
      
        void Start()
    {
        pos = transform.position;
        axis = transform.right;
        _enemyMoveType = Random.Range(0, 4);
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _boss = GameObject.Find("Boss(Clone)").GetComponent<Boss>();
        _targetplayer = GameObject.FindWithTag("Player");
        

        if (_boss == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if (_uiManager == null)
            {
                Debug.LogError("The UI Manager is NULL");
            }

            StagePosition1 = new Vector3(-7.2f, 4.24f, 0);
            StagePosition2 = new Vector3(-6f, 2.8f, 0);
            StagePosition3 = new Vector3(-4.8f, 4.31f, 0);
            StagePosition4 = new Vector3(4.8f, 4.22f, 0);
            StagePosition5 = new Vector3(6f, 2.8f, 0);
            StagePosition6 = new Vector3(7.2f, 4.26f, 0);
        _ImmunityStart = Time.time + ImmunityDuration;
    }
    void Update()
    {
        EnemyInitialStaging();

       // EnemyMovement();            //remove after testing!!!
       // if (Time.time > _canFire)   //remove after testing!!!
      //  {
      //      EnemyFireSequence();
     //   }

    }
    private void EnemyInitialStaging()
    {

        
        float velocity = _speed * Time.deltaTime;

        if (_boss._grandEntranceComplete == false)
        {
            Debug.Log(" Spawned Movement started");

            if (transform.position != StagePosition1 && name == "Spawned Fighter(Clone)0")
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-7.2f, 4.24f, 0), velocity);
                return;
            }
            if (transform.position != StagePosition2 && name == "Spawned Fighter(Clone)1")
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-6f, 2.8f, 0), velocity);
                return;
            }
            if (transform.position != StagePosition3 && name == "Spawned Fighter(Clone)2")
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-4.8f, 4.31f, 0), velocity);
                return;
            }
            if (transform.position != StagePosition4 && name == "Spawned Fighter(Clone)3")
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(4.8f, 4.22f, 0), velocity);
                return;
            }
            if (transform.position != StagePosition5 && name == "Spawned Fighter(Clone)4")
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(6f, 2.8f, 0), velocity);
                return;
            }
            if (transform.position != StagePosition6 && name == "Spawned Fighter(Clone)5")
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(7.2f, 4.26f, 0), velocity);
                return;
            }
        }


        if (_boss.FirstSequenceComplete == false)
        {

            if (transform.position != StagePosition1 && name == "Spawned Fighter(Clone)6")
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-7.2f, 4.24f, 0), velocity);
                return;
            }
            if (transform.position != StagePosition2 && name == "Spawned Fighter(Clone)7")
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-6f, 2.8f, 0), velocity);
                return;
            }
            if (transform.position != StagePosition3 && name == "Spawned Fighter(Clone)8")
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-4.8f, 4.31f, 0), velocity);
                return;
            }
            if (transform.position != StagePosition4 && name == "Spawned Fighter(Clone)9")
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(4.8f, 4.22f, 0), velocity);
                return;
            }
            if (transform.position != StagePosition5 && name == "Spawned Fighter(Clone)10")
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(6f, 2.8f, 0), velocity);
                return;
            }
            if (transform.position != StagePosition6 && name == "Spawned Fighter(Clone)11")
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(7.2f, 4.26f, 0), velocity);
                return;
            }
        }

        if (_canmove != false && _boss._grandEntranceComplete == true)
        {

            if (name == "Spawned Fighter(Clone)0" || name == "Spawned Fighter(Clone)1" || name == "Spawned Fighter(Clone)2" || name == "Spawned Fighter(Clone)3" || name == "Spawned Fighter(Clone)4" || name == "Spawned Fighter(Clone)5")
            {
                EnemyMovement();
                if (Time.time > _canFire)
                {
                    EnemyFireSequence();
                }
            }
            else if (name == "Spawned Fighter(Clone)6" || name == "Spawned Fighter(Clone)7" || name == "Spawned Fighter(Clone)8" || name == "Spawned Fighter(Clone)9" || name == "Spawned Fighter(Clone)10" || name == "Spawned Fighter(Clone)11")
            {
            }
        }
        if (_canmove != false && _boss.FirstSequenceComplete == true)
        {
            if (name == "Spawned Fighter(Clone)6" || name == "Spawned Fighter(Clone)7" || name == "Spawned Fighter(Clone)8" || name == "Spawned Fighter(Clone)9" || name == "Spawned Fighter(Clone)10" || name == "Spawned Fighter(Clone)11")
            {
                EnemyMovement();

                if (Time.time > _canFire)
                {
                    EnemyFireSequence();
                }
            }
        }
    }

   private void EnemyMovement()
    {
        if (_targetplayer != null)
        {
            target = _targetplayer.transform.position;
        }
        float distance = Vector3.Distance(target, transform.position);
        float velocity = _speed * Time.deltaTime;

       // Vector2 direction = PlayerTarget.position - transform.position;
       // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
       // Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
       // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, velocity);


        
            
            Debug.Log("setting player as target");


        if (_targetplayer != null && distance <= 8f)
        {
            Debug.Log("Moving towards Player");
            transform.position = Vector3.MoveTowards(transform.position, target, velocity);
        }
        else
        {
            Debug.Log("Moving");
            
            switch (_enemyMoveType)
            {

                case 0:
                    transform.Translate(Vector3.down * velocity);
                    break;
                case 1:
                    Debug.Log("diag left");
                    transform.Translate(-velocity * 0.75f, -velocity * 0.75f, 0);
                    break;
                case 2:
                    Debug.Log("diag right");
                    transform.Translate(velocity * 0.75f, -velocity * 0.75f, 0);
                    break;
                case 3:
                    Debug.Log("Moving Cosine");
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

        /*   transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World); transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);  */


    }

    private void EnemyFireSequence()
    {
        _canFire = Time.time + Random.Range(1f,3f);
        Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
        
    }
    public void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("collision triggered");

        if (other.tag == "Player")
        {

            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();

                if (_ImmunityStart <= Time.time)
                {
                   // _ImmunityStart = Time.time + ImmunityDuration;
                    _canmove = false;
                    _audioSource.Play();
                    Instantiate(_explosionPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                    transform.gameObject.tag = "Dead Enemy";
                    Destroy(GetComponent<SpriteRenderer>(), 0.25f);
                    foreach (Transform child in ChildHolder.transform)
                        Destroy(child.gameObject.GetComponent<SpriteRenderer>(), 0.25f);
                    Destroy(GetComponent<Collider2D>());
                    Destroy(this.gameObject, 2.5f);
                }
            }
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_ImmunityStart <= Time.time)
            {
             //   _ImmunityStart = Time.time + ImmunityDuration;
                _canmove = false;
                _audioSource.Play();
                Instantiate(_explosionPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                transform.gameObject.tag = "Dead Enemy";
                Destroy(GetComponent<SpriteRenderer>(), 0.25f);
                foreach (Transform child in ChildHolder.transform)
                    Destroy(child.gameObject.GetComponent<SpriteRenderer>(), 0.25f);
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.5f);
            }
        }

        if (other.tag == "SuperPower")
        {
            if (_ImmunityStart <= Time.time)
            {
            //    _ImmunityStart = Time.time + ImmunityDuration;
                _canmove = false;
                Instantiate(_explosionPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                _audioSource.Play();
                transform.gameObject.tag = "Dead Enemy";
                Destroy(GetComponent<SpriteRenderer>(), 0.25f);
                foreach (Transform child in ChildHolder.transform)
                    Destroy(child.gameObject.GetComponent<SpriteRenderer>(), 0.25f);
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.5f);
            }
        }
    }
}
