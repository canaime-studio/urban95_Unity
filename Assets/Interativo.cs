using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interativo : MonoBehaviour
{
    public GameObject objetoInterativo;
    void Start()
    {
        if (GameManager.gameMode.Equals(GameMode.VR))
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
            //gameObject.SetActive(false);
        } else
        {
            gameObject.GetComponent<SphereCollider>().enabled = true;
        }
    }

}
