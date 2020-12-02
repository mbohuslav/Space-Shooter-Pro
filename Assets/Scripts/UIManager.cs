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
    private Image _thrusterReserve;
    private float _ScaleRate = 0.35f;
    private float _maxThrusterReserve = 1.75f;
    private float _currentThrusterReserve;


    public bool ThrusterActive = true;
    public float ThrusterCooldown = 7f;
    public float ThrusterTime;
    public float MaxThrusterTime = 5f;


    public float _timestamp;
    private Player _player;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _scoreText.text = "Score: " + 0;
        _gameOver.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
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
        _gameManager.GameOver();
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
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

        if (Input.GetKeyDown(KeyCode.LeftShift) == true)
        {
            _timestamp = Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftShift) == true && ThrusterActive == true && _currentThrusterReserve > 0)
        {
            _player.ThrusterActive(true);
            _currentThrusterReserve -= _ScaleRate * (_timestamp);
            _thrusterReserve.rectTransform.localScale = new Vector3(_currentThrusterReserve, 0.35f, 1);
        }


        if (_currentThrusterReserve <= 0)
        {
            _player.ThrusterActive(false);
            StartCoroutine(ThrusterCoolDown());
        }

    }
    IEnumerator ThrusterCoolDown()
    {
        _currentThrusterReserve = _maxThrusterReserve;
        ThrusterActive = false;
        yield return new WaitForSeconds(8.0f);
        _thrusterReserve.rectTransform.localScale = new Vector3(_currentThrusterReserve, 0.35f, 1);
        ThrusterActive = true;
        
    }


    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
            
    }
}
