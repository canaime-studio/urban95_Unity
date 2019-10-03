using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPlayer : MonoBehaviour {
    public bool inHands=false;
    public int zonas;
    public int score=0;
    //public GameObject camera;
    //Transform objeto;

    //public MissaoRondaCheckPoints mrcp;

    void Start () {
        //objeto = GetComponent<Transform>();
        //mrcp = GetComponent<MissaoRondaCheckPoints>();
    }

    private void FixedUpdate()
    {
        //objeto.transform.Rotate(yAngle )=;
    }

    public void CheckPoints()
    {
        //mrcp.checkpointsPosition.Length
    }

    //pensar em uma forma de limpar as mãos quando inhands = false
    public void OnTriggerEnter(Collider other)
    {
        /*
        if(other.tag == "CheckpointRedMarker")
        {
            Debug.Log("Triger");
            mrcp.ProximoCheck();
        }
        */
    }
}
