using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing_Missle : MonoBehaviour
{
    [SerializeField]
    private float _speed = 7f;
    public Transform target;
    public Rigidbody2D rigidBody;
    public float angleChangingSpeed = 210.0f;
    public SpawnManager _spawnManager;
    GameObject _enemyTarget;
    private bool _doNotRunSpawnCheckAnymore = true;

    void Start()
    {
         _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        rigidBody = GetComponent<Rigidbody2D>();
    }
     private void EnemySpawned()
    {
        _doNotRunSpawnCheckAnymore = false;
        _enemyTarget = GameObject.FindWithTag("Enemy");
        
        if (_enemyTarget == null)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        } 
    } 
    private void Update()
    {
        if (_spawnManager._enemySpawnDetection == true && _doNotRunSpawnCheckAnymore == true)
        {
            EnemySpawned();
        }
    }
   void FixedUpdate()
    {
        if (target == null)
        { 
         transform.Translate(Vector3.up * _speed * Time.deltaTime);
         
        }

        if (_enemyTarget != null)
         { 
           target = _enemyTarget.transform;
      //     float step = _speed * Time.deltaTime;
        //    transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            Vector2 direction = (Vector2)target.position - rigidBody.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rigidBody.angularVelocity = -angleChangingSpeed * rotateAmount;
            rigidBody.velocity = transform.up * _speed;
          
         }
          



      /*  Vector2 direction = (Vector2)target.position - rigidBody.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rigidBody.angularVelocity = -angleChangingSpeed * rotateAmount;
        rigidBody.velocity = transform.up * _speed;
      */
        if (transform.position.y >=6f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }

    } 
}
