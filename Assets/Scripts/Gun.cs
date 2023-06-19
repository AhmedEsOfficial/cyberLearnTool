using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    public GameObject projectile;
    public float accuracy; //between 1 and 0
    public float defaultAccuracyOffset;
    public float cooldownTime;
    private string damageTag;
    public float travelDistance;
    bool shooting;
    public Transform spawnPoint;
    private Vector2 newPos;
    public bool manual;
    public Transform target;
    public bool considersY;
    public bool _hasTarget;

    public EnemyManager em;

    private Vector2 _targetPos;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
    }

    private bool _considersY;
    public void CreateBullet()
    {

        if (_hasTarget && target != null)
        {
            GameObject b = Instantiate(projectile, spawnPoint );
            Projectile pr = b.GetComponent<Projectile>();
            float yControl;

            if (considersY)
            {  

                yControl = 1;
            }
            else
            {
                yControl = 0;

            }

            _targetPos = target.position;
            _targetPos = new Vector2(_targetPos.x + (Random.Range(-defaultAccuracyOffset, defaultAccuracyOffset)*(1f-accuracy )),
                _targetPos.y + (Random.Range(-defaultAccuracyOffset, defaultAccuracyOffset)*(1f-accuracy)* yControl));
            pr.target =  _targetPos ;
            pr.forceMag = travelDistance;
        }else
        {
            FindNextTarget();

        }


    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!shooting)
        {
            CreateBullet();
            shooting = true;
            StartCoroutine(StartCooldown());
        }
    }

    public void FindNextTarget()
    {
        Debug.Log(em.enemiesInScene.Count);
        if (em.enemiesInScene.Count > 0)
        {
            foreach (var e in em.enemiesInScene)
            {
                if (e != null)
                {
                    e.marker = this;
                    e.marked = true;
                    target = e.gameObject.transform;
                    _hasTarget = true;
                    return;
                }

            } 
        }
        else
        {
            _hasTarget = false;
        }
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        shooting = false;
    }
}
