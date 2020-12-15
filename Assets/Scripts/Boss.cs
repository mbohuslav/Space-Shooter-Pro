using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private Spawned_Fighters _spawnedFightersScript;

    [SerializeField]
    private Animator _bossDeathAnim;
    [SerializeField]
    private Animator[] Explosion;
    private Animator _laserAnim;

    [SerializeField]
    private GameObject[] Colliders;
    public GameObject[] _laserVisualizerLeft;
    public GameObject[] _laserVisualizerRightt;
    public GameObject _shieldVisualizer;
    public GameObject Thrusters;
    public GameObject[] ArmsImage;

    private GameObject _targetplayer;

    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _BossExplosion;
    [SerializeField]
    private AudioClip _ShieldStartSound;
    [SerializeField]
    private AudioClip _LaserBeam;
    [SerializeField]
    AudioClip[] _bossArmSound;
  //  private GameObject BossMusic; 



    private float _speed = 3f;
    private int _shieldStrength = 10;
    private int _hitpoints = 21;
    private float _ImmunityStart = 0f;
    private float ImmunityDuration = 0.24f;

    // private bool _shieldsActive = false;

    private Color _startingColor;
    private Color _targetColor;
   

    private float _FireRate = 2f;
    private float _canFire = -1f;

    private bool _canfirelaser = false;

    public bool _grandEntranceComplete = false;
    public bool FirstSequenceComplete = false;
    private bool _firstSequenceStart = false;  // starts when shield dies ;  more minions appear
    private bool _secondSequenceStart = false; // starts when 1/2 hp ; Ship Arm Extensions fly in

    private bool didFirstPhaseOccur = false;
    private bool didSecondPhaseOccur = false;

    RaycastHit2D hit;
    private Vector3 _boxCastSize = new Vector3(3, 6, 0);
    private Vector3 _boxCastSizeLarge = new Vector3(7, 6, 0);
    //color blink when close to death
    private SpriteRenderer _bossRenderer;
    [SerializeField]
    private SpriteRenderer[] _bossArmsRenderer;
    private Color _clearColor;
    private Color _redColor;
    private bool _bossBlinking = false;

    //Boss Movement Pattern with Sin and Cos
    Vector3 pos;
    Vector3 axis;
    Vector3 axis2;
    float frequency = 1.3f;
    float magnitude = 1.2f;
    float frequencySin = 0.8f;
    float magnitudeSin = 8.5f;
    public bool BossInSequence;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _targetplayer = GameObject.FindWithTag("Player");
        _bossRenderer = GetComponent<SpriteRenderer>();
        _laserAnim = GetComponent<Animator>();

        //Enemy movement Axis and Positon
        pos = new Vector3(0, 2.5f, 0); //transform.position;
        axis = transform.up;
        axis2 = transform.right;

        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL");
        }
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }


        //transform.position = new Vector3(0, 10.7f, 0); //start position for Boss
        _shieldVisualizer.SetActive(false);
   
        _startingColor = new Color32(233, 214, 116, 255);   //shield
        _targetColor = new Color32(233, 214, 116, 190);     // blink rooutine

        //color blink routine:
        _clearColor = Color.white;
        _redColor = Color.red;

        StartCoroutine(GrandEntrance());
    }

    void Update()
    {
        BossMovement();
       
    }

    public void BossMovement()
    {
        float velocity = _speed * Time.deltaTime;


        if (_grandEntranceComplete == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 2.5f, 0f), 1.2f * Time.deltaTime);
        }

        else if (BossInSequence == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 2.5f, 0f), 3.6f * Time.deltaTime);
            if (transform.position == new Vector3(0, 2.5f, 0f) && _secondSequenceStart == false)
            {
                _firstSequenceStart = false;
            }
            if (transform.position == new Vector3(0, 2.5f, 0f) && _firstSequenceStart == false)
            {
                _secondSequenceStart = false;
            }
        }
        
        if (_grandEntranceComplete == true)
            
        {
            Debug.Log("SinMovement");
                transform.position = pos + (axis2 * Mathf.Sin(Time.time * frequencySin) * magnitudeSin) - (axis * Mathf.Cos(Time.time * frequency) * magnitude);
        }


        if (didFirstPhaseOccur == false && _grandEntranceComplete == true && _shieldStrength <= 0)
        {
            Debug.Log("1st phase occurs");
            StartCoroutine(FirstSequence());
            didFirstPhaseOccur = true;
        }

        if (didSecondPhaseOccur == false && didFirstPhaseOccur == true && _hitpoints <=10)
        {
            Debug.Log("2nd phase occurs");
            StartCoroutine(SecondSequence());
            didSecondPhaseOccur = true;
        }
     /*   if (_grandEntranceComplete == true)
        {
            if (_firstSequenceStart != true || _secondSequenceStart != true)
            {
                transform.position = pos + (axis2 * Mathf.Sin(Time.time * frequencySin) * magnitudeSin) - (axis * Mathf.Cos(Time.time * frequency) * magnitude);
            }
        }
       */         
    }

    IEnumerator GrandEntrance()
    {
        
        for (int i = 0; i < 10; i++)
        {
            Colliders[i].GetComponent<Collider2D>().enabled = false;
        }
        yield return new WaitForSeconds(7f);
        _spawnManager.MinionsSpawn();
        yield return new WaitForSeconds(5f);
        _shieldVisualizer.SetActive(true);
        _audioSource.clip = _ShieldStartSound;
        _audioSource.volume = 0.25f;
        _audioSource.Play();
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 10; i++)
            {
                Colliders[i].GetComponent<Collider2D>().enabled = true;
            }
        _grandEntranceComplete = true;
       // yield return new WaitForSeconds(0.3f);
        _canfirelaser = true;


    }

    IEnumerator FirstSequence() 
    {
        BossInSequence = true;
        Debug.Log("Boss Sequence Starts");
        Debug.Log("First Sequence");
       // _canfirelaser = false;
        for (int i = 0; i < 10; i++)
        {
            Colliders[i].GetComponent<Collider2D>().enabled = false;
        }
        _firstSequenceStart = true;
        yield return new WaitForSeconds(2.5f);
        _spawnManager.MinionsSpawn();
        yield return new WaitForSeconds(5f);
        Debug.Log("Colliders back on");
        for (int i = 0; i < 10; i++)
        {
            Colliders[i].GetComponent<Collider2D>().enabled = true;
        }
        FirstSequenceComplete = true;
        yield return new WaitForSeconds(0.3f);
       // _canfirelaser = true;
        BossInSequence = false;
        Debug.Log("Boss Sequence Ends");
    }

    IEnumerator SecondSequence()
    {
        BossInSequence = true;
        Debug.Log("Boss2 Sequence Starts");
 
         //_audioSource.mute = true;
        Debug.Log("2nd Sequence");
        //_canfirelaser = false;
        _secondSequenceStart = true;
        for (int i = 0; i < 10; i++)
        {
            Colliders[i].GetComponent<Collider2D>().enabled = false;
        }
        yield return new WaitForSeconds(3f);
        GetComponent<Animator>().SetTrigger("2nd Sequence");
        yield return new WaitForSeconds(6.5f);
      //  _audioSource.mute = false;
        _audioSource.clip = _bossArmSound[1];
        _audioSource.volume = 0.30f;
        _audioSource.Play();
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < 10; i++)
        {
            Colliders[i].GetComponent<Collider2D>().enabled = true;
        }
        yield return new WaitForSeconds(0.3f);
       // _canfirelaser = true;
        BossInSequence = false;
        Debug.Log("Boss2 Sequence Ends");
    }
    private void FixedUpdate()
    {
        DetectPlayer();
        StartCoroutine(FireLasers());
    }

    private void DetectPlayer()
    {
        if (didSecondPhaseOccur == false)
        {
            int layerMask = LayerMask.GetMask("Default");
            hit = Physics2D.BoxCast(transform.position, _boxCastSize, 0f, Vector2.down, Mathf.Infinity, layerMask);
            Debug.Log("Raycast cast");
        }
        else
        {
            int layerMask = LayerMask.GetMask("Default");
            hit = Physics2D.BoxCast(transform.position, _boxCastSizeLarge, 0f, Vector2.down, Mathf.Infinity, layerMask);
            Debug.Log("Raycast cast");
        }
    }


    IEnumerator FireLasers()
    {
              
               if (_canfirelaser == true && hit.collider != null && hit.collider.tag == "Player" && didSecondPhaseOccur == false)
                 {
                    _canfirelaser = false;
                     yield return new WaitForSeconds(0.2f);
                      _laserAnim.SetTrigger("FireWeapon");
                      Debug.Log("FireWeapon");
                      _audioSource.clip = _LaserBeam;
                      _audioSource.volume = 0.250f;
                     _audioSource.Play();
                    yield return new WaitForSeconds(1.5f);
                    _laserAnim.SetTrigger("WeaponPowerDown");
                    Debug.Log("WeaponCoolDown");
                    yield return new WaitForSeconds(2f);
                    _canfirelaser = true;
                  }
               if (_canfirelaser == true && hit.collider != null && hit.collider.tag == "Player" && didSecondPhaseOccur == true)
                 {
                    _canfirelaser = false;
                  yield return new WaitForSeconds(0.2f);
                 _laserAnim.SetTrigger("FireAllWeapon");
                  Debug.Log("FireAllWeapon");
                 _audioSource.clip = _LaserBeam;
                 _audioSource.volume = 0.25f;
                 _audioSource.Play();
                 yield return new WaitForSeconds(1.5f);
                 _laserAnim.SetTrigger("AllWeaponPowerDown");
                 Debug.Log("AllWeaponCoolDown");
                  yield return new WaitForSeconds(2f);
                 _canfirelaser = true;
               
            if(_canfirelaser == false)
            {
                yield return new WaitForSeconds(0.01f);
            }

        }


    }
     
    
    
    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
              

                

                player.Damage();
               
                if (_shieldStrength > 0 && _ImmunityStart <= Time.time)
                {
                    _ImmunityStart = Time.time + ImmunityDuration;
                    _shieldStrength -= 1;
                    _audioSource.clip = _BossExplosion;
                    _audioSource.volume = 0.08f;
                    _audioSource.Play();
                    StartCoroutine(Shieldblink());
                }

                if (_shieldStrength == 0 && _shieldVisualizer == true)
                {
                    _firstSequenceStart = true;
                    _shieldVisualizer.SetActive(false);
                }

                if (_shieldStrength <= 0  && _ImmunityStart <= Time.time) 
                {
                    _ImmunityStart = Time.time + ImmunityDuration;
                    _hitpoints -= 1;
                    _audioSource.clip = _BossExplosion;
                    _audioSource.volume = 0.15f;
                    _audioSource.Play();
                }
                
                if (_hitpoints == 10 && _ImmunityStart <= Time.time)
                {
                    _secondSequenceStart = true;
                }
              


                if (_shieldStrength <= 0 && _hitpoints <= 0)
                {
                    _canfirelaser = false;
                    _bossBlinking = false;
                    StartCoroutine(DestructionSequence());
                }
            }
        }

        if (other.tag == "Laser")
        {

            Destroy(other.gameObject);
            
            if (_shieldStrength > 0 && _ImmunityStart <= Time.time)
            {
                _ImmunityStart = Time.time + ImmunityDuration;
                _shieldStrength -= 1;
                _audioSource.clip = _BossExplosion;
                _audioSource.volume = 0.08f;
                _audioSource.Play();
                StartCoroutine(Shieldblink());
            }

            if (_shieldStrength == 0 && _shieldVisualizer == true)
            {
                _firstSequenceStart = true;
                _shieldVisualizer.SetActive(false);
            }


            if (_shieldStrength <= 0 && _ImmunityStart <= Time.time)
            {
                _ImmunityStart = Time.time + ImmunityDuration;
                _shieldVisualizer.SetActive(false);
                _hitpoints -= 1;
                _audioSource.clip = _BossExplosion;
                _audioSource.volume = 0.15f;
                _audioSource.Play();
            }

            if (_hitpoints == 10 && _ImmunityStart <= Time.time)
            {
                _secondSequenceStart = true;
            }

            if (_shieldStrength <= 0 && _hitpoints <= 0)
            {
                _canfirelaser = false;
                _bossBlinking = false;
                StartCoroutine(DestructionSequence());
            }
        }

        if (other.tag == "SuperPower")
        {
            if (_shieldStrength == 0 && _shieldVisualizer == true)
            {
                _firstSequenceStart = true;
                _shieldVisualizer.SetActive(false);
            }

            if (_shieldStrength > 0 && _ImmunityStart <= Time.time)
            {
                _ImmunityStart = Time.time + ImmunityDuration;
                _shieldStrength -= 1;
                _audioSource.clip = _BossExplosion;
                _audioSource.volume = 0.08f;
                _audioSource.Play();
                StartCoroutine(Shieldblink());
            }

            if (_shieldStrength == 0 && _shieldVisualizer == true)
            {
                _firstSequenceStart = true;
                _shieldVisualizer.SetActive(false);
            }

            if (_shieldStrength <= 0 && _ImmunityStart <= Time.time)
            {
                _ImmunityStart = Time.time + ImmunityDuration;
                _shieldVisualizer.SetActive(false);
                _hitpoints -= 1;
                _audioSource.clip = _BossExplosion;
                _audioSource.volume = 0.15f;
                _audioSource.Play();
            }

            if (_hitpoints == 10 && _ImmunityStart <= Time.time)
            {
                _secondSequenceStart = true;
            }

            if (_shieldStrength <= 0 && _hitpoints <= 0)
            {
                _canfirelaser = false;
                _bossBlinking = false;
                StartCoroutine(DestructionSequence());
            }
        }

        if (_hitpoints <= 5 && _ImmunityStart <= Time.time)
        {
            _bossBlinking = true;
            Debug.Log("Color Blinking when Health low!!!!");
        //    StartCoroutine(ColorChange());     //Not currently working!
        }

    }
    IEnumerator Shieldblink()
    {
        _shieldVisualizer.GetComponent<SpriteRenderer>().color = _targetColor;
        yield return new WaitForSeconds(0.1f);
        _shieldVisualizer.GetComponent<SpriteRenderer>().color = _startingColor;
    }
   /* IEnumerator ColorChange()  // Boss flashes red when down to 5 hitpoints
    {
       if (_bossRenderer == null)
        {
        }

        while (_bossBlinking == true)
        {
            _bossRenderer.color = _redColor;
            _bossArmsRenderer[0].color = _redColor;
            _bossArmsRenderer[1].color = _redColor;
            yield return new WaitForSeconds(0.25f);
            _bossRenderer.color = _clearColor;
            _bossArmsRenderer[0].color = _clearColor;
            _bossArmsRenderer[1].color = _clearColor;
            yield return new WaitForSeconds(0.25f);
        }
    
    }
   */

    IEnumerator DestructionSequence()
    {
        for (int i = 0; i < 10; i++)
        { 
            Colliders[i].GetComponent<Collider2D>().enabled = false; 
        }
        Explosion[3].SetTrigger("OnEnemyDamage");
        _audioSource.clip = _BossExplosion;
        _audioSource.volume = 0.50f;
        _audioSource.Play();
        yield return new WaitForSeconds(1f);
        Explosion[1].SetTrigger("OnEnemyDamage");
        _audioSource.clip = _BossExplosion;
        _audioSource.volume = 0.25f;
        _audioSource.Play();
        yield return new WaitForSeconds(1f);
        Explosion[2].SetTrigger("OnEnemyDamage");
        _audioSource.clip = _BossExplosion;
        _audioSource.volume = 0.25f;
        _audioSource.Play();
        yield return new WaitForSeconds(0.75f);
        Explosion[3].SetTrigger("OnEnemyDamage");
        _audioSource.clip = _BossExplosion;
        _audioSource.volume = 0.25f;
        _audioSource.Play();
        yield return new WaitForSeconds(0.75f);
        Explosion[4].SetTrigger("OnEnemyDamage");
        _audioSource.clip = _BossExplosion;
        _audioSource.volume = 0.25f;
        _audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        Explosion[5].SetTrigger("OnEnemyDamage");
        _audioSource.clip = _BossExplosion;
        _audioSource.volume = 0.25f;
        _audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        Explosion[6].SetTrigger("OnEnemyDamage");
        _audioSource.clip = _BossExplosion;
        _audioSource.volume = 0.25f;
        _audioSource.Play();
        yield return new WaitForSeconds(0.25f);
        Explosion[7].SetTrigger("OnEnemyDamage");
        Explosion[0].SetTrigger("OnEnemyDamage");
        _audioSource.clip = _BossExplosion;
        _audioSource.volume = 0.40f;
        _audioSource.Play();
        yield return new WaitForSeconds(0.75f);
        Explosion[8].SetTrigger("OnEnemyDamage");
        yield return new WaitForSeconds(0.10f);
        Explosion[9].SetTrigger("OnEnemyDamage");
        _audioSource.clip = _BossExplosion;
        _audioSource.volume = 0.40f;
        _audioSource.Play();
        Thrusters.SetActive(false);
        yield return new WaitForSeconds(0.15f);
        ArmsImage[0].SetActive(false);
        yield return new WaitForSeconds(0.10f);
        _bossDeathAnim.SetTrigger("OnEnemyDeath");
        _audioSource.clip = _BossExplosion;
        _audioSource.volume = 0.60f;
        _audioSource.Play();
        yield return new WaitForSeconds(0.10f);
        ArmsImage[1].SetActive(false);
        yield return new WaitForSeconds(0.25f);
        Destroy(GetComponent<SpriteRenderer>());
        Destroy(this.gameObject, 3.5f);
    }
}
  /*  private void FixedUpdate()
   {
        if (BossDead = true)
        {
            StartCoroutine(DestructionSequence());
        }
    }
}*/

// ExplosionLocation;
// ExplosionLocationAnim;

//   public Collider2D GroundCol;
//public LayerMask WhatIsGround;
// ...

//private void FixedUpdate()
//{
 //   bGrounded = GroundCol.IsTouchingLayers(WhatIsGround)

/*  void OnCollisionEnter(Collision collision)
{
    ContactPoint contact = collision.contacts[0];
    Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
    Vector3 position = contact.point;
    Instantiate(explosionPrefab, position, rotation);
}
*/