using System;
using System.Collections;
using System.Collections.Generic;
using Enigma_Emulator;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public List<InfantryEnemy> enemiesInScene;

    public GameObject airEnemy;

    public Transform airObjective;

    public float enemySpawnInterval;
    
    public bool spawning;

    public GameManager manager;

    public List<string> possibleMessages;

    public TMP_Text messageContainer;

    public string[] plainm;

    // Start is called before the first frame update
    void Start()
    {
        enemiesInScene = new List<InfantryEnemy>();
        //possibleMessages = new List<string>();
        spawning = true;
        //plainm = new string[] {"My enemies are many my equals are none”"};
        //GenerateCiphers(1);
    }

    void GenerateCiphers(int c)
    {
        for (int i = 0; i < c; i++)
        {
            EnigmaMachine machine = new EnigmaMachine();

            EnigmaHandler.EnigmaSettings eSettings = new EnigmaHandler.EnigmaSettings();
            eSettings.SetCustomSettings(ReadEnigmaSettings());
            machine.setSettings(eSettings.rings, eSettings.grund, eSettings.order, eSettings.reflector);

            eSettings.plugs = new List<string>() {"bq", "cr", "di", "ej", "kw", "mt", "os", "px", "uz", "gh"};
            foreach (string plug in eSettings.plugs) {
                char[] p = plug.ToCharArray();
                machine.addPlug(p[0], p[1]);
            }
            possibleMessages.Add(machine.runEnigma(plainm[0]));

        }
      
    }

    private string ReadEnigmaSettings()
    {
        string s = "I-II-III";

        return s;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawning)
        {
            spawning = false;
            CreateEnemy();
            StartCoroutine(SpawnCoolDown());
        }
    }

    void CreateEnemy()
    {
        GameObject b = Instantiate(airEnemy, transform);
        Vector2 locPos = b.transform.localPosition;
        locPos = new Vector2(locPos.x, locPos.y + Random.Range(-2, 2));
        b.transform.localPosition = locPos;
        InfantryEnemy enemy = b.GetComponent<InfantryEnemy>();
        enemy.em = this;
        enemy.pointOfInterest = new Vector3(locPos.x + 20, locPos.y);

            enemy.carryingMessage = true;
            enemy.message = possibleMessages[Random.Range(0, possibleMessages.Count)];
            enemy.messageBook = messageContainer;
        
        enemiesInScene.Add(enemy);
    }

    private IEnumerator SpawnCoolDown()
    {
        yield return new WaitForSeconds(enemySpawnInterval);
        spawning = true;
    }

    public void TakePlayerLife()
    {
        manager.curPlayerLives--;
    }
    
}
