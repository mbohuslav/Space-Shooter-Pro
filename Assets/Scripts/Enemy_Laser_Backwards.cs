using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Laser_Backwards : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private AudioSource _audioSource;
    private Enemy _enemy;


    // Start is called before the first frame update
    void Start()
    {
        _enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();
        if (_enemy == null)
        {
            Debug.LogError("Enemy is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemy != null)
        {
            Debug.Log("player fired up");
            transform.Translate(Vector3.up * _speed * Time.deltaTime);

        }
        if (transform.position.y >= 9f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
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

                Destroy(this.gameObject);
            }
        }
    }
}
