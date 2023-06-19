using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class InfantryEnemy : BaseEnemy
{
    
    [FormerlySerializedAs("_marked")] public bool marked;//returns true if is being targeted
    public Gun marker;
    public EnemyManager em;
    public TMP_Text messageBook;

    // Start is called before the first frame update
    void Start()
    {
        AssignAll();
    }

    // Update is called once per frame
    void Update()
    {
        Advance();
    }
 
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.tag);
        if (other.tag.Equals("Bullet"))
        {
            Projectile p = other.GetComponent<Projectile>();
            p.hit = true;
            if (carryingMessage)
            {
                messageBook.text = messageBook.text + "\n" + message;
                Debug.Log(message);
            }

            Die();
        }
        if (other.tag.Equals("EnemyGoal"))
        {
            em.TakePlayerLife();
            Die();
        }
    }

    public void DropMessage()
    {
        
    }
    public void Die()
    {
        em.enemiesInScene.Remove(this);
        if (marked)
        {
            Debug.Log("Died");

            marker._hasTarget = false;
            marker.target = null;
            marker.FindNextTarget();
        }
        
        Destroy(gameObject);
    }

}
