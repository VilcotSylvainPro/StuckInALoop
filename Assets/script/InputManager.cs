using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private GameManager GameManager;

    private Vector2 Direction = Vector2.zero;
    private bool Jumping = false;
    private bool Shooting = false;
    private Vector2 CursorPositionVector = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        Direction = context.ReadValue<Vector2>();
        GameManager.Move(Direction);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Jumping = context.ReadValueAsButton();
        GameManager.Jump(Jumping);
    }

    public void DepressionGunShot(InputAction.CallbackContext context)
    {
        Shooting = context.ReadValueAsButton();
        GameManager.DepressionGunShot(Shooting);
    }

    public void CursorPosition(InputAction.CallbackContext context) 
    {
        CursorPositionVector = context.ReadValue<Vector2>();
        GameManager.CursorPosition(CursorPositionVector);
    }
}
