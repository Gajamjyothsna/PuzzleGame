using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZInputManager : MonoBehaviour
{
    #region Input Events
    public delegate void MoveLeft();
    public static event MoveLeft moveLeft;

    public static event MoveLeft moveRight;
    public static event MoveLeft moveUp;
    public static event MoveLeft moveDown;

    #endregion
    #region Singleton
    private static PZInputManager Instance;
    public static PZInputManager instance => Instance;
    #endregion
    #region Private Methods
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveLeft?.Invoke();
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveRight?.Invoke();
        }
        else if( Input.GetKeyDown(KeyCode.UpArrow))
        { 
            moveUp?.Invoke();
        }
        else if(Input.GetKeyDown (KeyCode.DownArrow))
        {
            moveDown?.Invoke();
        }
    }
    #endregion
}
