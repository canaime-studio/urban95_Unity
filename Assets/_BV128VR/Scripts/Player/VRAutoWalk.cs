using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Canaime.Players;

public enum InputType
{
    GEARVR,
    AUTOWALK,
    CLICK2MOVE,
}

[RequireComponent(typeof(CharacterController))]
public class VRAutoWalk : MonoBehaviour, IMovementPlayer
{
    public InputType inputType;
    public NvrBody controleCorpo;

    public Transform cameraPosition;



    #region AutoWalk
    [Header("AutoWalk")]
    public float speed = 3.0F;
    public bool moveForward;
    private CharacterController controller;
    private Transform vrHead;
    #endregion


    #region Click2Move
    [Header("Click2Move")]
    public GameObject LocaisAndar;
    public NavMeshAgent agentNav;
    public Transform alvo;
    public AudioClip[] andarFX;

    public bool podeAndar;
    public bool podeExibirInventario;
    private AudioSource m_AudioSource;
    public  CharacterController m_CharacterController;
    public float m_StepCycle;
    public float m_NextStep;
    [SerializeField] private bool m_IsWalking;
    [SerializeField] private float m_WalkSpeed;
    private Vector2 m_Input;
    [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
    [SerializeField] private float m_StepInterval;

    public ReticleRingInventory gazeRing;
    public InformativoUI inforUI;

    
    public GameObject camera;
    #endregion

    private void Awake()
    {
        if(controleCorpo == null)
        {
            controleCorpo = GetComponent<NvrBody>();
        }        

        if (inputType == InputType.CLICK2MOVE)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;

            if (LocaisAndar != null)
            {
                LocaisAndar.SetActive(true);
            }

            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y + 1f, camera.transform.position.z);
        }
        else
        {
            if (LocaisAndar != null)
            {
                LocaisAndar.SetActive(false);
            }
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
        }
    }
    void Start()
    {
        podeExibirInventario = false;
        Cursor.visible = false;


        m_AudioSource = GetComponent<AudioSource>();
        m_CharacterController = GetComponent<CharacterController>();
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;

        if (inputType == InputType.AUTOWALK)
        {

            controller = GetComponent<CharacterController>();
            vrHead = Camera.main.transform;
            enabled = true;
        }
        else if (inputType == InputType.CLICK2MOVE)
        {
            if (agentNav == null)
            {
                agentNav = this.gameObject.GetComponent<NavMeshAgent>();
            }
        }
    }

    void Update()
    {
        if (inputType == InputType.AUTOWALK)
        {
            _AndarAuto();
        }
        else if (inputType == InputType.CLICK2MOVE)
        {
            _AndarClick2Move();
        }
    }

    public void Informacao()
    {        
        StopWalk();     
        gazeRing.buscarInformacao = true;
    }
    public void FecharInformacao()
    {
        inforUI.CloseInformation();        
        StopWalk();
    }

    #region AutoWalk
    public void Info()
    {
        if (moveForward)
        {
            moveForward = !moveForward;
        }
    }

    void _AndarAuto()
    {
        if (podeAndar)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                moveForward = !moveForward;         
            }
            if (moveForward)
            {
                m_IsWalking = true;
                ProgressStepCycle(0.1f * 3.5f);
                Vector3 forward = vrHead.TransformDirection(Vector3.forward);
                controller.SimpleMove(forward * speed);
            }
            else
            {
                m_CharacterController.SimpleMove(Vector3.zero);
            }
        }
    }
    public void Psarar()
    {

    }
    public void Andar()
    {

    }
    public void StopWalk()
    {
        Debug.Log("Metodo Parar");
        m_CharacterController.SimpleMove(Vector3.zero);
        moveForward = false;
    }
    public void Sentar()
    {
        Debug.Log("teste");
    }
    public void Sentado(bool isSitting)
    {
        if (isSitting)
        {
            controleCorpo.cameraHeightOffset = 1.3f;
            controleCorpo.bodyTurnAngleIdle = 360;
            controleCorpo.bodyTurnAngleMoving = 360;
            Debug.Log("Sentado Message");
        } else
        {
            Debug.Log("Levantando Camera");
            controleCorpo.cameraHeightOffset = FindObjectOfType<StartupScene>().alturaPlayer-0.10f;
            //controleCorpo.cameraHeightOffset = 1.7f;
            controleCorpo.bodyTurnAngleIdle = 75;
            controleCorpo.bodyTurnAngleMoving = 10;
        }
        
    }
    #endregion
    public void OnEnable()
    {
        Debug.Log("Controle Habilitado");
    }
    public void OnDisable()
    {
        Debug.Log("VR AUTO DESABILITADO Controle Desabilitado");
    }
    public void isEnable(bool enabled)
    {
        this.enabled = enabled;
    }





    #region Click2Move
    void _AndarClick2Move()
    {
        agentNav.SetDestination(alvo.position);
        //transform.rotation = GvrVRHelpers.GetHeadRotation();
        agentNav.updateRotation = false;
        ProgressStepCycle(agentNav.velocity.sqrMagnitude * 3.5f);
        //PlayFootStepAudio();
        if (agentNav.velocity.magnitude > 0)
        {
            m_IsWalking = true;
        }
        else
        {

            m_IsWalking = false;
        }
        //Debug.Log(agentNav.velocity);
    }
    private void PlayFootStepAudio()
    {

        if (!m_IsWalking)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0        
        int n = Random.Range(1, andarFX.Length);
        m_AudioSource.clip = andarFX[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        andarFX[n] = andarFX[0];
        andarFX[0] = m_AudioSource.clip;
    }
    private void ProgressStepCycle(float speed)
    {
        if (inputType == InputType.CLICK2MOVE)
        {
            if (agentNav.velocity.sqrMagnitude > 0)
            {
                m_StepCycle += (agentNav.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                             Time.fixedDeltaTime;
            }
            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;
            PlayFootStepAudio();
        }
        else
        {
            //Debug.Log("Tocar som passos");
            //if (agentNav.velocity.sqrMagnitude > 0)
            //{
            m_StepCycle += (0.01f + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) * Time.fixedDeltaTime;
            //}
            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }
            m_NextStep = m_StepCycle + 0.3f;
            PlayFootStepAudio();
        }

    }
    #endregion
}