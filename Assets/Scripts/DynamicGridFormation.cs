using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class DynamicGridFormation : MonoBehaviour
{
    public enum GridSize
    {
        None,
        ThreeCrossGride,
        FourCrossGride
    }
    [SerializeField] public GridSize _contentGridSize;
    [SerializeField] GridLayoutGroup gridBoardLayout;
    [SerializeField] GameObject blockerObject;
    [SerializeField] GameObject squareBoardGameObject;
    [SerializeField] Transform sqaureBoardTransform;

    void Start()
    {
        Debug.LogError(_contentGridSize.ToString());

        RectTransform _squareBoardGameObjectRectTransform = squareBoardGameObject.GetComponent<RectTransform>();
        if (_contentGridSize == GridSize.ThreeCrossGride)
        {
            Debug.LogWarning(_contentGridSize.ToString());  
            gridBoardLayout.constraintCount = 3;
            _squareBoardGameObjectRectTransform.sizeDelta = new Vector2(600, 600);
            SetThreeCrossGrideLayout(gridBoardLayout.constraintCount);
        }
        else if (_contentGridSize == GridSize.FourCrossGride)
        {
            Debug.LogWarning(_contentGridSize.ToString());
            gridBoardLayout.constraintCount = 4;
            _squareBoardGameObjectRectTransform.sizeDelta = new Vector2(800, 800);
            SetFourCrossGrideLayout(gridBoardLayout.constraintCount);
        }
    }
    void SetThreeCrossGrideLayout(int _size)
    {
        for(int i=0;i< _size * _size; i++)
        {
           GameObject obj = Instantiate(blockerObject, sqaureBoardTransform);
        }
    }
    void SetFourCrossGrideLayout(int _size)
    {
        for (int i = 0; i <_size * _size; i++)
        {
            GameObject obj = Instantiate(blockerObject, sqaureBoardTransform);
        }
    }
}
