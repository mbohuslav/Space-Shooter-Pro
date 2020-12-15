using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float speed = 3f;
    Vector3 pos;
    Vector3 pos2;
    Vector3 axis;
    Vector3 axis2;
    float frequency = 1.3f; 
    float magnitude =1.2f; 
    float frequencySin = 0.8f; 
    float magnitudeSin = 8.5f; 

    void Start()
    {
   pos = transform.position;
   pos2 = transform.position;
   axis = transform.up;
   axis2 = transform.right; 
 
    }

   


    void Update()
    {
      
        pos += Vector3.right * Time.deltaTime*0; // keep this value
        pos2 += Vector3.right * Time.deltaTime*0;
        transform.position = pos2 + (axis2 * Mathf.Sin(Time.time * frequencySin) * magnitudeSin) - (axis * Mathf.Cos(Time.time * frequency) * magnitude);
          
       // transform.position = pos + axis * Mathf.Cos(Time.time * frequency) * magnitude;
    }
}

/*
pos += Vector3.right * Time.deltaTime;
transform.position = pos + axis * Mathf.Cos(Time.time * frequency) * magnitude;
*/