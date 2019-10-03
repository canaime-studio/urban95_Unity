using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCaseVR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.gameMode == GameMode.VR)
        {
            this.gameObject.SetActive(false);
        }
    }
}
