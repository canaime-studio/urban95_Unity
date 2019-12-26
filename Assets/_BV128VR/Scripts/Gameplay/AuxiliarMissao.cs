using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxiliarMissao : MonoBehaviour
{
    [Tooltip("Apontar para algum checkpoint")]
    public GameObject alvo;
    private bool isVR;

    // Start is called before the first frame update
    void Start()
    {
 /*       isVR = GameManager.gameMode.Equals(GameMode.VR) ? true : false;
        if (isVR)
        {
            this.gameObject.SetActive(true);
        }
        else if (!isVR)
        {
            this.gameObject.SetActive(false);

        }*/

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (alvo != null)
        {
            this.gameObject.transform.LookAt(alvo.transform);
        }

    }
}
