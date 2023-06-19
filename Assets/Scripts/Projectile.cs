using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{

    [FormerlySerializedAs("velocity")] public float forceMag;
    private float _initialVelocity;
    public Vector2 target;
    public ParticleSystem _onImpactEffect;
    public bool hit;

    private bool _timedOut;
    // Start is called before the first frame update
    void Start()
    {
        _initialVelocity = 0;
        StartCoroutine(ProjectileLifeTime());

    }
    

    // Update is called once per frame
    void Update()
    {
        
        if (!hit && !_timedOut)
        {
            transform.position = (Vector3.MoveTowards(transform.position, target, forceMag * Time.deltaTime) );
        }
        else
        {
            StartCoroutine(OnImpactParticleWait());
        }
    }

    IEnumerator OnImpactParticleWait()
    {
        _onImpactEffect.Play();
        yield return new WaitForSeconds(0.5f);
        _onImpactEffect.Stop();
        Destroy(gameObject);

    }
    IEnumerator ProjectileLifeTime()
    {
        yield return new WaitForSeconds(1f);
        _timedOut = true;
    }
}

