using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    CursorLockMode wantedMode;
    public GameManager gm;
    
    // Apply requested cursor state
    void SetCursorState()
    {
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }
    private void Awake()
    {
        if (GameManager.gameMode.Equals(GameMode.VR))
        {
            wantedMode = CursorLockMode.Locked;
            SetCursorState();
        }        
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = wantedMode = CursorLockMode.None;
    }
}