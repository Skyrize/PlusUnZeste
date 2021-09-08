using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInputManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    public void EnableUIActionMap(InputAction.CallbackContext context)
    {
        if (context.canceled && playerInput.currentActionMap.name != "UI")
            playerInput.SwitchCurrentActionMap("UI");
    }
    public void EnablePlayerActionMap(InputAction.CallbackContext context)
    {
        if (context.canceled && playerInput.currentActionMap.name != "Player")
            playerInput.SwitchCurrentActionMap("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
