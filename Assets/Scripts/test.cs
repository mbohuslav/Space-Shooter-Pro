using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float speed = 4f;
    Vector3 pos;
    Vector3 axis;
    float frequency = 2.4f; // Speed of sine movement
    float magnitude = 3f; //  Size of sine movement


    void Start()
    {
   pos = transform.position;
   axis = transform.right;
    




    
    }





    void Update()
    {
        pos += Vector3.down * Time.deltaTime * speed;
        transform.position = pos + axis * Mathf.Cos(Time.time * frequency) * magnitude;
    }
}
