using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody bulletRigidbody;


    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        float speed = 10.0f;
        bulletRigidbody.velocity = Vector3.forward;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject); 
    }
}
