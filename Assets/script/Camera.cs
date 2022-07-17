using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera : MonoBehaviour
{
    [SerializeField] GameObject Vcam;
   
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CinemachineVirtualCamera>().m_Lens.NearClipPlane=-40;
        Vcam.SetActive(false);
    }
 
}
