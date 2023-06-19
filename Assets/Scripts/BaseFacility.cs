using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacilityType{
    Civil = 0,  
}

public class BaseFacility : MonoBehaviour
{
    public int garrison;
    private bool[] activeDefenecs;
    public int energyConsumption;

}
