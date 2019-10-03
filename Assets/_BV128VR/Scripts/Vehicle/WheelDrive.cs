using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
//using UnityEditor.Presets;
using UnityStandardAssets.CrossPlatformInput;

[Serializable]
public enum DriveType
{
    RearWheelDrive,
    FrontWheelDrive,
    AllWheelDrive
}
[System.Serializable]
public class Roda
{
    public string nomeRoda;
    public WheelCollider wheelCollider;
    public GameObject wheelMesh;

}
[RequireComponent(typeof(Vehicle))]
public class WheelDrive : MonoBehaviour
{
    #region Variaveis
    [Tooltip("Maximum steering angle of the wheels")]
    public float maxAngle = 30f;
    [Tooltip("Maximum torque applied to the driving wheels")]
    public float maxTorque = 300f;
    [Tooltip("Maximum brake torque applied to the driving wheels")]
    public float brakeTorque = 30000f;
    //[Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
    //public GameObject wheelShape;

    [Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
    public float criticalSpeed = 5f;
    [Tooltip("Simulation sub-steps when the speed is above critical.")]
    public int stepsBelow = 5;
    [Tooltip("Simulation sub-steps when the speed is below critical.")]
    public int stepsAbove = 1;

    [Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
    public DriveType driveType;

    private WheelCollider[] m_Wheels;
    public GameManager gameManager;
    public List<Roda> rodas;
    public VehiclePlayerControl vehiclePlayerControl;




    [Header("Game Objects")]
    public GameObject puxador;
    public GameObject ControllerAnimalPuxador;
    public GameObject player;
    public GameObject playerRoot;
    public GameObject c_WagonPulled;
    public GameObject c_WagonCotrolled;

    public GameObject[] gameObjectsController;
    public Vehicle vehicle;

    [Header("Components")]
    HingeJoint hj;
    //public Preset presetWagonHingJoint;

    [Header("Positions")]
    public GameObject _animalPosition;
    public GameObject _playerPosition;
    public Transform _exitPlayerPosition;
    public Transform _VRCanvasPosition;

    [Header("3 Pessoa")]
    public Transform pivotCamera;

    //public IAAnimais a_iaController;
    //public NavMesh a_iaNavAgent;


    float angle;
    float handBrake;
    float torque;

    public float currentSpeed;

    #endregion

    private void Awake()
    {
        if (vehicle == null)
        {
            vehicle = GetComponent<Vehicle>();
        }
        if (vehiclePlayerControl == null)
        {
            vehiclePlayerControl = FindObjectOfType<VehiclePlayerControl>();
        }

        if (gameManager == null) gameManager = FindObjectOfType<GameManager>();

    }
    void Start()
    {
        player = gameManager.playerAtivo.gameObject;
        playerRoot = player.transform.root.gameObject;

        if (ControllerAnimalPuxador != null)
        {
            var a_iaController = ControllerAnimalPuxador.GetComponent<IAAnimais>();
            var a_iaNavAgent = ControllerAnimalPuxador.GetComponent<NavMeshAgent>();

            // Mudar para o awake
            if (vehicle.isControlled)
            {
                a_iaController.enabled = false;
                a_iaNavAgent.enabled = false;

                //SetupPlayerController();
            }
            else
            {
                c_WagonCotrolled.SetActive(false);
                c_WagonPulled.SetActive(true);

                foreach (GameObject o in gameObjectsController)
                {
                    o.SetActive(false);
                }
            }
        }

        m_Wheels = GetComponentsInChildren<WheelCollider>();


    }
    void FixedUpdate()
    {
        if (vehicle.isControlled)
        {
            PlayerDriverController();
            currentSpeed = this.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude;
            ControllerAnimalPuxador.GetComponent<Animator>().SetBool("IsControlled", true);
            ControllerAnimalPuxador.GetComponent<Animator>().SetFloat("Speed", currentSpeed);
            //ControllerAnimalPuxador.GetComponent<Animator>().SetInteger("status", -2);
            //ControllerAnimalPuxador.GetComponent<Animator>().speed = currentSpeed;
        }
        else
        {
            IADrive();
            
        }
        
    }


    #region WellsBehaviour
    // We simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero.
    // This helps us to figure our which wheels are front ones and which are rear.
    void WhellsBehaviour()
    {
        foreach (Roda roda in rodas)
        {
            Quaternion q;
            Vector3 p;
            roda.wheelCollider.GetWorldPose(out p, out q);
            roda.wheelMesh.transform.position = p;
            roda.wheelMesh.transform.rotation = q;
        }
    }
    // Automatic mesh instantiate 
    void WhellsInstantiate()
    {
        /*
        for (int i = 0; i < m_Wheels.Length; ++i)
        {
            var wheel = m_Wheels[i];
            //Create wheel shapes only when needed.
            if (wheelShape != null)
            {
                var ws = Instantiate(wheelShape);
                ws.transform.parent = wheel.transform;
            }
        }
        */
    }
    #endregion

    #region Player Control
    public void DriveVehicle()
    {
        SetupPlayerDriver();
    }
    public void ExitVehicle()
    {
        //Debug.Log("Saiu do Veiculo");
        WagonSetupStandBy();

        //// Player Controle
        //if (vehicle.isControlled && vehicle.canExitVehicle)
        //{
        //    //Debug.Log("Tentar Sair");
        //    //StartCoroutine("WagonSetupStandBy");
        //    WagonSetupStandBy();
        //    vehicle.isControlled = false;
        //    vehicle.canExitVehicle = false;
        //}
        //else if (vehicle.isRide && vehicle.canExitVehicle)
        //{            
        //    WagonSetupStandBy();
        //    vehicle.isControlled = false;
        //    //isRide = false;
        //    //canExit = false;
        //    //canGetRide = true;
        //    vehicle.canDriveVehicle = true;
        //}
    }
    public void GetRide()
    {

        if (!vehicle.isControlled && vehicle.canGetRideVehicle)
        {
            WagonSetupRide();
            vehicle.isControlled = false;
            vehicle.canGetRideVehicle = false;
            vehicle.canDriveVehicle = false;
            vehicle.isRide = true;
            vehicle.canExitVehicle = true;            
        }
    }
    #endregion

    #region Wagon Setup
    // Boi Puxa Carroca Sozinho IA
    void WagonSetupStandBy()
    {
        if (vehicle.canExitVehicle)
        {            
            // Configura Carroca
            if (gameObject.GetComponent<HingeJoint>() == null)
            {
                //Debug.Log("HJ é NULL");
                var hingeJoint = gameObject.AddComponent<HingeJoint>();
                hingeJoint.anchor = new Vector3(0, -0.15f, 3.27f);
                hingeJoint.axis = new Vector3(0, 0, 0);
                hingeJoint.autoConfigureConnectedAnchor = true;
                
                JointLimits limits = hingeJoint.limits;
                limits.min = -111;
                limits.max = 17;
                limits.bounciness = 0;
                limits.contactDistance = 0;
                limits.bounceMinVelocity = 0.2f;
                hingeJoint.limits = limits;
                hingeJoint.useLimits = true;

                //presetWagonHingJoint.ApplyTo(gameObject.GetComponent<HingeJoint>());
                hingeJoint.connectedBody = puxador.GetComponent<Rigidbody>();
            }

            if (gameManager.playerAtivo != null)
            {
                switch (GameManager.gameMode)
                {
                    case GameMode.VR:

                        // Implementar metodo dentro do player
                        player.GetComponent<Player>().EnableComponents(true, null);

                        player.GetComponent<Player>().SetPositionCanvasVR(playerRoot.transform);
                        //player.GetComponent<Player>().canvasObject.transform.position = playerRoot.transform.position;                                                
                        //player.GetComponent<Player>().canvasObject.transform.SetParent(playerRoot.transform);
                        //player.GetComponent<Player>().canvasObject.transform.localRotation = Quaternion.Euler(new Vector3(0, -46.5f, 0));
                        //player.GetComponent<Player>().canvasObject.transform.localPosition = new Vector3(-1, -0.45f, 1.45f);

                        player.transform.SetParent(playerRoot.transform);
                        player.transform.position = _exitPlayerPosition.transform.position;
                        player.transform.rotation = new Quaternion(0, 0, 0, 0);
                        player.transform.localScale = new Vector3(1, 1, 1);

                        player.GetComponent<Player>().Sentar(false);
                        break;
                    case GameMode.ThirdPerson:

                        player.GetComponent<Player>().Sentar(false);
                        setPivotDistance(new Vector3(0, 2, 0), new Vector3(0,0,0));
                        player.GetComponent<Player>().EnableComponents(true, player.GetComponent<Player>().rb);
                        
                        player.transform.position = _exitPlayerPosition.transform.position;
                        
                        player.transform.SetParent(playerRoot.transform);
                        
                        //Debug.Log("Chegando aqui NULL");
                        player.transform.rotation = new Quaternion(0, 0, 0, 0);
                        //Debug.Log("player: " + player.transform.rotation);
                        break;
                    case GameMode.FirstPerson:
                        break;
                    default:
                        break;
                }
            } else
            {
                //Debug.Log("Algo de errado no swith");
            }

            if (_animalPosition != null)
            {
                var a_iaController = ControllerAnimalPuxador.GetComponent<IAAnimais>();
                var a_iaNavAgent = ControllerAnimalPuxador.GetComponent<NavMeshAgent>();

                //Define Animal novamente para o objeto pai
                ControllerAnimalPuxador.transform.SetParent(ControllerAnimalPuxador.GetComponent<AnimalPuxador>().gameObjectRoot);
                ControllerAnimalPuxador.GetComponent<Animator>().SetBool("IsControlled", false);
                a_iaController.enabled = true;
                a_iaNavAgent.enabled = true;


                c_WagonCotrolled.SetActive(false);
                c_WagonPulled.SetActive(true);
            }
        }

        // Configura Animal


        //yield return new WaitForSeconds(5f);


    }



    // Player Controla Carroca
    void SetupPlayerDriver()
    {
        if (vehicle.canDriveVehicle)
        {
            var a_iaController = ControllerAnimalPuxador.GetComponent<IAAnimais>();
            var a_iaNavAgent = ControllerAnimalPuxador.GetComponent<NavMeshAgent>();

            c_WagonCotrolled.SetActive(true);
            c_WagonPulled.SetActive(false);

            puxador.GetComponent<CapsuleCollider>().enabled = false;
            foreach (GameObject o in gameObjectsController) // Desabilita Objetos controlados pelo player
            {
                o.SetActive(true);
            }

            // Desabilita comportamento do animal
            a_iaController.enabled = false;
            a_iaNavAgent.enabled = false;

            if (gameObject.GetComponent<HingeJoint>() != null)
            {
                gameObject.GetComponent<HingeJoint>().connectedBody = null;
                gameObject.GetComponent<HingeJoint>().breakForce = 0;
            }

            if (_animalPosition != null) // Define Animal como filho da Carroca
            {
                ControllerAnimalPuxador.transform.SetParent(_animalPosition.transform);
            }

            // Configura Player
            if (gameManager.playerAtivo != null)
            {
                player = gameManager.playerAtivo.gameObject;
                playerRoot = player.transform.root.gameObject;
                switch (GameManager.gameMode)
                {
                    case GameMode.VR:
                        //Debug.Log("TEste");

                        player.GetComponent<Player>().EnableComponents(false, null);
                        player.GetComponent<Player>().canvasObject.transform.position = _playerPosition.transform.position;
                        player.GetComponent<Player>().canvasObject.transform.SetParent(_playerPosition.transform);
                        player.transform.position = _playerPosition.transform.position;
                        player.transform.rotation = _playerPosition.transform.rotation;
                        player.transform.SetParent(_playerPosition.transform);
                        player.GetComponent<Player>().Sentar(true);
                        break;
                    case GameMode.ThirdPerson:
                        setPivotDistance(new Vector3(0, 3, -2), new Vector3(30, 0, 0));
                        player.GetComponent<Player>().EnableComponents(false, player.GetComponent<Player>().rb);
                        player.transform.position = _playerPosition.transform.position;
                        player.transform.rotation = _playerPosition.transform.rotation;
                        player.transform.SetParent(_playerPosition.transform);
                        player.GetComponent<Player>().Sentar(true);
                        
                        break;
                    case GameMode.FirstPerson:
                        break;
                    default:
                        break;
                }
            }
            //yield return new WaitForSeconds(1);
        }

    }

    // Falta Implementar a Carona
    void WagonSetupRide()
    {
        ControllerAnimalPuxador.GetComponent<Animator>().SetBool("IsControlled", false);

        if (gameObject.GetComponent<HingeJoint>() == null)
        {
            var hingeJoint = gameObject.AddComponent<HingeJoint>();

            hingeJoint.anchor = new Vector3(0, -0.15f, 3.27f);
            hingeJoint.axis = new Vector3(0, 0, 0);
            hingeJoint.autoConfigureConnectedAnchor = true;

            JointLimits limits = hingeJoint.limits;
            limits.min = -111;
            limits.max = 17;
            limits.bounciness = 0;
            limits.contactDistance = 0;
            limits.bounceMinVelocity = 0.2f;
            hingeJoint.limits = limits;
            hingeJoint.useLimits = true;

            //presetWagonHingJoint.ApplyTo(gameObject.GetComponent<HingeJoint>());

            hingeJoint.connectedBody = puxador.GetComponent<Rigidbody>();

        }
        if (gameManager.playerAtivo != null)
        {
            player = gameManager.playerAtivo.gameObject;
            playerRoot = player.transform.root.gameObject;
            switch (GameManager.gameMode)
            {
                case GameMode.VR:
                    Debug.Log("TEste");

                    player.GetComponent<Player>().EnableComponents(false, null);
                    //player.GetComponent<Player>().canvasObject.transform.position = _playerPosition.transform.position;
                    player.GetComponent<Player>().SetPositionCanvasVR(_VRCanvasPosition);

                    player.transform.position = _playerPosition.transform.position;
                    player.transform.rotation = _playerPosition.transform.rotation;
                    player.transform.SetParent(_playerPosition.transform);                   

                    player.GetComponent<Player>().Sentar(true);
                    break;
                case GameMode.ThirdPerson:
                    player.GetComponent<Player>().EnableComponents(false, player.GetComponent<Player>().rb);
                    player.transform.position = _playerPosition.transform.position;
                    player.transform.SetParent(_playerPosition.transform);
                    player.transform.rotation = _playerPosition.transform.rotation;
                    player.GetComponent<Player>().Sentar(true);
                    setPivotDistance(new Vector3(0, 3, -2), new Vector3(30, 0, 0));
                    break;
                case GameMode.FirstPerson:
                    break;
                default:
                    break;
            }
        }
        //yield return new WaitForSeconds(1f);


    }
    #endregion


    void IADrive()
    {
        WhellsBehaviour();
    }

    //Inputs
    void PlayerDriverController()
    {
        //float h = CrossPlatformInputManager.GetAxis("Horizontal");
        //float v = CrossPlatformInputManager.GetAxis("Vertical");
#if MOBILE_INPUT
        angle = maxAngle * CrossPlatformInputManager.GetAxis("HorizontalVehicle");
        torque = maxTorque * CrossPlatformInputManager.GetAxis("VerticalVehicle");
#else
        angle = maxAngle * Input.GetAxis("Horizontal");
        torque = maxTorque * Input.GetAxis("Vertical");
#endif
        Debug.Log(torque);

        handBrake = Input.GetKey(KeyCode.X) ? brakeTorque : 0;
        m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);

        foreach (WheelCollider wheel in m_Wheels)
        {
            // A simple car where front wheels steer while rear ones drive.
            if (wheel.transform.localPosition.z > 0)
                wheel.steerAngle = angle;

            if (wheel.transform.localPosition.z < 0)
                wheel.brakeTorque = handBrake;

            if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
                wheel.motorTorque = torque;

            if (wheel.transform.localPosition.z >= 0 && driveType != DriveType.RearWheelDrive)
                wheel.motorTorque = torque;

            WhellsBehaviour();
        }
    }


    void setPivotDistance(Vector3 position, Vector3 rotation)
    {
        //Debug.Log(rotation);
        if (pivotCamera != null)
        {
            pivotCamera.transform.localPosition = position;
            pivotCamera.localRotation = Quaternion.Euler(rotation);
            //pivotCamera.Rotate(rotation);
            //Debug.Log(pivotCamera.rotation);
        }

    }


    /** Setup Wagon
     * 
    void ExitWagon()
    {

    }
    void SetupRideWagon()
    {
    }
    void SetupPlayerController()
    {
        Debug.LogError("Setup Player Controller");
        c_WagonCotrolled.SetActive(true);
        c_WagonPulled.SetActive(false);
        foreach (GameObject o in gameObjectsController)
        {
            o.SetActive(true);
        }

        gameObject.GetComponent<HingeJoint>().connectedBody = null;
        gameObject.GetComponent<HingeJoint>().breakForce = 0;

        puxador.GetComponent<CapsuleCollider>().enabled = false;
        if (_animalPosition != null)
        {
            ControllerAnimalPuxador.transform.SetParent(_animalPosition.transform);
        }
        if (gameManager.playerAtivo != null)
        {
            //player = gameManager.playerAtivo.gameObject;
            //playerRoot = player.transform.root.gameObject;
            switch (GameManager.gameMode)
            {
                case GameMode.VR:
                    Debug.Log("TEste");

                    player.GetComponent<Player>().EnableComponents(false, null);
                    player.GetComponent<Player>().canvasObject.transform.position = _playerPosition.transform.position;
                    player.GetComponent<Player>().canvasObject.transform.SetParent(_playerPosition.transform);
                    player.transform.position = _playerPosition.transform.position;
                    player.transform.SetParent(_playerPosition.transform);
                    player.GetComponent<Player>().Sentar(true);
                    break;
                case GameMode.ThirdPerson:
                    player.GetComponent<Player>().EnableComponents(false, player.GetComponent<Player>().rb);
                    player.transform.position = _playerPosition.transform.position;
                    player.transform.SetParent(_playerPosition.transform);
                    player.GetComponent<Player>().Sentar(true);
                    break;
                case GameMode.FirstPerson:
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("Ta vazio");
        }
    }
    */

    /*
    void Pilotar()
    {

        var a_iaController = ControllerAnimalPuxador.GetComponent<IAAnimais>();
        var a_iaNavAgent = ControllerAnimalPuxador.GetComponent<NavMeshAgent>();

        pesoCarroca.size = new Vector3(pesoCarroca.size.x, pesoCarroca.size.y, pesoCarroca.size.z + tamanhoCarroca);
        pesoCarroca.center = new Vector3(pesoCarroca.center.x, pesoCarroca.center.y, pesoCarroca.center.z - tamanhoCarroca);
        foreach (GameObject o in gameObjectsController)
        {
            o.SetActive(true);
        }
        a_iaController.enabled = false;
        a_iaNavAgent.enabled = false;
        gameObject.GetComponent<FixedJoint>().connectedBody = null;
        gameObject.GetComponent<FixedJoint>().breakForce = 0;
        if (_animalPosition != null)
        {
            ControllerAnimalPuxador.transform.SetParent(_animalPosition.transform);
        }
        if (gameManager.playerAtivo != null)
        {
            player = gameManager.playerAtivo.gameObject;
            playerRoot = player.transform.root.gameObject;
            switch (GameManager.gameMode)
            {
                case GameMode.VR:
                    Debug.Log("TEste");

                    player.GetComponent<Player>().EnableComponents(false, null);
                    player.GetComponent<Player>().canvasObject.transform.position = _playerPosition.transform.position;
                    player.GetComponent<Player>().canvasObject.transform.SetParent(_playerPosition.transform);
                    player.transform.position = _playerPosition.transform.position;
                    player.transform.SetParent(_playerPosition.transform);
                    player.GetComponent<Player>().Sentar();
                    break;
                case GameMode.ThirdPerson:
                    player.GetComponent<Player>().EnableComponents(false, player.GetComponent<Player>().rb);
                    playerRoot.transform.position = _playerPosition.transform.position;
                    playerRoot.transform.SetParent(_playerPosition.transform);
                    break;
                case GameMode.FirstPerson:
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("Ta vazio");
        }


    }
    */
}