using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimacao : MonoBehaviour
{

    private Animator animator;
    public bool sentado;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (sentado)
        {
            animator.SetBool("Sentado", true);
        }
        
    }
}
