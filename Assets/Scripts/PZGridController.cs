using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle2048
{
    public class PZGridController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private Transform spawningPosition;
        [SerializeField] private PZTileObject _tile;
        [SerializeField] private GameObject _blockContainer;

        private Vector3 blockPosition;
        Transform randomBlockPosition;
        private GridLayoutGroup blockArea;
        private int blocksCount;

        List<int> randomIndexList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
        List<int> generatedIndexList = new List<int>();
        List<int> existedBlockList = new List<int>();
        private bool blockStatus;
        #endregion
        #region Subscribing Events
        private void OnEnable()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            PZInputManager.moveLeft += MoveBlocksToLeft;
            PZInputManager.moveRight += MoveBlocksToRight;
            PZInputManager.moveUp += MoveBlocksToUp;
            PZInputManager.moveDown += MoveBlocksToDown;
        }
        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        private void UnSubscribeEvents()
        {
            PZInputManager.moveLeft -= MoveBlocksToLeft;
            PZInputManager.moveRight -= MoveBlocksToRight;
            PZInputManager.moveUp -= MoveBlocksToUp;
            PZInputManager.moveDown -= MoveBlocksToDown;
        }

        #endregion
        #region Private Methods
        private void Awake()
        {
            blockArea = _blockContainer.GetComponent<GridLayoutGroup>();
            blocksCount = blockArea.transform.childCount;
        }
        internal void InitializeStartingBlocks(bool _threeGrid)
        {
            if (_threeGrid)
            {
                GetPositionToSpawnBlock(_threeGrid);
                for (int i = 0; i < generatedIndexList.Count; i++)
                {
                    randomBlockPosition = blockArea.transform.GetChild(generatedIndexList[i]);
                    PZTileObject obj = Instantiate(_tile, randomBlockPosition);
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
            if (_threeGrid)
            {
                GenerateUniqueRandomNumber();
            }
        }
        int _index;
        private void GenerateUniqueRandomNumber()
        {
            for (int i = 0; i < PZConstants.initialBlockCount; i++)
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
            for (int i = 0; i < generatedIndexList.Count; i++)
            {
                Debug.Log(generatedIndexList[i]);
            }
        }
        private void DebugExistedList()
        {
            for (int i = 0; i < existedBlockList.Count; i++)
            {
                Debug.Log(existedBlockList[i]);
            }
        }
        private void MoveBlocksToLeft()
        {
            ResetData();
            bool status = CheckAnyBlocksExisted();
            GetAlreadyExistedBlocks();
            for (int i = 0; i < existedBlockList.Count; i++)
            {
                if (existedBlockList[i] % PZConstants.gridMatrix == 1)
                {
                    Debug.LogError("Index" + existedBlockList[i]);
                    int previousIndex = existedBlockList[i];
                    PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                    Color _tileColor = obj.GetTileColor();
                    int _tileNumber = obj.GetTileNumber();
                    Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                    int currentIndex = existedBlockList[i] - 1;
                    randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                    tileObj.AssignData(_tileNumber, _tileColor);
                }
                else if (existedBlockList[i] % PZConstants.gridMatrix == 2)
                {
                    Debug.LogError("Index" + existedBlockList[i]);
                    int previousIndex = existedBlockList[i];
                    PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                    Color _tileColor = obj.GetTileColor();
                    int _tileNumber = obj.GetTileNumber();
                    Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                    int currentindex = existedBlockList[i] - 2;
                    randomBlockPosition = blockArea.transform.GetChild(currentindex);
                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                    tileObj.AssignData(_tileNumber, _tileColor);
                }
            }
        }
        private void MoveBlocksToRight()
        {
            ResetData();
            bool status = CheckAnyBlocksExisted();
            GetAlreadyExistedBlocks();
            for (int i = 0; i < existedBlockList.Count; i++)
            {
                if (existedBlockList[i] % PZConstants.gridMatrix == 1)
                {
                    Debug.LogError("Right Index" + existedBlockList[i]);
                    int previousIndex = existedBlockList[i];
                    PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                    Color _tileColor = obj.GetTileColor();
                    int _tileNumber = obj.GetTileNumber();
                    Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                    int currentIndex = existedBlockList[i] + 1;
                    randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                    tileObj.AssignData(_tileNumber, _tileColor);
                }
                else if (existedBlockList[i] % PZConstants.gridMatrix == 0)
                {
                    Debug.LogError("Right Index" + existedBlockList[i]);
                    int previousIndex = existedBlockList[i];
                    PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                    Color _tileColor = obj.GetTileColor();
                    int _tileNumber = obj.GetTileNumber();
                    Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                    int currentindex = existedBlockList[i] + 2;
                    randomBlockPosition = blockArea.transform.GetChild(currentindex);
                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                    tileObj.AssignData(_tileNumber, _tileColor);
                }
            }
        }
        private void MoveBlocksToUp()
        {
            ResetData();
            bool status = CheckAnyBlocksExisted();
            GetAlreadyExistedBlocks();
            for (int i = 0; i < existedBlockList.Count; i++)
            {
                if (existedBlockList[i] % PZConstants.gridMatrix == 1)
                {
                    if (existedBlockList[i] % PZConstants.secondRowMultiplier == 0)
                    {
                        Debug.LogError("Right Index" + existedBlockList[i]);
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        int currentIndex = existedBlockList[i] - PZConstants.gridMatrix;
                        randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                        tileObj.AssignData(_tileNumber, _tileColor);
                    }
                    else if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.gridMatrix)
                    {
                        Debug.LogError("Right Index" + existedBlockList[i]);
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        int currentindex = existedBlockList[i] - (PZConstants.gridMatrix + PZConstants.gridMatrix);
                        randomBlockPosition = blockArea.transform.GetChild(currentindex);
                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                        tileObj.AssignData(_tileNumber, _tileColor);
                    }

                }
                else if(existedBlockList[i] % PZConstants.gridMatrix == 0)
                {
                    if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.gridMatrix)
                    {
                        Debug.LogError("Right Index" + existedBlockList[i]);
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        int currentIndex = existedBlockList[i] - PZConstants.gridMatrix;
                        randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                        tileObj.AssignData(_tileNumber, _tileColor);
                    }
                    else if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.initialBlockCount)
                    {
                        Debug.LogError("Right Index" + existedBlockList[i]);
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        int currentindex = existedBlockList[i] - (PZConstants.gridMatrix + PZConstants.gridMatrix);
                        randomBlockPosition = blockArea.transform.GetChild(currentindex);
                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                        tileObj.AssignData(_tileNumber, _tileColor);
                    }
                }
                else if (existedBlockList[i] % PZConstants.gridMatrix == 2)
                {
                    if (existedBlockList[i] % PZConstants.secondRowMultiplier == 1)
                    {
                        Debug.LogError("Right Index" + existedBlockList[i]);
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        int currentIndex = existedBlockList[i] - PZConstants.gridMatrix;
                        randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                        tileObj.AssignData(_tileNumber, _tileColor);
                    }
                    else if (existedBlockList[i] % PZConstants.secondRowMultiplier == 0)
                    {
                        Debug.LogError("Right Index" + existedBlockList[i]);
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        int currentindex = existedBlockList[i] - (PZConstants.gridMatrix + PZConstants.gridMatrix);
                        randomBlockPosition = blockArea.transform.GetChild(currentindex);
                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                        tileObj.AssignData(_tileNumber, _tileColor);
                    }
                }

            }
        }
        private void MoveBlocksToDown()
        {
            ResetData();
            bool status = CheckAnyBlocksExisted();
            GetAlreadyExistedBlocks();
            for (int i = 0; i < existedBlockList.Count; i++)
            {
                if (existedBlockList[i] % PZConstants.gridMatrix == 1)
                {
                    if (existedBlockList[i] % PZConstants.secondRowMultiplier == 0)
                    {
                        Debug.LogError("Right Index" + existedBlockList[i]);
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        int currentIndex = existedBlockList[i] + PZConstants.gridMatrix;
                        randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                        tileObj.AssignData(_tileNumber, _tileColor);
                    }
                    else if (existedBlockList[i] % PZConstants.secondRowMultiplier == 1)
                    {
                        Debug.LogError("Right Index" + existedBlockList[i]);
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        int currentindex = existedBlockList[i] + (PZConstants.gridMatrix + PZConstants.gridMatrix);
                        randomBlockPosition = blockArea.transform.GetChild(currentindex);
                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                        tileObj.AssignData(_tileNumber, _tileColor);
                    }
                }
                else if (existedBlockList[i] % PZConstants.gridMatrix == 0)
                {
                    if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.gridMatrix)
                    {
                        Debug.LogError("Right Index" + existedBlockList[i]);
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        int currentIndex = existedBlockList[i] + PZConstants.gridMatrix;
                        randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                        tileObj.AssignData(_tileNumber, _tileColor);
                    }
                    else if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.secondRowMultiplier)
                    {
                        Debug.LogError("Right Index" + existedBlockList[i]);
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        int currentindex = existedBlockList[i] + (PZConstants.gridMatrix + PZConstants.gridMatrix);
                        randomBlockPosition = blockArea.transform.GetChild(currentindex);
                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                        tileObj.AssignData(_tileNumber, _tileColor);
                    }
                }
                else if (existedBlockList[i] % PZConstants.gridMatrix == 2)
                {
                    if (existedBlockList[i] % PZConstants.secondRowMultiplier == 1)
                    {
                        Debug.LogError("Right Index" + existedBlockList[i]);
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        int currentIndex = existedBlockList[i] + PZConstants.gridMatrix;
                        randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                        tileObj.AssignData(_tileNumber, _tileColor);
                    }
                    else if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.initialBlockCount)
                    {
                        Debug.LogError("Right Index" + existedBlockList[i]);
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        int currentindex = existedBlockList[i] + (PZConstants.gridMatrix + PZConstants.gridMatrix);
                        randomBlockPosition = blockArea.transform.GetChild(currentindex);
                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                        tileObj.AssignData(_tileNumber, _tileColor);
                    }
                }
            }
        }
        private bool CheckAnyBlocksExisted()
        {
            for (int i = 0; i < blocksCount; i++)
            {
                int _count = blockArea.transform.GetChild(i).gameObject.transform.childCount;
                if (_count > 0)
                {
                    blockStatus = true;
                }
                else
                {
                    blockStatus = false;
                }
            }
            return blockStatus;
        }
        private void GetAlreadyExistedBlocks()
        {
            for (int i = 0; i < blocksCount; i++)
            {
                int _count = blockArea.transform.GetChild(i).gameObject.transform.childCount;
                if (_count > 0)
                {
                    existedBlockList.Add(i);
                }
            }
        }
        private void ResetData()
        {
            existedBlockList.Clear();
        }
        #endregion
    }
}
