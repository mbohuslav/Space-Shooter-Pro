using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;
    private Animator _anim;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private int _enemyID;
    private bool _canmove = true;

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
        _enemyID = Random.Range(0,4);
        transform.position = new Vector3(Random.Range(-8f, 8f), 7f, 0);

        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }

        if (_canmove == true)
        { StartCoroutine(FireLaser()); }

    }

    // Update is called once per frame
    void Update()
    {
        if (_canmove == true)
        {
            float velocity = _speed * Time.deltaTime;


            switch (_enemyID)
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
                _enemyID = Random.Range(0, 4);
    transform.position = new Vector3(randomX, 7f, 0);
                pos = transform.position;
            }
        }  

    }

    IEnumerator FireLaser()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -1.05f, 0), Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
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
            }
            _canmove = false;
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>(), 0.50f);
            Destroy(this.gameObject, 2.5f);
        } 
        if (other.tag == "Laser")

        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Destroyed_anim"))
                {
                }
                else
                {
                    _player.AddScore(10);
                }

            }
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _canmove = false;
            transform.gameObject.tag = "Dead Enemy";
            Destroy(GetComponent<Collider2D>(), 0.50f);
            Destroy(this.gameObject, 2.5f);
        }
    
    }
}
