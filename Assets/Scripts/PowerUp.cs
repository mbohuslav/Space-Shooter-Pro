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
    
  
    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

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
