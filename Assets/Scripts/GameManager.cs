using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxPlayerLives;
    public int curPlayerLives;
    

    // Start is called before the first frame update
    void Start()
    {
        curPlayerLives = maxPlayerLives;
    }

    // Update is called once per frame
    void Update()
    {
        if (curPlayerLives < 1)
        {
            Debug.Log("You lost");
        }
    }
}
