using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
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
    public enum GridSize
    {
        None,
        ThreeCrossGride,
        FourCrossGride
    }

    #region PUBLIC VARIABLES
    public Number _setNumber;
    #endregion
    #region PRIVATE VARIABLES
    [SerializeField] public GridSize _contentGridSize;
    [SerializeField] GridLayoutGroup gridBoardLayout;
    [SerializeField] GameObject blockerObject;
    [SerializeField] GameObject squareBoardGameObject;
    [SerializeField] public Transform sqaureBoardTransform;
    [SerializeField] private List<NumberColors> numberColorsData;
    private bool threeGrid;
    #endregion
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        _contentGridSize = GridSize.ThreeCrossGride;
        RectTransform _squareBoardGameObjectRectTransform = squareBoardGameObject.GetComponent<RectTransform>();
        var spawingBlocks = squareBoardGameObject.GetComponent<SpawningBlocks>();
        if (_contentGridSize == GridSize.ThreeCrossGride)
        {
            gridBoardLayout.constraintCount = 3;
            _squareBoardGameObjectRectTransform.sizeDelta = new Vector2(600, 600);
            spawingBlocks.InitializeStartingBlocks(true);
        }
        else if (_contentGridSize == GridSize.FourCrossGride)
        {
            Debug.LogWarning(_contentGridSize.ToString());
            gridBoardLayout.constraintCount = 4;
            _squareBoardGameObjectRectTransform.sizeDelta = new Vector2(800, 800);
            spawingBlocks.InitializeStartingBlocks(false);
        }
    }
    public Color AssignNumberColorForBg(Number _colorNumber)
    {
        return numberColorsData.Find(x => x._number == _colorNumber).color;
    }

    
   
}
