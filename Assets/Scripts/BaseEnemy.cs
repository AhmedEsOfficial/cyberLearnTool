using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class BaseEnemy : MonoBehaviour
{
    public Vector3 pointOfInterest;
    public float speed;

    
    public string message;
    public bool carryingMessage;

    public void AssignAll()
    {

    }

    public void Advance()
    {   
        transform.position = Vector3.MoveTowards(transform.position, pointOfInterest, Time.deltaTime * speed);
    }
    

   
}
