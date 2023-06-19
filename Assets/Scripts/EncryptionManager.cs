using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enigma_Emulator;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum States
{
    Binary = 0, Hex = 1, Enigma = 2, Anagram = 3
}

public class EncryptionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField plainTextInput;
    public TMP_InputField cipherTextInput;
    
    public TMP_InputField anagramKeyInput;
    public TMP_InputField anagramSKeyInput;


    public Dropdown cipherTypeDropdown;
    
    private String _currentPlainText;
    private String _currentCipherText;

    public GameObject enigmaSettingPanel;
    public GameObject anagramSettingPanel;


    public List<Dropdown> rotorInput;

    private string[] possibleRotors;
    


    private int _currentState;
    private EnigmaMachine machine;
    
    
    
    void Awake()
    {
        possibleRotors = new[] {"I", "II", "III"};
    }

    private void Start()
    {
    }


    // Update is called once per frame

    
    public void OnCipherTypeChange()
    {
        _currentState = cipherTypeDropdown.value;
        if (_currentState == (int) States.Enigma)
        {
            SetAnagramUI(false);
            SetEnigmaUI(true);
        }
        else if (_currentState == (int) States.Anagram)
        {
            SetEnigmaUI(false);
            SetAnagramUI(true);
        }
        else
        {
            SetEnigmaUI(false);
            SetAnagramUI(false);
        }
    }

    public void OnEncrypt()
    {
        _currentPlainText = plainTextInput.text;
        switch (_currentState)
        {
            case (int) States.Binary:
                cipherTextInput.text = ToBinary(ConvertToByteArray(_currentPlainText, Encoding.ASCII));
                break;
            case (int) States.Hex:
                cipherTextInput.text = ConvertStringToHex(_currentPlainText);
                break;
            case (int) States.Enigma:
                SetUpEnigma();
                cipherTextInput.text= machine.runEnigma(_currentPlainText);
                break;
            case (int) States.Anagram:
                cipherTextInput.text = EncryptAnagram(_currentPlainText, anagramKeyInput.text, anagramSKeyInput.text, false);
                break;
        }


    }
    
    public void OnDecrypt()
    {
        _currentCipherText = cipherTextInput.text;
        if (_currentState == (int) States.Binary)
        {
            plainTextInput.text = ConvertBinaryToString(_currentCipherText);
        }else if (_currentState == (int) States.Hex)
        {
            byte[] data = ConvertHexToString(_currentCipherText);
            if (data != null)
            {
                plainTextInput.text = Encoding.ASCII.GetString(data); 
            }
            else
            {
                plainTextInput.text = "Wrong Hex Format";
            }
        }

        if (_currentState == (int) States.Enigma)
        {
            SetUpEnigma();
            plainTextInput.text= machine.runEnigma(_currentCipherText);
        }

        if (_currentState == (int) States.Anagram)
        {
            Debug.Log(anagramKeyInput.text);
            plainTextInput.text = EncryptAnagram(_currentPlainText, anagramKeyInput.text, anagramSKeyInput.text, true);

        }

    }

    public void SetEnigmaUI(bool active)
    {
        enigmaSettingPanel.SetActive(active);
    }

    public void SetAnagramUI(bool active)
    {
        anagramSettingPanel.SetActive(active);
    }
    string ReadEnigmaSettings()
    {
        string ord = "";

        foreach (var rotorIn in rotorInput)
        {
            ord += possibleRotors[rotorIn.value] + "-";
        }
        ord = ord.Substring(0, ord.Length-1);
        print(ord);
        return ord;
    }
    void SetUpEnigma()
    {
        machine = new EnigmaMachine();
        EnigmaHandler.EnigmaSettings eSettings = new EnigmaHandler.EnigmaSettings();
        eSettings.SetCustomSettings(ReadEnigmaSettings());
        machine.setSettings(eSettings.rings, eSettings.grund, eSettings.order, eSettings.reflector);

        eSettings.plugs = new List<string>() {"bq", "cr", "di", "ej", "kw", "mt", "os", "px", "uz", "gh"};
        foreach (string plug in eSettings.plugs) {
            char[] p = plug.ToCharArray();
            machine.addPlug(p[0], p[1]);
        }
    }
    
    /*
     * Encrypt Binary
     */
    string ConvertBinaryToString(string binaryCode)
    {
        String oneExpression = "";
        String resultText = "";
        binaryCode += " ";
        int characterCount = binaryCode.Replace(" ", "").Length;
        if (characterCount % 8 != 0)
        {
            return "Incorrect Binary Code";
        }

        foreach (char character in binaryCode)
        {
            if (character != '0' && character != '1' && character != ' ')
            {
                return "WrongFormat";
            }
            if (character == '0' || character == '1')
            {
                oneExpression += character;
            }

            if (character == ' ')
            {
                if (oneExpression != "")
                {
                    var data = GetBytesFromBinaryString(oneExpression);
                    resultText += Encoding.ASCII.GetString(data);
                    oneExpression = "";
                }
                
            }
        }
        return resultText;
    }
    public Byte[] GetBytesFromBinaryString(string binary)
    {
        var list = new List<Byte>();

        for (int i = 0; i < binary.Length; i += 8)
        {
            
            String t = binary.Substring(i, 8);

            list.Add(Convert.ToByte(t, 2));

        }

        return list.ToArray();
    }
    

    /*
     * Decrypt Binary
     */
    public static String ToBinary(Byte[] data)
    {
        return string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
    }

    // Use any sort of encoding you like. 
    public static byte[] ConvertToByteArray(string str, Encoding encoding)
    {
        return encoding.GetBytes(str);
    }
    
    /*
     * Encrypt Hex
     */
    string ConvertStringToHex(string asciiString)
    {
        string hex = "";
        foreach (char c in asciiString)
        {
            int tmp = c;
            hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
        }
        return hex;
    }


    string EncryptAnagram(string text, string key, string scrambledKey, bool decrypt)
    {
        Dictionary<int, int> charOffsets= new Dictionary<int, int>();

        if (decrypt)
        {
            string temp = "" + key;
            key = "" + scrambledKey;
            scrambledKey = "" + temp;
        }
        
        if (text.Length < key.Length)
        {
            return "The message cannot be shorter than the key";
        }
        
        foreach (var c in scrambledKey)
        {
            if (!key.Contains(c))
            {
                return "The scrambled key must contain letters from original key";
            }
        }


        int remainder = text.Length % key.Length;
        int l = text.Length;
        for (int i = text.Length; i < l + (key.Length - remainder); i++)
        {
            text = text + "_";
        }

        text = text.Replace(" ", "");

        //Divide sentence into words of equal length to the key
        string[] words = new string[text.Length / key.Length];
        for (int i = 0; i < words.Length; i++)
        {
            print(text);
            words[i] = text.Substring(i * key.Length, key.Length);
            print(words[i]);
        }
        

        print(key + " "+ scrambledKey);
        //Actual Encryption
        string result = "";
        for (int i = 0; i < scrambledKey.Length; i++)
        {
            for (int j = 0; j < key.Length; j++)
            { 
                if (key[j] == scrambledKey[i])
                {
                    charOffsets.Add(i, j);
                    print(i + " to " + j );
                }
            }
        }

        foreach (string word in words )
        {
            int f = word.Length;
            char[] chars = new char[f];
            for (int i = 0; i < f; i++)
            {
                chars[i] = word[charOffsets[i]];
            }

            result = result + chars.ArrayToString() + " ";
        }
        
        print(result);
        
        return result.Replace("-","");
    }



    public static byte[] ConvertHexToString(string hex)
    {
        hex = hex.Replace("-", "");
        hex = hex.Replace(" ", "");
        foreach (var character in hex)
        {
            if (!"0123456789ABCDEFabcdef".Contains(character))
            {
                return null;

            }

        }
        byte[] raw = new byte[hex.Length / 2];
        for (int i = 0; i < raw.Length; i++)
        {
            raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }
        return raw;
    }



    public void ClearCipher()
    {
        cipherTextInput.text = "";
    }

    public void ClearPlain()
    {
        plainTextInput.text = "";
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
