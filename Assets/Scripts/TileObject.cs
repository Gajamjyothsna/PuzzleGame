using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class TileObject : MonoBehaviour
{
    #region Private Variables
    [SerializeField] private Text numberText;
    [SerializeField] private Image numberBgColor;

    private GameManager.Number _SetNumber;
    
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
            switch(initalNumberList[_generatedIndex])
            {
                case 2:
                    _SetNumber = GameManager.Number.Two;
                    break;
                case 4:
                    _SetNumber = GameManager.Number.Four;
                    break;
            }
            numberBgColor.color = GameManager.Instance.AssignNumberColorForBg(_SetNumber);
        }
    }
   

    #endregion
    #region Private Methods
    #endregion

}
