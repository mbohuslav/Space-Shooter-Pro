using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _livesprites;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Sprite[] _ammoCount;
    [SerializeField]
    private Image _AmmoImg;
    [SerializeField]
    private Text _newEnemyWave;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _NewEnemyWaveSounds;
    [SerializeField]
    private AudioClip _gameOverSound;
    [SerializeField]
    private Image _thrusterReserve;
    private float _depletionRate = 0.30f;
    private float _ThrusterRecharge = 0.15f;
    private float _ThrusterAccelRecharge = 1f;
    private float _maxThrusterReserve = 1.75f;
    private float _currentThrusterReserve;

    public bool ThrusterActive = true;

    private int _playerScore;

    public float _timestamp;
    private Player _player;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    public int EnemyWave;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        //EnemyWave = 0;

        if (_spawnManager == null)
        {
            Debug.LogError("Player is NULL");
        }

        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _scoreText.text = "Score: " + 0;
        _gameOver.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _newEnemyWave.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _currentThrusterReserve = _maxThrusterReserve;
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }

    }
    public void UpdateAmmo(int currentAmmo)
    {
        _AmmoImg.sprite = _ammoCount[currentAmmo];

        if (currentAmmo == 0)
        {
        }
    }


    public void Updatelives(int currentlives)
    {
        _LivesImg.sprite = _livesprites[currentlives];

        if (currentlives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        // EnemyWave = 0;
        _gameManager.GameOver();
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
        _audioSource.clip = _gameOverSound;
        _audioSource.volume = 1f;
        _audioSource.Play();
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOver.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            _gameOver.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.4f);

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) == true && ThrusterActive == true && _currentThrusterReserve > 0)
        {
            _player.ThrusterActive(true);
            _currentThrusterReserve -= _depletionRate * Time.deltaTime;
            _currentThrusterReserve = Mathf.Clamp(0, _currentThrusterReserve, 0);
            _thrusterReserve.rectTransform.localScale = new Vector3(_currentThrusterReserve, 0.60f, 1);
        }
        if (Input.GetKey(KeyCode.LeftShift) == false && ThrusterActive == true && _currentThrusterReserve > 0)
        {
            _currentThrusterReserve += _ThrusterRecharge * Time.deltaTime;
            _currentThrusterReserve = Mathf.Min(_maxThrusterReserve, _currentThrusterReserve);
            _thrusterReserve.rectTransform.localScale = new Vector3(_currentThrusterReserve, 0.60f, 1);
        }

        if (_currentThrusterReserve <= 0)
        {
            _player.ThrusterActive(false);
            StartCoroutine(ThrusterCoolDown());
        }

    }

    public void MonkeyKillThruster()
    {
        StartCoroutine(ThrusterCoolDown());
    }

    IEnumerator ThrusterCoolDown()
    {
        ThrusterActive = false;
        yield return new WaitForSeconds(5.0f);
        ThrusterActive = true;
        ThrusterRefill();
    }

    private void ThrusterRefill()
    {
        _currentThrusterReserve += (_ThrusterAccelRecharge * Time.deltaTime);
        _currentThrusterReserve = Mathf.Min(_maxThrusterReserve, _currentThrusterReserve);
        _thrusterReserve.rectTransform.localScale = new Vector3(_currentThrusterReserve, 0.60f, 1);
    }

    public void UpdateScore(int points)
    {
        _playerScore += points;
         
        _scoreText.text = "Score: " + _playerScore;

        if (_playerScore >= 50 && EnemyWave == 0)
        {   
            EnemyWave = 1;
            StartCoroutine(NewEnemyWave());
        }
                
       if (_playerScore >= 100 && EnemyWave == 1)
       {
            EnemyWave = 2;
            StartCoroutine(NewEnemyWave());
       }
     
        if (_playerScore >= 400 && EnemyWave == 2)
        {   
            EnemyWave = 3;
            StartCoroutine(NewEnemyWave());
        }
       
        if (_playerScore >= 700 && EnemyWave ==3)
        {   
            EnemyWave = 4;
            StartCoroutine(NewEnemyWave());
        }
       
        if (_playerScore >= 1000 && EnemyWave == 4)
        {   
            EnemyWave = 5;
            StartCoroutine(NewEnemyWave());
        }

        if (_playerScore >= 1500 && EnemyWave == 5)
        {      EnemyWave = 6;
            StartCoroutine(NewEnemyWave());
        }
        
        if (_playerScore >= 2000 && EnemyWave == 6)
        {
            EnemyWave = 7;
            StartCoroutine(NewEnemyWave());
        }


    }
    public void InitiateEnemyWave()
    {
            StartCoroutine(NewEnemyWave());
    }

    IEnumerator NewEnemyWave()
    {
        yield return new WaitForSeconds(0.5f);
        _newEnemyWave.gameObject.SetActive(true);
        _audioSource.clip = _NewEnemyWaveSounds;
        _audioSource.volume = 0.25f;
        _audioSource.Play();
        yield return new WaitForSeconds(0.28f);
        _newEnemyWave.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.28f);
        _newEnemyWave.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.28f);
        _newEnemyWave.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.28f);
        _newEnemyWave.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.28f);
        _newEnemyWave.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.28f);
        _newEnemyWave.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.28f);
        _newEnemyWave.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.28f);
    }



}
