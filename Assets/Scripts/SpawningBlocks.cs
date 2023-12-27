using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ObjectPooling;

public class SpawningBlocks : MonoBehaviour
{
    [SerializeField] private Transform spawningPosition;
    [SerializeField] private TileObject _tile;
    [SerializeField] private GameObject _blockContainer;

    private Vector3 blockPosition;
    Transform randomBlockPosition;
    private GridLayoutGroup blockArea;
    private int blocksCount;

    List<int> randomIndexList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
    List<int> generatedIndexList = new List<int>();

    private void Awake()
    {
        blockArea = _blockContainer.GetComponent<GridLayoutGroup>();
        blocksCount = blockArea.transform.childCount;
    }
    internal void InitializeStartingBlocks(bool _threeGrid)
    {
        if(_threeGrid)
        {
            GetPositionToSpawnBlock(_threeGrid);
            for (int i = 0;i<generatedIndexList.Count;i++)
            {
                randomBlockPosition = blockArea.transform.GetChild(generatedIndexList[i]);
                TileObject obj = Instantiate(_tile, randomBlockPosition);
                obj.InitData(true);
                Debug.Log("InitializeStartingBlocks");
            }
        }
        else
        {
            GetPositionToSpawnBlock(_threeGrid);
        }
    }
    int previousRandomIndex;
    private void GetPositionToSpawnBlock(bool _threeGrid)
    {
        if ( _threeGrid)
        {
            GenerateUniqueRandomNumber();
            DebugList();
        }
    }
    int _index;
    private void GenerateUniqueRandomNumber()
    {
        for (int i = 0;i<2;i++)
        {
            _index = Random.Range(0, randomIndexList.Count);
            generatedIndexList.Add(_index);
            Debug.Log("index" + _index);
        }
        if (generatedIndexList[0] == generatedIndexList[1])
        {
            randomIndexList.RemoveAt(generatedIndexList[1]);
            generatedIndexList.RemoveAt(1);
            _index = Random.Range(0, randomIndexList.Count);
            generatedIndexList.Add(_index);
        }

    }
    private void DebugList()
    {
        for(int i = 0;i<generatedIndexList.Count;i++)
        {
            Debug.Log(generatedIndexList[i]);
        }
    }
}
