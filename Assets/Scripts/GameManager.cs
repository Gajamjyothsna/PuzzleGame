using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GridSize
    {
        None,
        ThreeCrossGride,
        FourCrossGride
    }

    #region PUBLIC VARIABLES
    [SerializeField] public GridSize _contentGridSize;
    [SerializeField] GridLayoutGroup gridBoardLayout;
    [SerializeField] GameObject blockerObject;
    [SerializeField] GameObject squareBoardGameObject;
    [SerializeField] public Transform sqaureBoardTransform;
    #endregion

    #region PRIVATE VARIABLES
    private bool threeGrid;
    #endregion
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        Debug.LogError(_contentGridSize.ToString());
        _contentGridSize = GridSize.ThreeCrossGride;
        RectTransform _squareBoardGameObjectRectTransform = squareBoardGameObject.GetComponent<RectTransform>();
        var spawingBlocks = squareBoardGameObject.GetComponent<SpawningBlocks>();
        if (_contentGridSize == GridSize.ThreeCrossGride)
        {
            Debug.LogWarning(_contentGridSize.ToString());  
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

    
   
}
