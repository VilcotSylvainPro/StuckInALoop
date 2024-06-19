using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player PlayerScript;
    private InputManager InputManager;

    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = GameObject.Find("Player").GetComponent<Player>();
        InputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector2 Direction)
    {
        PlayerScript.movingPlayer(Direction);
    }

    public void Jump(bool State)
    {
        PlayerScript.Jump(State);
    }

    public void DepressionGunShot(bool State)
    {
        PlayerScript.DepressionGunShot(State);
    }

    public void CursorPosition(Vector2 CursorPositionVector) 
    {
        PlayerScript.CursorPosition(CursorPositionVector);
    }


}
