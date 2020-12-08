using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Elite : MonoBehaviour
{
    private float _speed = 4.5f;
    private int _hitpoints = 5;
    [SerializeField]
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private Animator _anim;
    private SpriteRenderer _eliteRenderer;

    //Color change variables
    private bool _eliteBlinking = false;
    private Color _targetColor;
    private Color _startingColor;


    private Vector3 _targetPos;
    private bool isCenterReached = false;
    private bool isRightReached = false;
    private bool isLeftReached = false;

    // Vector3 pos;
    // Vector3 axis;
    // float frequency = 0.6f; // Speed of sine movement
    // float magnitude = 13f; //  Size of sine movement

    public GameObject EliteChildHolder;
    [SerializeField]
    private Image _enemyLaserBeam;
    private float _beamDecreaseSpeed = 1.15f;
    private float _beamIncreaseSpeed = 2.3f;
    private float _maxLength = 2.3f;
    private float _currentLaserLength;
    [SerializeField]
    private bool  _isLaserActive=true;
    private bool _canfire = true;



    // Start is called before the first frame update
    void Start()
    {
        // _currentPosition = transform.position;
        _targetPos = new Vector3(0f, 3.82f, 0f);
        //  axis = transform.right;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _anim = GetComponent<Animator>();
        _eliteRenderer = GetComponent<SpriteRenderer>();
        
        _audioSource = GetComponent<AudioSource>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL");
        }
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        //Color blinking routine setup
        _startingColor = Color.white;
        _targetColor = Color.red;

        _currentLaserLength = 0;
      



}

    // Update is called once per frame

    private void EnemyMovement()
    {
        float velocity = _speed * Time.deltaTime;
        if (transform.position.y != 3.82f && isCenterReached == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, velocity);
        }
        if (transform.position.y == 3.82f)
        {
            isCenterReached = true;
            int _move = Random.Range(0, 2);
            switch (_move)
            {
                case 0:
                    isRightReached = true;
                    isLeftReached = false;
                    break;
                case 1:
                    isRightReached = false;
                    isLeftReached = true;
                    break;
                default:
                    Debug.Log("Not a valid direction for Elite");
                    break;
            }
        }

        if (transform.position.x != 11.8f && isRightReached == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(11.8f, 3.8f, 0f), velocity);
        }
        if (transform.position.x == 11.8f)
        {
            isRightReached = true;
            isLeftReached = false;
        }
        if (transform.position.x != -11.8f && isLeftReached == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-11.8f, 3.8f, 0f), velocity);
        }
        if (transform.position.x == -11.8f)
        {
            isRightReached = false;
            isLeftReached = true;
        }

        // (_targetPos != new Vector3(0f, 3.9f, 0f))

        // else
        //  {
        //    transform.position = Vector3.down * velocity;
        //     transform.position = transform.position + axis * Mathf.Sin(Time.deltaTime * frequency) * magnitude;
        //   }

    }

   private void FireLaser()
    {

        if (_canfire == true)
        {

            if (_isLaserActive == true)
            {
                _currentLaserLength += _beamIncreaseSpeed * Time.deltaTime;
                _currentLaserLength = Mathf.Min(_currentLaserLength, _maxLength);
                _enemyLaserBeam.rectTransform.localScale = new Vector3(1.2f, _currentLaserLength, 1f);
                _enemyLaserBeam.transform.Translate(Vector3.down * (4.795f + _beamIncreaseSpeed) * Time.deltaTime);

                if (_currentLaserLength == _maxLength)
                {
                    _isLaserActive = false;
                }


            }
            if (_isLaserActive == false)
            {
                _currentLaserLength -= _beamDecreaseSpeed * Time.deltaTime;
                _currentLaserLength = Mathf.Clamp(0, _currentLaserLength, 0);
                _enemyLaserBeam.rectTransform.localScale = new Vector3(1.2f, _currentLaserLength, 1f);
                _enemyLaserBeam.transform.Translate(Vector3.up * (2.41f + _beamDecreaseSpeed) * Time.deltaTime);
                // _enemyLaserBeam.transform.position = new Vector3(transform.position.x, -(_beamIncreaseSpeed / 2) * Time.deltaTime, transform.position.z);
                if (_currentLaserLength == 0)
                {
                    ;
                    _isLaserActive = true;
                }
            }
        }
   
    }



    void Update()
    {
        EnemyMovement();
        FireLaser();


    }







    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();

                if (_hitpoints > 0)
                {
                    _hitpoints -= 1;
                    _audioSource.volume = 0.10f;
                    _audioSource.Play();
                }

                if (_hitpoints < 1)
                {
                   // _eliteBlinking = false;
                    _anim.SetTrigger("OnEnemyDeath");
                    _canfire = false;
                    _audioSource.volume = 0.22f;
                    _audioSource.Play();
                    transform.gameObject.tag = "Dead Enemy";
                    _speed = 0;
                    Destroy(GetComponent<Collider2D>());
                    foreach (Transform child in EliteChildHolder.transform)
                        Destroy(child.gameObject);
                    Destroy(this.gameObject, 2.5f);
                }
            }
        }
      
        if (other.tag == "Laser")
        {
            if (_hitpoints > 0)
            {
                _hitpoints -= 1;
                Destroy(other.gameObject);
                _audioSource.volume = 0.10f;
                _audioSource.Play();
            }

            if (_hitpoints < 1)
            {
                Destroy(other.gameObject);
                Debug.Log("Laser destroyed");
                _uiManager.UpdateScore(30);
               // _eliteBlinking = false;
                _anim.SetTrigger("OnEnemyDeath");
                _canfire = false;
                _audioSource.volume = 0.22f;
                _audioSource.Play();
                transform.gameObject.tag = "Dead Enemy";
                _speed = 0;
                Destroy(GetComponent<Collider2D>());
                foreach (Transform child in EliteChildHolder.transform)
                    Destroy(child.gameObject);
                Destroy(this.gameObject, 2.5f);
            }
        }
        if (_hitpoints == 1)
        {
            _eliteBlinking = true;
            StartCoroutine(ColorChange());
        }
        else
        {
            _eliteBlinking = false;
        }
    }

    IEnumerator ColorChange()
    {
        if (_eliteRenderer == null)
        {
        }

        while (_eliteBlinking == true)
        {
            _eliteRenderer.color = _targetColor;
            yield return new WaitForSeconds(0.25f);
            _eliteRenderer.color = _startingColor;
            yield return new WaitForSeconds(0.25f);
        }


    }
}

