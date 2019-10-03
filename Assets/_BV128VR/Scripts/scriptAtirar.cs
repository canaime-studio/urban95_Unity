using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class scriptAtirar : MonoBehaviour
{
    public Animator soldado1;
    public Animator canhao;
    public Animator soldado2;
    public BoxCollider colisorSoldado;
    private IEnumerator coroutine;


    void Start()
    {
        colisorSoldado=soldado1.GetComponent<BoxCollider>();


    }
    public void scriptAtira()
    {
        soldado1.SetInteger("puxar",1);
        soldado2.SetInteger("andar",1);
        canhao.Play("canhaoGirar");
        coroutine = tempoAndar();
        StartCoroutine(coroutine);



    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)){
            Debug.Log("Apertei F");
            scriptAtira();
        }
    }

    void OnTriggerExit(Collider other)
    {
        soldado1.SetInteger("puxar", 0);
       // soldado2.SetInteger("andar", 0);
        canhao.Play("32to0");
    }

    private IEnumerator tempoAndar()
    {
        
            yield return new WaitForSeconds(0.65f);
            soldado2.SetInteger("andar", 0);
            soldado2.SetInteger("virar", 1);
            yield return new WaitForSeconds(1.2f);
            soldado2.SetInteger("virar", 0);
            yield return new WaitForSeconds(8f);
            soldado2.SetInteger("andar", 1);
            yield return new WaitForSeconds(1.5f);
            soldado2.SetInteger("andar", 0);
            soldado2.SetInteger("limpar", 1);
            yield return new WaitForSeconds(1.5f);
            soldado2.SetInteger("limpar", 0);



    }

}