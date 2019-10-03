using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class sciptAndar : MonoBehaviour {
    public NavMeshAgent agentNav;
    public Transform alvo;
    public AudioClip[] andarFX;
    //public GameObject camera;

    public bool andando;


    private AudioSource m_AudioSource;
    private CharacterController m_CharacterController;
    public float m_StepCycle;
    public float m_NextStep;
    [SerializeField] private bool m_IsWalking;
    [SerializeField] private float m_WalkSpeed;
    private Vector2 m_Input;
    [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
    [SerializeField] private float m_StepInterval;

    // Use this for initialization
    void Start () {
        if (agentNav == null)
        {
            agentNav = this.gameObject.GetComponent<NavMeshAgent>();
        }
        m_AudioSource = GetComponent<AudioSource>();
        m_CharacterController = GetComponent<CharacterController>();
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
    }
	
	// Update is called once per frame
	void Update () {
        agentNav.SetDestination(alvo.position);
        //transform.rotation = GvrVRHelpers.GetHeadRotation();
        agentNav.updateRotation = false;
        ProgressStepCycle(agentNav.velocity.sqrMagnitude*3.5f);
        //PlayFootStepAudio();
        if (agentNav.velocity.magnitude > 0)
        {
            m_IsWalking = true;
        } else
        {
            m_IsWalking = false;
        }
        //Debug.Log(agentNav.velocity);
    }
    private void FixedUpdate()
    {
        //transform.rotation = camera.transform.rotation;
    }

    private void PlayFootStepAudio()
    {
       
        if (!m_IsWalking)
        {
          //  Debug.Log("Nao Tocar");
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        //Debug.Log("Tocar");
        int n = Random.Range(1, andarFX.Length);
        m_AudioSource.clip = andarFX[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        andarFX[n] = andarFX[0];
        andarFX[0] = m_AudioSource.clip;
    }
    private void ProgressStepCycle(float speed)
    {
        
        //Debug.Log("Adar");
        //Debug.Log(m_CharacterController.velocity);        
        if (agentNav.velocity.sqrMagnitude > 0 )
        {
            m_StepCycle += (agentNav.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                         Time.fixedDeltaTime;
        }

        //Debug.Log("m_StepCycle: " + m_StepCycle);
        //Debug.Log("m_NextStep:  " + m_NextStep);
        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }
        //Debug.Log("Aqui");

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }

}
