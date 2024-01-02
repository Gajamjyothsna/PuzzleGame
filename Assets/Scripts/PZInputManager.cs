using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle2048
{
    public class PZInputManager : MonoBehaviour
    {
        #region Input Events
        public delegate void MoveLeft();
        public static event MoveLeft moveLeft;

        public static event MoveLeft moveRight;
        public static event MoveLeft moveUp;
        public static event MoveLeft moveDown;

        public delegate void InstanteObjects();
        public static event InstanteObjects instanteObjects;

        #endregion
        #region Singleton
        private static PZInputManager Instance;
        public static PZInputManager instance => Instance;
        #endregion
        #region Private Methods
        private bool isLeft;
        private bool isRight;
        private bool isUp;
        private bool isDown;
        #endregion
        #region Public Methods
        public bool IsLeft { get { return isLeft; } }
        public bool IsRight { get {  return isRight; } }
        public bool IsUp { get {  return isUp; } }
        public bool IsDown { get {  return isDown; } }
        #endregion
        #region Private Methods
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ResetAllTriggers();
                isLeft = true;
                moveLeft?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ResetAllTriggers();
                isRight = true;
                moveRight?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ResetAllTriggers();
                isUp = true;
                moveUp?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ResetAllTriggers();
                isDown = true;
                moveDown?.Invoke();
            }
            else if(Input.GetKeyDown(KeyCode.C))
            {
                instanteObjects?.Invoke(); 
            }
        }
        private void ResetAllTriggers()
        {
            isRight = false;
            isUp = false;
            isDown = false;
            isLeft = false;
            
        }
        #endregion
    }
}
