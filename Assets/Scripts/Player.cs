using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6f;
    private float _maxSpeed = 12f;
    private float _timeZerotoMax = 4f;
    private float _accelRatePerSec;
    private float _velocity;
    [SerializeField]
    private float _speedMultiplier = 3f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _missileFireRate = 0.3f;
    private float _canFire = -1f;
    [SerializeField]
    private int _ammoCount = 15;
    private float _ammoRegen = 1.75f;
    private float _canRegen = -1;
    private float _ImmunityStart = 0f;
    public float ImmunityDuration = 0.75f;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    public int _score;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private Main_Camera _mainCamera;

    public bool _homingMissileActive = false;
    private bool _tripleShotActive = false;
    private bool _speedBoostActive = false;
    private bool isThrusterActive = true;
    private bool _shieldsActive = false;
    public int _shieldLevel = 0;
    [SerializeField]
    private GameObject[] _shieldVisualizer;
    [SerializeField]
    private GameObject[] _thrusterVisualizer;
    [SerializeField]
    private GameObject _superPower;
    private Animator _animSuperPower;
    [SerializeField]
    private GameObject _sparks;
    private Animator _animSparks;
    private Enemy _enemy;
    float _circleRadius = 0f;       //not needed anymore
    float _circleRadiusMax = 1.3f;  //not needed anymore
    bool _superPowerActive = true;

    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _leftEngineVisualizer, _rightEngineVisualizer;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _playerExplosion;
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _noAmmoSound;
    [SerializeField]
    private AudioClip _MissileSound;
    [SerializeField]
    private AudioClip _superPowerSound;
    [SerializeField]
    private GameObject _explosionPrefab;
    public GameObject playerChildHolder;
    [SerializeField]
    private GameObject _homingMissilePrefab;

    //Color change variables
    private bool _playerBlinking = false;
    private Color _targetColor;
    private Color _startingColor;
    [SerializeField]
    private SpriteRenderer _playerRenderer;
   
    public Vector3 PlayerTarget; // target for PowerUp Pickup



    void Start()
    {
        transform.position = new Vector3(0, -4.5f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _accelRatePerSec = _maxSpeed / _timeZerotoMax;
        _mainCamera = GameObject.Find("Main Camera").GetComponent<Main_Camera>();
        _playerRenderer = GetComponent<SpriteRenderer>();
        _animSuperPower = _superPower.GetComponent<Animator>();
        _animSparks = _sparks.GetComponent<Animator>();

        // _enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
        if (_audioSource == null)
        {
            Debug.LogError(" The Audio Source on Player is NULL");
        }
        if (_mainCamera == null)
        {
            Debug.LogError(" the Main Camera is NULL");
        }
        if (_playerRenderer == null)
        {
            Debug.LogError(" Sprite Renderer is NULL");
        }

        else
        {
            _audioSource.clip = _laserSound;
        }

        //Color blinking routine setup
        _startingColor = Color.white;
        _targetColor = Color.red;

    
    }

    void Update()
    {
        CalculateMovement();
      
        

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _lives > 0)
        {
            FireLaser();
        }

        if (Time.time > _canRegen & _ammoCount < 15)
        {
            _ammoCount += 1;
            _canRegen = Time.time + _ammoRegen;
            _uiManager.UpdateAmmo(_ammoCount);
        }

        if (Input.GetKeyDown(KeyCode.Z) && _superPowerActive == true) //Activate SuperPower
        {
            _superPowerActive = false;
            _uiManager._superPower.SetActive(false);
            GetComponent<CircleCollider2D>().enabled = false;
            _ImmunityStart = Time.time + ImmunityDuration;
            _audioSource.volume = 1f;
            _audioSource.clip = _superPowerSound;
            _audioSource.Play();
            _sparks.SetActive(true);
            _animSparks.SetTrigger("ActivatePower");
            _superPower.SetActive(true);
            StartCoroutine(SuperPowerCooldown());
           Time.timeScale = 0.5f;
           

        }

    }

    IEnumerator SuperPowerCooldown()
    {
        _superPowerActive = false;
        yield return new WaitForSeconds(0.375f);
        
        if (_shieldsActive  == true)
        {
            Debug.Log("shields 0 being activated");
            _shieldVisualizer[2].SetActive(false);
            _shieldVisualizer[1].SetActive(false);
            _shieldVisualizer[0].SetActive(true);
        }
     
        yield return new WaitForSeconds(0.375f);
        _superPower.GetComponent<CircleCollider2D>().radius = 0.25f;
        _animSuperPower.SetTrigger("ActivatePower");
        _shieldVisualizer[0].SetActive(false);
        _shieldLevel = 0;
        _shieldsActive = false;
        _sparks.SetActive(false);
        yield return new WaitForSeconds(0.15f);
        Time.timeScale = 1f;
        _superPower.GetComponent<CircleCollider2D>().radius = 0.5f;
        yield return new WaitForSeconds(0.15f);
        _superPower.GetComponent<CircleCollider2D>().radius = 0.85f;
        yield return new WaitForSeconds(0.15f);
        _superPower.GetComponent<CircleCollider2D>().radius = _circleRadiusMax;
        yield return new WaitForSeconds(1f);
        _circleRadius = 0;
        _superPower.GetComponent<CircleCollider2D>().radius = _circleRadius;
        _superPower.SetActive(false);
        GetComponent<CircleCollider2D>().enabled = true;
        yield return new WaitForSeconds(10f);
        _uiManager._superPower.SetActive(true);
        _superPowerActive = true;
    }
  /*  private void Ontriggerstay(Collider2D other)
    {
        if (other.tag == "PowerUp" && Input.GetKeyDown(KeyCode.C))
        {
            PowerUp Powerup = other.transform.GetComponent<PowerUp>();

            if (Powerup != null)
            {
                other.transform.position = Vector3.MoveTowards(transform.position, PlayerTarget, _speed * Time.deltaTime);
            }
        }

    }       
                    
  */            
            
               
           
        





        //          I am checking the distance between the enemy and the player Vector3.Distance(transform.position, _player.transform.position) < 4f
        // and use MoveTowards to process movements.
        // transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
    
    



    public void ThrusterActive(bool active)
    {
        isThrusterActive = active;
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (GetComponent<Collider2D>() != null)
        {
            if (_playerBlinking == true)
            {
                transform.Translate(direction * 2.0f * Time.deltaTime);
            }
            else
            {
                if (_speedBoostActive == false)
                {
                    if (Input.GetKey(KeyCode.LeftShift) == true && isThrusterActive == true)
                    {
                        _velocity += _accelRatePerSec * Time.deltaTime;
                        _velocity = Mathf.Min(_velocity, _maxSpeed);
                        transform.Translate(direction * (_speed + _velocity) * Time.deltaTime);

                        _thrusterVisualizer[1].SetActive(true);
                        _thrusterVisualizer[0].SetActive(false);
                    }

                    if (Input.GetKeyUp(KeyCode.LeftShift) == true || isThrusterActive == false)
                    {
                        _velocity = 0;
                        transform.Translate(direction * (_speed + _velocity) * Time.deltaTime);    
                        _thrusterVisualizer[0].SetActive(true);
                        _thrusterVisualizer[1].SetActive(false);
                    }

                    else
                    {
                        transform.Translate(direction * _speed * Time.deltaTime);
                    }
                }
            }
            if (_speedBoostActive == true)
            {
                transform.Translate(direction * (_speedMultiplier * _speed) * Time.deltaTime);
                _thrusterVisualizer[1].SetActive(true);
                _thrusterVisualizer[0].SetActive(false);
            }
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5.5f, 5.10f), 0);

        if (transform.position.x <= -13.4f)
        {
            transform.position = new Vector3(13.4f, transform.position.y, 0);
        }
        else if (transform.position.x >= 13.4f)
        {
            transform.position = new Vector3(-13.4f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        if (_homingMissileActive == true && _playerBlinking == false)
        {
            
             _canFire = Time.time + _missileFireRate;
                Instantiate(_homingMissilePrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                _audioSource.clip = _MissileSound;
                _audioSource.volume = 0.07f;
                _audioSource.Play();
         }

        else
        {
             if (_ammoCount > 0 && _playerBlinking == false) 
             {
                  _ammoCount -= 1;
                  _canFire = Time.time + _fireRate;

                  if (_tripleShotActive == true)
                       {
                           Instantiate(_tripleShotPrefab, transform.position + new Vector3(-1.34f, 0.82f, 0), Quaternion.identity);
                       }

                  else
                  {
                       Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
                  }

              _audioSource.clip = _laserSound;
              _audioSource.volume = 0.25f;
              _audioSource.Play();
             _uiManager.UpdateAmmo(_ammoCount);
             }

             if (_ammoCount <=0 || _playerBlinking == true)
             {
                   _audioSource.clip = _noAmmoSound;
                   _audioSource.volume = 0.07f;
                   _audioSource.Play();
             }

        }
    }

    public void Damage()
    {

        
        
            _audioSource.clip = _playerExplosion;
            _audioSource.Play();
            _audioSource.volume = 0.15f;
            _mainCamera.shake();
        

        if (_shieldsActive == true && _ImmunityStart <= Time.time)
        {
            _shieldLevel -= 1;
            _ImmunityStart = Time.time + ImmunityDuration;

            switch (_shieldLevel)
            {

                case 0:
                    _shieldVisualizer[0].SetActive(false);
                    _shieldsActive = false;
                    break;
                case 1:
                    _shieldVisualizer[1].SetActive(false);
                    _shieldVisualizer[0].SetActive(true);
                    break;
                case 2:
                    _shieldVisualizer[2].SetActive(false);
                    _shieldVisualizer[1].SetActive(true);
                    break;
                default:
                    Debug.Log("Not a valid Shield Strength");
                    break;
            }
            return;
        }

        if (_ImmunityStart <= Time.time && _lives > 0)
        {

            _lives -= 1; //_lives --;   same function

            int randomEngineVisualizer = Random.Range(0, 2);
            switch (randomEngineVisualizer)
            {
                case 0:
                    if (_leftEngineVisualizer.activeSelf == false)
                    {
                        _leftEngineVisualizer.SetActive(true);
                    }
                    else
                    {
                        _rightEngineVisualizer.SetActive(true);
                    }
                    break;
                case 1:
                    if (_rightEngineVisualizer.activeSelf == false)
                    {
                        _rightEngineVisualizer.SetActive(true);
                    }
                    else
                    {
                        _leftEngineVisualizer.SetActive(true);
                    }
                    break;
                default:
                    Debug.Log("Not a Valid Engine");
                    break;
            }

            _uiManager.Updatelives(_lives);
            _ImmunityStart = Time.time + ImmunityDuration;
        }

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _audioSource.clip = _playerExplosion;
            _audioSource.volume = 0.25f;
            _audioSource.Play();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _speed = 0;
            // Destroy(GetComponent<SpriteRenderer>(), 0.25f);
            StartCoroutine(DestroyPlayerSpriteRoutine());
            Destroy(GetComponent<CircleCollider2D>());
            foreach (Transform child in playerChildHolder.transform)
                Destroy(child.gameObject.GetComponent<SpriteRenderer>());
            Destroy(this.gameObject, 3.018f);
        }
        IEnumerator DestroyPlayerSpriteRoutine()
        {
            yield return new WaitForSeconds(0.25f);
            _playerRenderer.enabled = false;
        }
    }
    public void HomingMissileActive()
    {
        _homingMissileActive = true;
        StartCoroutine(HomingMissilePowerDownRoutine());
    }
    IEnumerator HomingMissilePowerDownRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        _homingMissileActive = false;
    }



    public void TripleShotActive()
    {
        _tripleShotActive = true;
        _ammoCount += 15;
        _ammoCount = Mathf.Min(15, _ammoCount);
        _uiManager.UpdateAmmo(_ammoCount);

        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        _tripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        if (_playerBlinking == false)
        {
            _speedBoostActive = true;
            StartCoroutine(SpeedBoostPowerDownRoutine());
        }

    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
            yield return new WaitForSeconds(5.0f);
            _speedBoostActive = false;
            _thrusterVisualizer[1].SetActive(false);
            _thrusterVisualizer[0].SetActive(true);
    }

    public void ShieldsActive()
    {
        _shieldsActive = true;
        _shieldVisualizer[0].SetActive(false);
        _shieldVisualizer[1].SetActive(false);
        _shieldVisualizer[2].SetActive(true);
        _shieldLevel = 3;

    }

    public void AmmoActive()
    {
        _ammoCount = 15;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void HealthActive()
    {
        if (_lives == 3)
        {
            _lives = 3;
        }
        else
        {
            _lives += 1;
            _uiManager.Updatelives(_lives);

            if (_leftEngineVisualizer.activeSelf == true && _rightEngineVisualizer.activeSelf == true)
            {
                _leftEngineVisualizer.SetActive(false);
            }

            else if (_leftEngineVisualizer.activeSelf == true && _rightEngineVisualizer.activeSelf == false)
            {
                _leftEngineVisualizer.SetActive(false);
            }

            else if (_leftEngineVisualizer.activeSelf == false && _rightEngineVisualizer.activeSelf == true)
            {
                _rightEngineVisualizer.SetActive(false);
            }
        }
    }
    public void MonkeyActive()
    {
       if (_playerRenderer.enabled == true)
        
        {
            _speed = 2f;
            _fireRate = 100f;
            isThrusterActive = false;
            _uiManager.MonkeyKillThruster();
            _playerBlinking = true;

            StartCoroutine(MonkeyPowerDownRoutine());
            StartCoroutine(ColorChange());
        }
    } 

    IEnumerator MonkeyPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = 6f;
        _fireRate = 0.15f;
        _playerBlinking = false;
        
       
    }

    IEnumerator ColorChange()
    {
        if (_playerRenderer == null)
        {   
        }

        while (_playerBlinking == true)
        {
            _playerRenderer.color = _targetColor;
            yield return new WaitForSeconds(0.25f);
            _playerRenderer.color = _startingColor;
            yield return new WaitForSeconds(0.25f);
        }








      /*  var _playerRenderer = GetComponent<SpriteRenderer>();

        if (_timeLeft <= Time.deltaTime && _timeLeft >0)
             {

                 _playerRenderer.color = _targetColor;
                 _targetColor = new Color(255, Random.Range(0,255) , Random.Range(0, 255));
                 _timeLeft = 1f;
             }
             if (_timeLeft <= 0)
             {
                 _playerRenderer.color = Color.red;
                 _playerBlinking = false;
             }

             else
             {
                 _playerRenderer.color = Color.Lerp(_startingColor, _targetColor, t);
                 _timeLeft -= Time.deltaTime;

             }
         */  

    }

    //public void AddScore(int points)
   // {
   //     _score += points;
    //    _uiManager.UpdateScore(_score);
