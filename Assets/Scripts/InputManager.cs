using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject encryptionTool;

    private bool _showingET;
    // Start is called before the first frame update
    void Start()
    {
        encryptionTool.SetActive(false);
        _showingET = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_showingET)
            {
                encryptionTool.SetActive(true);
                _showingET = true;
            }
            else
            {
                encryptionTool.SetActive(false);
                _showingET = false;
            }

        }
    }
}
