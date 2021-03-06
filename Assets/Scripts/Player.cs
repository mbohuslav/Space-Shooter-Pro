﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
     [SerializeField]
    private float _speed = 7f;
    [SerializeField]
    private float _speedMultiplier = 2.25f;
    
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    private float _ImmunityStart = 0f;
    public float ImmunityDuration = 2f;
    
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    public int _score;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    
    private bool _tripleShotActive = false;
    private bool _speedBoostActive = false;
    private bool _shieldsActive = false;

    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _leftEngineVisualizer, _rightEngineVisualizer;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _playerExplosion;
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private GameObject _explosionPrefab;
    public GameObject playerChildHolder;
    // bool resetPowerUp;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -4.5f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        
        

        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL."); 
        }

        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
        if (_audioSource == null)
        {
            Debug.LogError(" The Audio Source on Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }

}

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _lives >0)
        {
            FireLaser();
        }


    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //new Vector3(1, 0, 0) * 0 * 3.5f * real time
        // BASIC
        // transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        // transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        //BETTER
        // transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

        // BEST
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
       if (_speedBoostActive == false)
        { 
          transform.Translate(direction * _speed * Time.deltaTime);
        }
       else
        {
            transform.Translate(direction * (_speedMultiplier * _speed) * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.8f, 0), 0);

        if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
        else if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
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
        _audioSource.Play();
       
     

    }

    public void Damage()
    {
        _audioSource.clip = _playerExplosion;
        _audioSource.Play();
        
        if (_shieldsActive == true)
        {
            _shieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        if (_ImmunityStart <= Time.time && _lives >0)
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
       
        if (_lives <1)
        {
            _spawnManager.OnPlayerDeath();
            _audioSource.clip = _playerExplosion;
            _audioSource.Play();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _speed = 0;
            Destroy(GetComponent<SpriteRenderer>(), 0.25f);
            foreach (Transform child in playerChildHolder.transform)
            {
                  Destroy(child.gameObject);
             }
            Destroy(this.gameObject, 3.018f);
        }
        
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }    
   
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speedBoostActive = false;
    }

   public void ShieldsActive()
    {   
        _shieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
            
    }






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
}