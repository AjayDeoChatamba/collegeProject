using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralVariable : MonoBehaviour
{

    public static GeneralVariable instance;

    public float delayChaseTime;

    [Header("particleEffects")]
    public GameObject bikeExplosion;

 

    // Start is called before the first frame update
    void Start()
    {
       
        instance = this;
    }


   
}
