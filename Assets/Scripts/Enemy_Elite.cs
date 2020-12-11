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
    public AudioClip _laserSound;
    public AudioClip _explosion;
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
    //private Image _enemyLaserBeam;
   // [SerializeField]                    no longer neaded
   // private bool  _isLaserActive=true;
    private bool _canfire = true;

    [SerializeField]
    private GameObject _leftEngineVisualizer, _rightEngineVisualizer, _leftEngineVisualizerOn, _rightEngineVisualizerOn, _rearEngineVisualizer;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    RaycastHit2D hit;

    //Elite immunity when it reaches 1 hitpoint out of 5
    private float _ImmunityStart = 0f;
    public float ImmunityDuration = 0.65f;



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

        //_currentLaserLength = 0; no longer needed

     
       
    }

    // Update is called once per frame

    private void EnemyMovement()
    {
        float velocity = _speed * Time.deltaTime;

        if (_leftEngineVisualizer != null && _rightEngineVisualizer != null && _leftEngineVisualizerOn != null && _rightEngineVisualizerOn != null)
        {
            _leftEngineVisualizer.SetActive(false);

            if (transform.position.y != 3.82f && isCenterReached == false)
            {

                transform.position = Vector3.MoveTowards(transform.position, _targetPos, velocity);
                return;
            }
            if (transform.position.y == 3.82f)
            {
                isCenterReached = true;
                _rearEngineVisualizer.SetActive(false);
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
                if (hit.collider != null && hit.collider.tag == "Laser")

                {
                    _rightEngineVisualizer.SetActive(false);
                    _rightEngineVisualizerOn.SetActive(true);

                    StartCoroutine(DodgeRight());
                    Debug.Log("Elite Dodged Laser to the left");
                }

                else
                {
                    _rightEngineVisualizer.SetActive(true);
                    _leftEngineVisualizer.SetActive(false);
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(11.8f, 3.8f, 0f), velocity);
                }
            }
            if (transform.position.x == 11.8f)
            {
                isRightReached = true;
                isLeftReached = false;
                _rightEngineVisualizer.SetActive(false);
            }

            if (transform.position.x != -11.8f && isLeftReached == false)
            {
                if (hit.collider != null && hit.collider.tag == "Laser")

                {
                    _leftEngineVisualizer.SetActive(false);
                    _leftEngineVisualizerOn.SetActive(true);
                    StartCoroutine(DodgeLeft());
                    Debug.Log("Elite Dodged Laser to the right");
                }

                else
                {
                    _leftEngineVisualizer.SetActive(true);
                    _rightEngineVisualizer.SetActive(false);
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(-11.8f, 3.8f, 0f), velocity);

                }

            }
            if (transform.position.x == -11.8f)
            {
                isRightReached = false;
                isLeftReached = true;
                _leftEngineVisualizer.SetActive(false);
            }
        }
    }  

    private IEnumerator DodgeRight()
    {
        float velocity = _speed * Time.deltaTime;
        yield return new WaitForSeconds(0.1f);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(13.8f, 3.8f, 0f), velocity * 3f);
        yield return new WaitForSeconds(.5f);
        if (_rightEngineVisualizer != null)
        {
            _rightEngineVisualizerOn.SetActive(false);
            _rightEngineVisualizer.SetActive(true);
        }
    }

    private IEnumerator DodgeLeft()
    {
        float velocity = _speed * Time.deltaTime;
        yield return new WaitForSeconds(0.1f);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(-13.8f, 3.8f, 0f), velocity * 3f);
        yield return new WaitForSeconds(0.5f);
        if (_leftEngineVisualizer != null)
        {
            _leftEngineVisualizerOn.SetActive(false);
            _leftEngineVisualizer.SetActive(true);
        }
    }

   
    IEnumerator LaserBeam()
    {
    if (_canfire == true && hit.collider != null && hit.collider.tag == "Player" && _enemyLaserPrefab != null)
    {
            yield return new WaitForSeconds (0.2f);
            _audioSource.clip = _laserSound;
            _audioSource.volume = 0.40f;
            _audioSource.Play();
            _enemyLaserPrefab.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            _enemyLaserPrefab.SetActive(false);
            yield return new WaitForSeconds(2f);


            //Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.584f, 0), Quaternion.identity);


        }
    }
          void Update()
    {
        EnemyMovement();
    }

    private void FixedUpdate()
    {
       
        DodgePlayerFire();
        StartCoroutine(LaserBeam());
    }

    private void DodgePlayerFire()
    {
        int layerMask = LayerMask.GetMask("Default");
        hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, layerMask);
        Debug.Log("Raycast cast");
       
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
                    _audioSource.clip = _explosion;
                    _audioSource.Play();
                }

                if (_hitpoints < 1)
                {
                   // _eliteBlinking = false;
                    _anim.SetTrigger("OnEnemyDeath");
                    _canfire = false;
                    _audioSource.volume = 0.22f;
                    _audioSource.clip = _explosion;
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
                _audioSource.clip = _explosion;
                _audioSource.Play();
                transform.gameObject.tag = "Dead Enemy";
                _speed = 0;
                Destroy(GetComponent<Collider2D>());
                foreach (Transform child in EliteChildHolder.transform)
                    Destroy(child.gameObject);
                Destroy(this.gameObject, 2.5f);
            }
        }

        if (other.tag == "SuperPower")
        {
          
            if (_hitpoints > 1)
            {
                _hitpoints =1;
                _audioSource.volume = 0.10f;
                _audioSource.Play();
                _ImmunityStart = Time.time + ImmunityDuration;
            }

            if (_hitpoints < 1)
            {
                _uiManager.UpdateScore(30);
                _anim.SetTrigger("OnEnemyDeath");
                _canfire = false;
                _audioSource.volume = 0.22f;
                _audioSource.clip = _explosion;
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

            if ( _ImmunityStart <= Time.time)
            {
                _eliteBlinking = false;
                _uiManager.UpdateScore(30);
                _anim.SetTrigger("OnEnemyDeath");
                _canfire = false;
                _audioSource.volume = 0.22f;
                _audioSource.clip = _explosion;
                _audioSource.Play();
                transform.gameObject.tag = "Dead Enemy";
                _speed = 0;
                Destroy(GetComponent<Collider2D>());
                foreach (Transform child in EliteChildHolder.transform)
                    Destroy(child.gameObject);
                Destroy(this.gameObject, 2.5f);
            }

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