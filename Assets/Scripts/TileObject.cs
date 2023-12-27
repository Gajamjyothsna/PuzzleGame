using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileObject : MonoBehaviour
{
    #region Data Class
    [Serializable]
    public class NumberColors
    {
        public Color color;
        public Number _number;
    }
    [Serializable]
    public enum Number
    {
        None,
        Two,
        Four,
        Eight,
        Sixteen,
        ThirtyTwo,
        SixtyFour,
        OneHundredTwentyEight,
        TwoFiftySix,
        FiveTwelve,
        OneZeroTwoFour,
        TwoZeroFourEight

    }
    #endregion
    #region Private Variables
    [SerializeField] private Text numberText;
    [SerializeField] private Image numberBgColor;
    [SerializeField] private List<NumberColors> numberColorsData;
    #endregion
    #region Public Methods
    #endregion
    #region Private Methods
    #endregion

}
