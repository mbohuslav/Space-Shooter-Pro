using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTest : MonoBehaviour
{
    private RaycastHit2D hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
     RayCastMethod();
    }
    
    
        
    void RayCastMethod()
    {
        int layerMask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down * 12f, layerMask);
        if (hit.collider != null)
            Debug.Log("Hitting: " + hit.collider.tag);
        Debug.DrawRay(transform.position, Vector2.down * 12, Color.green, 0.01f);
        Debug.DrawLine(transform.position, hit.collider.transform.position, Color.red, 0.01f);
        SpriteRenderer sprite = hit.collider.gameObject.GetComponent<SpriteRenderer>();
        sprite.color = Color.green;

        Debug.Log("Hit " + hit.collider.name);
        Debug.Log("Hit.distance = " + hit.distance);
    }



          //  if (hit.collider != null && hit.collider == _targetplayer && firePattern == 1)
        
         //   Instantiate(_enemyLaserPrefabAlt, transform.position + new Vector3(0, -0.584f, 0), Quaternion.identity);
        
        
}
