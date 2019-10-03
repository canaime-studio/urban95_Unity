using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum GameMode
{
    VR,
    ThirdPerson,
    FirstPerson,

}

public class GameManager : MonoBehaviour
{
    public static GameMode gameMode;
    public Player VRPlayer;
    public Player ThirdPersonPlayer;
    public GameObject[] GameObjectsVR;
    public GameObject[] GameObjects3P;
    public static string texto;
    public Player playerAtivo;


    public GameMode gameSelectModeEditor;
    public bool managerEditor;

    private void Awake()
    {
        if (managerEditor)
        {
            Debug.Log(gameSelectModeEditor.ToString());
            GameManager.gameMode = gameSelectModeEditor;
        }
        
        //Debug.Log("Jogando em: " + gameMode.ToString());

        //DontDestroyOnLoad(transform.gameObject);
        if (gameMode.Equals(GameMode.VR))
        {
            ConfigureManager(VRPlayer);
            ManagerPlayerController(VRPlayer, GameObjectsVR, true);
            ManagerPlayerController(ThirdPersonPlayer, GameObjects3P, false);
        }
        else if (gameMode.Equals(GameMode.ThirdPerson))
        {
            ConfigureManager(ThirdPersonPlayer);
            ManagerPlayerController(ThirdPersonPlayer, GameObjects3P, true);
            ManagerPlayerController(VRPlayer, GameObjectsVR, false);
        }
    }
    public void ManagerPlayerController(Player player, GameObject[] controllersAux, bool active)
    {
        if (player != null)
        {
            player.PrefabPlayer.SetActive(active);
        }
        if (controllersAux != null)
        {
            foreach (var g in controllersAux)
            {
                g.SetActive(active);
            }
        }
    }    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerController"></param>
    public void ConfigureManager(Player playerController)
    {        
        if (playerAtivo == null)
        {
            playerAtivo = playerController;
        }
    }
}