//
 //   }
//





    /* IEnumerator ShieldsActivePowerDownRoutine()
       {
          yield return new WaitForSeconds(10.0f);
          _shieldsActive = false;
          _shieldVisualizer.SetActive(false);
       } */

/*IEnumerator OnTriggerEnter2D(Collider2D other)
{
    //If the colliding object's tag is "PowerUp"
    //if (other.tag == "Shields")
    {
        //If Power_Up is false, it means the powerup routine is not currently running
        if (!_shieldsActive)
        {
            _shieldsActive = true;
            _shieldVisualizer.SetActive(true);
            float duration = 20f;

            //Takes a timeStamp
            float timeStamp = Time.time;

            //While the current time is less than the timeStamp + the duration of the power up
            while (Time.time < timeStamp + duration)
            {
                //If the flag to reset the powerup is active
                if (resetPowerUp)
                {
                    //Toggle it off
                    resetPowerUp = false;
                    //reset the powerup so that it ends in (current time + 5 seconds);
                    timeStamp = Time.time;
                }
                //Wait for next frame. Do that until the duration is over.
                yield return null;
            }
            //The current time is now greater or equal to the timeStamp + the duration
            //This means the power up should be over
            _shieldsActive = false;
            _shieldVisualizer.SetActive(false);
        }
        //Otherwise, it means that a different powerup routine is already running
        //We're just going to let that other routine know that it should reset the timer
        else
        {
            resetPowerUp = true;
        }
    } 
} */



/*

public void ColorChange()  Alternate cool visuals!!!! to be used with super heating with Beam Weapon later!
{
    var t = Time.deltaTime / _timeLeft;
    var _playerRenderer = GetComponent<SpriteRenderer>();

    if (_timeLeft <= Time.deltaTime && _timeLeft >0)
    {

       _playerRenderer.material.SetColor("_Color", _targetColor);
        _targetColor = new Color(255, Random.Range(0,255) , Random.Range(0, 255));
        _timeLeft = .2f;
    }
    if (_timeLeft <= 0)
    {
        _playerRenderer.material.color = Color.red;
        _playerBlinking = false;
    }

    else
    {
        _playerRenderer.material.color = Color.Lerp(_startingColor, _targetColor, t);
        _timeLeft -= Time.deltaTime;

    }


}
*/


}