/*
 * 
 *  
//This is free to use and no attribution is required
//No warranty is implied or given
using UnityEngine;
using System.Collections;
 
[RequireComponent (typeof(LineRenderer))]
 
public class LaserBeam : MonoBehaviour {
   
    public float laserWidth = 1.0f;
    public float noise = 1.0f;
    public float maxLength = 50.0f;
    public Color color = Color.red;
   
   
    LineRenderer lineRenderer;
    int length;
    Vector3[] position;
    //Cache any transforms here
    Transform myTransform;
    Transform endEffectTransform;
    //The particle system, in this case sparks which will be created by the Laser
    public ParticleSystem endEffect;
    Vector3 offset;
   
   
    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetWidth(laserWidth, laserWidth);
        myTransform = transform;
        offset = new Vector3(0,0,0);
        endEffect = GetComponentInChildren<ParticleSystem>();
        if(endEffect)
            endEffectTransform = endEffect.transform;
    }
   
    // Update is called once per frame
    void Update () {
        RenderLaser();
    }
   
    void RenderLaser(){
       
        //Shoot our laserbeam forwards!
        UpdateLength();
       
        lineRenderer.SetColors(color,color);
        //Move through the Array
        for(int i = 0; i<length; i++){
            //Set the position here to the current location and project it in the forward direction of the object it is attached to
            offset.x =myTransform.position.x+i*myTransform.forward.x+Random.Range(-noise,noise);
            offset.z =i*myTransform.forward.z+Random.Range(-noise,noise)+myTransform.position.z;
            position[i] = offset;
            position[0] = myTransform.position;
           
            lineRenderer.SetPosition(i, position[i]);
           
        }
       
       
       
    }
   
    void UpdateLength(){
        //Raycast from the location of the cube forwards
        RaycastHit[] hit;
        hit = Physics.RaycastAll(myTransform.position, myTransform.forward, maxLength);
        int i = 0;
        while(i < hit.Length){
            //Check to make sure we aren't hitting triggers but colliders
            if(!hit[i].collider.isTrigger)
            {
                length = (int)Mathf.Round(hit[i].distance)+2;
                position = new Vector3[length];
                //Move our End Effect particle system to the hit point and start playing it
                if(endEffect){
                endEffectTransform.position = hit[i].point;
                if(!endEffect.isPlaying)
                    endEffect.Play();
                }
                lineRenderer.SetVertexCount(length);
                return;
            }
            i++;
        }
        //If we're not hitting anything, don't play the particle effects
        if(endEffect){
        if(endEffect.isPlaying)
            endEffect.Stop();
        }
        length = (int)maxLength;
        position = new Vector3[length];
        lineRenderer.SetVertexCount(length);
       
       
    }
}
 
*/