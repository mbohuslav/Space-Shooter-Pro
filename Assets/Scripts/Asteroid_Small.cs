using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid_Small : MonoBehaviour
{
   private float _rotationSpeed = 80.0f;
    private float _speed = 3.0f;
    private Animator _anim;
    private int _asteroid;
    [SerializeField]
    private AudioSource _audioSource;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _asteroidRotation;
   
    // Start is called before the first frame update
    void Start()
    {
       _asteroid = Random.Range(0, 2);
        _anim = GetComponent<Animator>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }
           
    }

    void Update()
    {
        float velocity = _speed * Time.deltaTime;

        _asteroidRotation.transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);


        switch (_asteroid)
        {

            case 0:
                transform.Translate(-velocity * 0.50f, -velocity * 1.0f, 0);
                break;
            case 1:
                transform.Translate(velocity * 0.50f, -velocity * 1.0f, 0);
                break;

            default:
                Debug.Log("Noat a movement type");
                break;
        }

        if (transform.position.y <= -8.5f)
        {
            foreach (Transform childBees in transform)
            { Destroy(childBees.gameObject); }
            Destroy(this.gameObject);
            
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
            }
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.volume = 0.15f;
            _audioSource.Play();
            _speed = 0;
            _rotationSpeed = 0;
            transform.gameObject.tag = "Dead Enemy";
            Destroy(GetComponent<Collider2D>());
            _asteroidRotation.SetActive(false);
            Destroy(this.gameObject, 3.018f);
        }

        if (other.tag == "Laser")
        {

            _uiManager.UpdateScore(5);
            Destroy(other.gameObject);
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.volume = 0.15f;
            _audioSource.Play();
            _speed = 0;
            _rotationSpeed = 0;
            transform.gameObject.tag = "Dead Enemy";
            Destroy(GetComponent<Collider2D>());
            _asteroidRotation.SetActive(false);
            Destroy(this.gameObject, 3.018f);
        }
    }


}
