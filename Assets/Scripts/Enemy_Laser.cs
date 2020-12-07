using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    private Player _player;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private int[] _projectileType;

    // Start is called before the first frame update
    void Start()
    {
       // _player = GameObject.Find("Player").GetComponent<Player>();
      //  if (_player == null)
      //  {
       //     Debug.LogError("Player is NULL");
      //  }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -9f)
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
              //  _audioSource.Play(); not needed
             //   Destroy(GetComponent<SpriteRenderer>());  //?? not needed I must have been tired!
               Destroy(this.gameObject);  //?? delay not needed 
            }
        }
       /* if (other.tag == "Enemy")
        {
            Enemy enemy = other.transform.GetComponent<Enemy>();

            if (enemy  != null)
            {
                Destroy(this.gameObject);
            }
        }*/
    }

}

