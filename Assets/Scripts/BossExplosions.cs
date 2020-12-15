using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosions : MonoBehaviour
{
    public Animator Anim;
    public GameObject EngineExplosion;
    public GameObject _explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                Anim.SetTrigger("OnEnemyDamage");
            }
        }

        if (other.tag == "Laser")
        {
            Anim.SetTrigger("OnEnemyDamage");
        }

        if (other.tag == "SuperPower")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                        // EngineExplosion.GetComponent<Animator>().SetTrigger("OnEnemyDeath");

        }

    }

}


