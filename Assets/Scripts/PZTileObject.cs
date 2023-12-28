using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle2048
{
    public class PZTileObject : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private Text numberText;
        [SerializeField] private Image numberBgColor;

        private PZGameManager.Number _SetNumber;

        #endregion
        #region Public Methods
        internal void InitData(bool _initialBlock)
        {
            if (_initialBlock)
            {
                List<int> initalNumberList = new List<int> { 2, 4 };
                int _generatedIndex = UnityEngine.Random.Range(0, initalNumberList.Count);
                UnityEngine.Debug.Log("generatedNumber" + _generatedIndex);
                numberText.text = initalNumberList[_generatedIndex].ToString();
                switch (initalNumberList[_generatedIndex])
                {
                    case 2:
                        _SetNumber = PZGameManager.Number.Two;
                        break;
                    case 4:
                        _SetNumber = PZGameManager.Number.Four;
                        break;
                }
                numberBgColor.color = PZGameManager.Instance.AssignNumberColorForBg(_SetNumber);
            }
        }
        internal int GetTileNumber()
        {
            return int.Parse(numberText.text);
        }
        internal Color GetTileColor()
        {
            return numberBgColor.color;
        }
        internal void AssignData(int _number, Color _color)
        {
            numberText.text = _number.ToString();
            numberBgColor.color = _color;
        }



        #endregion
        #region Private Methods
        #endregion

    }
}
