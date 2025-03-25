using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody bulletRigidbody;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    


    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        float speed = 40.0f;
        bulletRigidbody.velocity = transform.forward*speed;
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("target hitted"); 
        Destroy(gameObject); 
    }
}
