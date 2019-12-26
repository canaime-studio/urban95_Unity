using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interativo : MonoBehaviour
{
    public GameObject objetoInterativo;
    public MissaoController mc;
    void Start()
    {
        if (mc == null)
        {
            mc = FindObjectOfType<MissaoController>();
        }
        if(objetoInterativo == null)
        {
            objetoInterativo = this.gameObject;
        }
        if (GameManager.gameMode.Equals(GameMode.VR))
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
        } else
        {
            gameObject.GetComponent<SphereCollider>().enabled = true;

            if(mc != null)
            {
                if (mc.carregarMissaoController)
                {
                    if (this.gameObject.layer == 10)
                    {
                        this.gameObject.SetActive(false);
                    }
                }
            }
            
        }
    }
    public void PodeConversar(bool conversar)
    {
        if (this.gameObject.layer == 10)
        {
            this.gameObject.SetActive(conversar);
        }
    }

}
