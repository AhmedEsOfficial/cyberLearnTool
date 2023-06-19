using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAt : MonoBehaviour
{
    public Transform target;
    
    private Gun parentGun;
    // Start is called before the first frame update
    void Start()
    {
        parentGun = transform.parent.GetComponentInParent<Gun>();

    }
    
    // Update is called once per frame
    void Update()
    { 
        if (parentGun._hasTarget && parentGun.target != null)
            {
                Vector2 destination;
                target = parentGun.target;
                destination = target.position;
                Vector3 diff = destination - (Vector2) transform.position;
                diff.Normalize();
                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                transform.rotation =Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0f, 0f, rot_z - 90), 10); ;
            }

        }
}
