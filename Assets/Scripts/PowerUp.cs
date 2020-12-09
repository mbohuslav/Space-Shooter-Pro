using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [SerializeField]
    private float _speed =3.0f;
    [SerializeField] 
    private int _powerUpID; 
    [SerializeField]
    private AudioClip _clip;
    [SerializeField]
    private AudioClip _tractorBeam;
    GameObject _targetplayer;
    private Vector3 target;
    private Main_Camera _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _targetplayer = GameObject.FindWithTag("Player");
        _mainCamera = GameObject.Find("Main Camera").GetComponent<Main_Camera>();
        if (_mainCamera == null)
        {
            Debug.LogError(" the Main Camera is NULL");
        }
    }
    // Update is called once per frame
    void Update()
    {
         float velocity = _speed * Time.deltaTime;
        float distance = Vector3.Distance(target, transform.position);
        if (_targetplayer != null)
        {
            target = _targetplayer.transform.position;
        }

        while (_targetplayer != null && Input.GetKey(KeyCode.C) && distance <= 8f)
          {
            
            AudioSource.PlayClipAtPoint(_tractorBeam, transform.position, 0.45f);
            transform.position = Vector3.MoveTowards(transform.position, target, velocity*3);
            _mainCamera.TractorBeam();
            return;
            
           }
       
          transform.Translate(Vector3.down * velocity);
      
     




        if (transform.position.y <= -9f)
        {
            Destroy(this.gameObject);
        }         
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
           AudioSource.PlayClipAtPoint(_clip, transform.position, 1f);
            if (player != null)
            {
                switch (_powerUpID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;                    
                    case 2:
                        player.ShieldsActive();
                        break;
                    case 3:
                        player.AmmoActive();
                        break;
                    case 4:
                        player.HealthActive();
                        break;
                    case 5:
                        player.HomingMissileActive();
                        break;
                    case 6:
                        player.MonkeyActive();
                        break;
                    default:
                        Debug.Log("Not a Valid Power Up");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }

}
