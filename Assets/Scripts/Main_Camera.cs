using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Camera : MonoBehaviour
{
    public float CameraShakeStrength;
    public float ShakeDuration;
    private Vector3 _initialCameraPosition;
    private float _remainingShakeTime;

    // Start is called before the first frame update
    void Start()
    {
        _initialCameraPosition = transform.localPosition;
        enabled = false;
    }

    public void shake()
    {
        
        _remainingShakeTime = ShakeDuration;
        CameraShakeStrength = 0.05f;
        enabled = true;
    }

    public void TractorBeam()
    {
        _remainingShakeTime = ShakeDuration;
        CameraShakeStrength = 0.03f;
        enabled = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (_remainingShakeTime <= 0)
        {
            transform.localPosition = _initialCameraPosition;
            enabled = false;
        }
        transform.Translate(Random.insideUnitCircle * CameraShakeStrength);
         _remainingShakeTime -= Time.deltaTime; 
    }

}

   
