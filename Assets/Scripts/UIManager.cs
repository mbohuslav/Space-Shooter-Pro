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
    


    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
      // _livesprites[CurrentPlayerLives = 3];
        _scoreText.text = "Score: " + 0;
        _gameOver.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

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
    
    }
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
            
    }
}
