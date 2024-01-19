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
            PZInputManager.instanteObjects += InstanteObjects;
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
            PZInputManager.instanteObjects -= InstanteObjects;
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
                Debug.Log("both are indexs are same");
                Debug.LogError("index at last" + randomIndexList[generatedIndexList[1]]);
                Debug.LogError("generation list last index " + generatedIndexList[1]);
                randomIndexList.RemoveAt(randomIndexList[generatedIndexList[1]]);
                generatedIndexList.RemoveAt(1);
                _index = Random.Range(0, randomIndexList.Count);
                Debug.LogError("Next generated Number" + _index);
                generatedIndexList.Add(_index);
                DebugList();
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
            bool currentIndexStatus;
            ResetExistedBlockData();
            GetAlreadyExistedBlocks();
            ResetBoolValues();
            bool status = CheckTwoLeftIndexs();
            if (status)
            {
                int _leftDestinationIndex;

                if(CheckBlockExistedAtSpecifiedIndex(0))
                {
                    _leftDestinationIndex = 0;
                }
                else if(CheckBlockExistedAtSpecifiedIndex(3))
                {
                    _leftDestinationIndex = PZConstants.gridMatrix;
                }
                else
                {
                    _leftDestinationIndex = PZConstants.gridMatrix + PZConstants.gridMatrix;
                }

                PZTileObject tileObject = blockArea.transform.GetChild(_leftDestinationIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                int tileNumber = tileObject.GetTileNumber();

                PZTileObject nextTileObject = blockArea.transform.GetChild(_leftDestinationIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                int _tileNumber = nextTileObject.GetTileNumber();

                if(_tileNumber == tileNumber)
                {
                    Destroy(blockArea.transform.GetChild(_leftDestinationIndex + 1).gameObject.transform.GetChild(0).gameObject);
                    MergeBlocks(tileNumber, _leftDestinationIndex, true);
                }

                return;
            }
            else
            {
                for (int i = 0; i < existedBlockList.Count; i++)
                {
              
                    if (existedBlockList[i] % PZConstants.gridMatrix == 1)
                    {
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        int currentIndex = existedBlockList[i] - 1;
                        if (!CheckBlockExistedAtSpecifiedIndex(currentIndex))
                        {
                            int nextToCurrentIndex = existedBlockList[i] + 1;
                            if (CheckBlockExistedAtSpecifiedIndex(nextToCurrentIndex))
                            {
                                int nextIndex = existedBlockList[i] + 1;
                                PZTileObject obj1 = blockArea.transform.GetChild(nextIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                int tileNumber = obj1.GetTileNumber();
                                Color tileColor = obj1.GetTileColor();
                                Destroy(blockArea.transform.GetChild(nextIndex).gameObject.transform.GetChild(0).gameObject);

                                if (_tileNumber == tileNumber)
                                {
                                    Debug.Log("Merge Blocks");
                                    Debug.LogError("Merge Blocks Left : " + "SourceIndex" + " " + previousIndex + " " + "destination Index" + currentIndex);
                                    MergeBlocks(_tileNumber, currentIndex, false);
                                }
                                else
                                {
                                    Transform nextRandomPosition = blockArea.transform.GetChild(previousIndex);
                                    PZTileObject nextTileObj = Instantiate(_tile, nextRandomPosition);
                                    nextTileObj.AssignData(tileNumber, tileColor);

                                    randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);
                                }
                               
                            }
                            else
                            {
                                randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                tileObj.AssignData(_tileNumber, _tileColor);
                            }
                        }
                    }

                    else if (existedBlockList[i] % PZConstants.gridMatrix == 2)
                    {
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        int currentindex = existedBlockList[i] - 2;
                        if (CheckBlockExistedAtSpecifiedIndex(currentindex))
                        {
                            int nextToCurrentIndex = currentindex + 1;
                            bool nextTocurrentIndexStatus = CheckBlockExistedAtSpecifiedIndex(nextToCurrentIndex);
                            if (!CheckBlockExistedAtSpecifiedIndex(nextToCurrentIndex))
                            {
                                currentindex = nextToCurrentIndex;
                                randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                tileObj.AssignData(_tileNumber, _tileColor);
                            }
                        }
                        else
                        {
                            currentIndexStatus = CheckBlockExistedAtSpecifiedIndex(currentindex);
                            int nextToCurrentIndex = currentindex + 1;
                            bool nextCurrentIndexStatusElse = CheckBlockExistedAtSpecifiedIndex(nextToCurrentIndex);
                            if (!CheckBlockExistedAtSpecifiedIndex(nextToCurrentIndex))
                            {
                                randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                tileObj.AssignData(_tileNumber, _tileColor);
                            }
                        }

                    }

                }
            }
        }
        private void MoveBlocksToRight()
        {
            ResetExistedBlockData();
            GetAlreadyExistedBlocks();
            ResetBoolValues();
            bool status = CheckTwoRightIndexs();
            if (status)
            {
                int _rightDestinationIndex;

                if(CheckBlockExistedAtSpecifiedIndex(PZConstants.initialBlockCount))
                {
                    _rightDestinationIndex = PZConstants.initialBlockCount;
                }
                else if(CheckBlockExistedAtSpecifiedIndex(PZConstants.initialBlockCount + PZConstants.gridMatrix))
                {
                    _rightDestinationIndex = PZConstants.initialBlockCount + PZConstants.gridMatrix;
                }
                else
                {
                    _rightDestinationIndex = PZConstants.initialBlockCount + PZConstants.gridMatrix + PZConstants.gridMatrix;
                }
                PZTileObject nextTileObject = blockArea.transform.GetChild(_rightDestinationIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                int _tileNumber = nextTileObject.GetTileNumber();

                PZTileObject obj = blockArea.transform.GetChild(_rightDestinationIndex - 1).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                int tileNumber = obj.GetTileNumber();

                if (_tileNumber == tileNumber)
                {
                    Destroy(blockArea.transform.GetChild(_rightDestinationIndex - 1).gameObject.transform.GetChild(0).gameObject);
                    MergeBlocks(_tileNumber, _rightDestinationIndex, true);
                }
                return;
            }
            else
            {

                for (int i = 0; i < existedBlockList.Count; i++)
                {
                    if (existedBlockList[i] % PZConstants.gridMatrix == 1)
                    {
                        int _indexCount = 0;
                        int currentIndex = existedBlockList[i] + 1;
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        if (!CheckBlockExistedAtSpecifiedIndex(currentIndex))
                        {
                            int nextToCurrentIndex = existedBlockList[i] - 1;
                            if (CheckBlockExistedAtSpecifiedIndex(nextToCurrentIndex))
                            {
                                Debug.Log("if case - index 1");
                                int nextIndex = existedBlockList[i] - 1;
                                PZTileObject obj1 = blockArea.transform.GetChild(nextIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                int tileNumber = obj1.GetTileNumber();
                                Color tileColor = obj1.GetTileColor();
                                Destroy(blockArea.transform.GetChild(nextIndex).gameObject.transform.GetChild(0).gameObject);

                                if(tileNumber == _tileNumber)
                                {
                                    Debug.Log("Merge Blocks");
                                    Debug.LogError("Merge Blocks right : " + "SourceIndex" + " " +  + previousIndex +" " + "destination Index" + currentIndex);
                                    MergeBlocks(_tileNumber, currentIndex, false);
                                }
                                else
                                {
                                    randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);

                                    Transform nextRandomPosition = blockArea.transform.GetChild(previousIndex);
                                    PZTileObject nextTileObj = Instantiate(_tile, nextRandomPosition);
                                    nextTileObj.AssignData(tileNumber, tileColor);
                                }

                            }
                            else
                            {
                                _indexCount = _indexCount + 1;
                                if(_indexCount == 1)
                                {
                                    randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);
                                }
                            }
                        }
                    }
                    else if (existedBlockList[i] % PZConstants.gridMatrix == 0)
                    {
                        int _indexCount = 0;
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);

                        int currentindex = existedBlockList[i] + 2;
                        if(!CheckBlockExistedAtSpecifiedIndex(currentindex))
                        {
                            int nextCurrentIndex = existedBlockList[i] + 1;
                            if(CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                            {
                                int nextIndex = existedBlockList[i] + 1;
                                PZTileObject obj1 = blockArea.transform.GetChild(nextIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                int tileNumber = obj1.GetTileNumber();
                                Color tileColor = obj1.GetTileColor();
                                Destroy(blockArea.transform.GetChild(nextIndex).gameObject.transform.GetChild(0).gameObject);
                                if (tileNumber == _tileNumber)
                                {
                                    Debug.Log("Merge Blocks");
                                    Debug.LogError("Merge Blocks Left : " + "SourceIndex" + " " + nextCurrentIndex + " " + "destination Index" + currentindex);
                                    MergeBlocks(_tileNumber, currentindex, false);
                                }
                                else
                                {
                                    randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(tileNumber, tileColor);

                                    Transform nextRandomPosition = blockArea.transform.GetChild(nextIndex);
                                    PZTileObject nextTileObj = Instantiate(_tile, nextRandomPosition);
                                    nextTileObj.AssignData(_tileNumber, _tileColor);
                                }

                            }
                            else
                            {
                                _indexCount = _indexCount + 1;
                                if(_indexCount == 1)
                                {
                                    randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);
                                }
                            }
                        }
                       
                    }
                }
            }
        }
        private void MoveBlocksToUp()
        {
            ResetExistedBlockData();
            GetAlreadyExistedBlocks();
            ResetBoolValues();
            bool checkUpBlockStatus = CheckTwoUpIndexs();
            Debug.Log("CheckUpBlockStatus" + checkUpBlockStatus);
            if (checkUpBlockStatus)
            {
                return;
            }
            else
            {
                for (int i = 0; i < existedBlockList.Count; i++)
                {
                    if (existedBlockList[i] % PZConstants.gridMatrix == 1)
                    {
                        if (existedBlockList[i] % PZConstants.secondRowMultiplier == 0)  //Index 4
                        {
                            int _indexCount = 0;
                            int previousIndex = existedBlockList[i];
                            PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                            Color _tileColor = obj.GetTileColor();
                            int _tileNumber = obj.GetTileNumber();
                            Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                            int currentIndex = existedBlockList[i] - PZConstants.gridMatrix;
                            if(!CheckBlockExistedAtSpecifiedIndex(currentIndex))
                            {
                                int nextCurrentIndex = existedBlockList[i] + PZConstants.gridMatrix;
                                if(CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                                {
                                    PZTileObject tempObj = blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                    Color tileColor = tempObj.GetTileColor();
                                    int tileNumber = tempObj.GetTileNumber();
                                    Destroy(blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).gameObject);
                                    if (tileNumber == _tileNumber)
                                    {
                                        MergeBlocks(_tileNumber, currentIndex, false);
                                    }
                                    else
                                    {
                                        randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                        tileObj.AssignData(_tileNumber, _tileColor);

                                        Transform previousRandomPosition = blockArea.transform.GetChild(previousIndex);
                                        PZTileObject tileObject = Instantiate(_tile, previousRandomPosition);
                                        tileObject.AssignData(tileNumber, tileColor);
                                    }

                                }
                                else
                                {
                                    _indexCount = _indexCount + 1;
                                    if (_indexCount == 1)
                                    {
                                        randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                        PZTileObject tileObj1 = Instantiate(_tile, randomBlockPosition);
                                        tileObj1.AssignData(_tileNumber, _tileColor);
                                    }
                                }
                            }
                        }
                        else if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.gridMatrix) //index 7
                        {
                            int _indexCount = 0;
                            int previousIndex = existedBlockList[i];
                            PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                            Color _tileColor = obj.GetTileColor();
                            int _tileNumber = obj.GetTileNumber();
                            Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                            int currentindex = existedBlockList[i] - (PZConstants.gridMatrix + PZConstants.gridMatrix);
                            if(CheckBlockExistedAtSpecifiedIndex(currentindex))
                            {
                                int nextCurrentIndex = existedBlockList[i] - PZConstants.gridMatrix;
                                if(!CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                                {
                                    randomBlockPosition = blockArea.transform.GetChild(nextCurrentIndex);
                                    PZTileObject tileObj1 = Instantiate(_tile, randomBlockPosition);
                                    tileObj1.AssignData(_tileNumber, _tileColor);
                                }
                            }
                            else
                            {
                                _indexCount = _indexCount + 1;
                                if(_indexCount == 1)
                                {
                                    randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                    PZTileObject tileObj1 = Instantiate(_tile, randomBlockPosition);
                                    tileObj1.AssignData(_tileNumber, _tileColor);
                                }
                            }
                        }

                    }
                    else if (existedBlockList[i] % PZConstants.gridMatrix == 0)
                    {
                        if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.gridMatrix) //Index 3
                        {
                            int previousIndex = existedBlockList[i];
                            PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                            Color _tileColor = obj.GetTileColor();
                            int _tileNumber = obj.GetTileNumber();
                            Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                            int currentIndex = existedBlockList[i] - PZConstants.gridMatrix;
                            if (!CheckBlockExistedAtSpecifiedIndex(currentIndex))
                            {
                                int nextCurrentIndex = existedBlockList[i] + PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                                {
                                    PZTileObject tempObj = blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                    Color tileColor = tempObj.GetTileColor();
                                    int tileNumber = tempObj.GetTileNumber();
                                    Destroy(blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).gameObject);

                                    if (tileNumber == _tileNumber)
                                    {
                                        MergeBlocks(_tileNumber, currentIndex, false);
                                    }
                                    else
                                    {
                                        randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                        tileObj.AssignData(_tileNumber, _tileColor);

                                        Transform previousRandomPosition = blockArea.transform.GetChild(previousIndex);
                                        PZTileObject tileObject = Instantiate(_tile, previousRandomPosition);
                                        tileObject.AssignData(tileNumber, tileColor);
                                    }
                                }
                                else
                                {
                                    randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);
                                }
                            }
                        }
                        else if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.initialBlockCount) //Index 6
                        {
                            int previousIndex = existedBlockList[i];
                            PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                            Color _tileColor = obj.GetTileColor();
                            int _tileNumber = obj.GetTileNumber();
                            Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                            int currentindex = existedBlockList[i] - (PZConstants.gridMatrix + PZConstants.gridMatrix);
                            if (CheckBlockExistedAtSpecifiedIndex(currentindex))
                            {
                                int nextCurrentIndex = existedBlockList[i] - PZConstants.gridMatrix;
                                if (!CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                                {
                                    randomBlockPosition = blockArea.transform.GetChild(nextCurrentIndex);
                                    PZTileObject tileObj1 = Instantiate(_tile, randomBlockPosition);
                                    tileObj1.AssignData(_tileNumber, _tileColor);
                                }
                            }
                            else
                            {
                                randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                PZTileObject tileObj1 = Instantiate(_tile, randomBlockPosition);
                                tileObj1.AssignData(_tileNumber, _tileColor);
                            }
                        }
                    }
                    else if (existedBlockList[i] % PZConstants.gridMatrix == 2)
                    {
                        if (existedBlockList[i] % PZConstants.secondRowMultiplier == 1) //Index 5
                        {
                            int previousIndex = existedBlockList[i];
                            PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                            Color _tileColor = obj.GetTileColor();
                            int _tileNumber = obj.GetTileNumber();
                            Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                            int currentIndex = existedBlockList[i] - PZConstants.gridMatrix;
                            if (!CheckBlockExistedAtSpecifiedIndex(currentIndex))
                            {
                                int nextCurrentIndex = existedBlockList[i] + PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                                {
                                    PZTileObject tempObj = blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                    Color tileColor = tempObj.GetTileColor();
                                    int tileNumber = tempObj.GetTileNumber();
                                    Destroy(blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).gameObject);

                                    if (tileNumber == _tileNumber)
                                    {
                                        MergeBlocks(_tileNumber, currentIndex, false);
                                    }
                                    else
                                    {
                                        randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                        PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                        tileObj.AssignData(_tileNumber, _tileColor);

                                        Transform previousRandomPosition = blockArea.transform.GetChild(previousIndex);
                                        PZTileObject tileObject = Instantiate(_tile, previousRandomPosition);
                                        tileObject.AssignData(tileNumber, tileColor);
                                    }

                                }
                                else
                                {
                                    Debug.Log("Index -5 else");
                                    randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);
                                }
                            }
                        }
                        else if (existedBlockList[i] % PZConstants.secondRowMultiplier == 0) //Index 8
                        {
                            int previousIndex = existedBlockList[i];
                            PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                            Color _tileColor = obj.GetTileColor();
                            int _tileNumber = obj.GetTileNumber();
                            Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                            int currentindex = existedBlockList[i] - (PZConstants.gridMatrix + PZConstants.gridMatrix);
                            if (CheckBlockExistedAtSpecifiedIndex(currentindex))
                            {
                                int nextCurrentIndex = existedBlockList[i] - PZConstants.gridMatrix;
                                if (!CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                                {
                                    Debug.Log("Index - 8 if");
                                    randomBlockPosition = blockArea.transform.GetChild(nextCurrentIndex);
                                    PZTileObject tileObj1 = Instantiate(_tile, randomBlockPosition);
                                    tileObj1.AssignData(_tileNumber, _tileColor);
                                }
                            }
                            else
                            {
                                Debug.LogError("Index - 8 else");
                                randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                PZTileObject tileObj1 = Instantiate(_tile, randomBlockPosition);
                                tileObj1.AssignData(_tileNumber, _tileColor);
                            }
                        }
                    }

                }
            }
        }
        private void MoveBlocksToDown()
        {
            ResetExistedBlockData();
            GetAlreadyExistedBlocks();
            ResetBoolValues();
            bool checkDownBlockStatus = CheckTwoDownIndexs();
            Debug.Log("checkDownBlockStatus" + checkDownBlockStatus);
            if (checkDownBlockStatus)
            {
                return;
            }
            else
            {
                for (int i = 0; i < existedBlockList.Count; i++)
                {
                    if (existedBlockList[i] % PZConstants.gridMatrix == 1)
                    {
                        if (existedBlockList[i] % PZConstants.secondRowMultiplier == 0) //Index 4
                        {
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
                        else if (existedBlockList[i] % PZConstants.secondRowMultiplier == 1) //Index 1
                        {
                            int previousIndex = existedBlockList[i];
                            PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                            Color _tileColor = obj.GetTileColor();
                            int _tileNumber = obj.GetTileNumber();
                            Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                            int currentindex = existedBlockList[i] + (PZConstants.gridMatrix + PZConstants.gridMatrix);
                            if (!CheckBlockExistedAtSpecifiedIndex(currentindex))
                            {
                                int nextCurrentIndex = existedBlockList[i] + PZConstants.gridMatrix;
                                if(CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                                {
                                    PZTileObject tileObj = blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                    Color tileColor = tileObj.GetTileColor();
                                    int tileNumber = tileObj.GetTileNumber();   
                                    Destroy(blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).gameObject);

                                    if (tileNumber == _tileNumber)
                                    {
                                        MergeBlocks(_tileNumber, currentindex, false);
                                    }
                                    else
                                    {
                                        randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                        PZTileObject tempTileObj = Instantiate(_tile, randomBlockPosition);
                                        tempTileObj.AssignData(tileNumber, tileColor);

                                        Transform nextRandomPosition = blockArea.transform.GetChild(nextCurrentIndex);
                                        PZTileObject tempNextTileObj = Instantiate(_tile, nextRandomPosition);
                                        tempNextTileObj.AssignData(_tileNumber, _tileColor);
                                    }
                                }
                                else
                                {
                                    randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);
                                }
                            }
                            else
                            {
                                int nextCurrentIndex = existedBlockList[i] + PZConstants.gridMatrix;
                                if(!CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                                {
                                    randomBlockPosition = blockArea.transform.GetChild(nextCurrentIndex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);
                                }

                            }

                        }
                    }
                    else if (existedBlockList[i] % PZConstants.gridMatrix == 0)
                    {
                        int _indexCount = 0;
                        if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.gridMatrix) //Index 3
                        {
                                _indexCount = _indexCount + 1;
                                int previousIndex = existedBlockList[i];
                                PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                Color _tileColor = obj.GetTileColor();
                                int _tileNumber = obj.GetTileNumber();
                                Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                                int currentIndex = existedBlockList[i] + PZConstants.gridMatrix;
                                if(!CheckBlockExistedAtSpecifiedIndex(currentIndex))
                                {
                                    int nextCurrentIndex = existedBlockList[i] - PZConstants.gridMatrix;
                                    if(!CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                                    {
                                        if (_indexCount == 1)
                                        {
                                            randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                            PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                            tileObj.AssignData(_tileNumber, _tileColor);
                                        }
                                    }
                                }
                        }
                        else if (existedBlockList[i] % PZConstants.secondRowMultiplier == 0) //Index 0
                        {
                            int previousIndex = existedBlockList[i];
                            PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                            Color _tileColor = obj.GetTileColor();
                            int _tileNumber = obj.GetTileNumber();
                            Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                            int currentindex = existedBlockList[i] + (PZConstants.gridMatrix + PZConstants.gridMatrix);
                            if (!CheckBlockExistedAtSpecifiedIndex(currentindex))
                            {
                                int nextCurrentIndex = existedBlockList[i] + PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                                {
                                    PZTileObject tileObj = blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                    Color tileColor = tileObj.GetTileColor();
                                    int tileNumber = tileObj.GetTileNumber();
                                    Destroy(blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).gameObject);
                                    if (tileNumber == _tileNumber)
                                    {
                                        MergeBlocks(_tileNumber, currentindex, false);
                                    }
                                    else
                                    {
                                        randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                        PZTileObject tempTileObj = Instantiate(_tile, randomBlockPosition);
                                        tempTileObj.AssignData(tileNumber, tileColor);

                                        Transform nextRandomPosition = blockArea.transform.GetChild(nextCurrentIndex);
                                        PZTileObject tempNextTileObj = Instantiate(_tile, nextRandomPosition);
                                        tempNextTileObj.AssignData(_tileNumber, _tileColor);
                                    }

                                }
                                else
                                {
                                    randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);
                                }
                            }
                            else
                            {
                                int nextCurrentIndex = existedBlockList[i] + PZConstants.gridMatrix;
                                if (!CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                                {
                                    randomBlockPosition = blockArea.transform.GetChild(nextCurrentIndex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);
                                }

                            }
                        }
                    }
                    else if (existedBlockList[i] % PZConstants.gridMatrix == 2)
                    {
                        int _indexCount = 0;
                        if (existedBlockList[i] % PZConstants.secondRowMultiplier == 1) //Index 5
                        {
                            if (PZInputManager.instance.IsDown)
                            {
                                _indexCount = _indexCount + 1;
                                int previousIndex = existedBlockList[i];
                                PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                Color _tileColor = obj.GetTileColor();
                                int _tileNumber = obj.GetTileNumber();
                                Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                                int currentIndex = existedBlockList[i] + PZConstants.gridMatrix;
                                if (!CheckBlockExistedAtSpecifiedIndex(currentIndex))
                                {
                                    int nextCurrentIndex = existedBlockList[i] - PZConstants.gridMatrix;
                                    if (!CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                                    {
                                        if (_indexCount == 1)
                                        {
                                            randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                            PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                            tileObj.AssignData(_tileNumber, _tileColor);
                                        }
                                    }
                                }
                            }
                        }
                        else if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.initialBlockCount) //Index 2
                        {
                            int previousIndex = existedBlockList[i];
                            PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                            Color _tileColor = obj.GetTileColor();
                            int _tileNumber = obj.GetTileNumber();
                            Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                            int currentindex = existedBlockList[i] + (PZConstants.gridMatrix + PZConstants.gridMatrix);
                            if (!CheckBlockExistedAtSpecifiedIndex(currentindex))
                            {
                                int nextCurrentIndex = existedBlockList[i] + PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                                {
                                    PZTileObject tileObj = blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                    Color tileColor = tileObj.GetTileColor();
                                    int tileNumber = tileObj.GetTileNumber();
                                    Destroy(blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).gameObject);
                                  
                                    if (tileNumber == _tileNumber)
                                    {
                                        MergeBlocks(_tileNumber, currentindex, false);
                                    }
                                    else
                                    {
                                        Transform nextRandomPosition = blockArea.transform.GetChild(nextCurrentIndex);
                                        PZTileObject tempNextTileObj = Instantiate(_tile, nextRandomPosition);
                                        tempNextTileObj.AssignData(_tileNumber, _tileColor);

                                        randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                        PZTileObject tempTileObj = Instantiate(_tile, randomBlockPosition);
                                        tempTileObj.AssignData(tileNumber, tileColor);
                                    }


                                }
                                else
                                {
                                    randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);
                                }
                            }
                            else
                            {
                                int nextCurrentIndex = existedBlockList[i] + PZConstants.gridMatrix;
                                if (!CheckBlockExistedAtSpecifiedIndex(nextCurrentIndex))
                                {
                                    randomBlockPosition = blockArea.transform.GetChild(nextCurrentIndex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);
                                }

                            }
                        }
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
        private bool CheckBlockExistedAtSpecifiedIndex(int _index)
        {
            int _count = blockArea.transform.GetChild(_index).gameObject.transform.childCount;
            return _count > 0 ? true : false;
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
        private void ResetExistedBlockData()
        {
            existedBlockList.Clear();
        }
        private void ResetEmptyBlockListData()
        {
            emptyBlockList.Clear();
        }
        private void InstanteObjects()
        { 

        }
        bool checkLeftIndexs;
        private bool CheckTwoLeftIndexs()
        {
            for(int i = 0;i<existedBlockList.Count;i++)
            {
                if(PZInputManager.instance.IsLeft)
                {
                    if (existedBlockList[i] % PZConstants.gridMatrix == 1)
                    {
                        int _index = existedBlockList[i] - 1;
                        if (CheckBlockExistedAtSpecifiedIndex(_index))
                        {
                            int _nextIndex = _index + 1;
                            if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                            {
                                checkLeftIndexs = true;
                            }
                        }
                    }
                    else if (existedBlockList[i] % PZConstants.gridMatrix == 2)
                    {
                        int _index = existedBlockList[i] - 2;
                        if (CheckBlockExistedAtSpecifiedIndex(_index))
                        {
                            int _nextIndex = _index + 1;
                            if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                            {
                                checkLeftIndexs = true;
                            }
                        }
                    }
                }
            }
            return checkLeftIndexs;
        }
        bool checkRightIndexes;
        bool checkUpIndexes;
        bool checkDownIndexs;
        private bool CheckTwoRightIndexs()
        {
            for (int i = 0; i < existedBlockList.Count; i++)
            {
                if (PZInputManager.instance.IsRight)
                {
                    if (existedBlockList[i] % PZConstants.gridMatrix == 1)
                    {
                        int _index = existedBlockList[i] + 1;
                        if (CheckBlockExistedAtSpecifiedIndex(_index))
                        {
                            int _nextIndex = _index - 1;
                            if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                            {
                                checkRightIndexes = true;
                            }
                        }
                    }
                    else if (existedBlockList[i] % PZConstants.gridMatrix == 2)
                    {
                        int _index = existedBlockList[i];
                        if (CheckBlockExistedAtSpecifiedIndex(_index))
                        {
                            int _nextIndex = _index - 1;
                            if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                            {
                                checkRightIndexes = true;
                            }
                        }
                    }

                }
            }
            return checkRightIndexes;
        }
        private bool CheckTwoDownIndexs()
        {
            for (int i = 0; i < existedBlockList.Count; i++)
            {
                if (PZInputManager.instance.IsDown)
                {
                    if (existedBlockList[i] % PZConstants.gridMatrix == 1)
                    {
                        if (existedBlockList[i] % PZConstants.secondRowMultiplier == 0)  //Index 4
                        {
                            int _index = existedBlockList[i];
                            if (CheckBlockExistedAtSpecifiedIndex(_index))
                            {
                                int _nextIndex = _index + PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                                {
                                    checkUpIndexes = true;
                                }
                            }
                        }
                        else if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.gridMatrix) //Index 7
                        {
                            int _index = existedBlockList[i];
                            if(CheckBlockExistedAtSpecifiedIndex((_index)))
                            {
                                int _nextIndex = _index - PZConstants.gridMatrix;
                                if(CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                                {
                                    checkUpIndexes = true; 
                                }
                            }
                        }
                    }
                    else if (existedBlockList[i] % PZConstants.gridMatrix == 2)
                    {
                        if (existedBlockList[i] % PZConstants.secondRowMultiplier == 1) //Index 5
                        {
                            int _index = existedBlockList[i];
                            if (CheckBlockExistedAtSpecifiedIndex(_index))
                            {
                                int _nextIndex = _index + PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                                {
                                    checkUpIndexes = true;
                                }
                            }
                        }
                        else if (existedBlockList[i] % PZConstants.secondRowMultiplier == 0) //Index 8
                        {
                            int _index = existedBlockList[i];
                            if (CheckBlockExistedAtSpecifiedIndex((_index)))
                            {
                                int _nextIndex = _index - PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                                {
                                    checkUpIndexes = true;
                                }
                            }
                        }

                    }
                    else if (existedBlockList[i] % PZConstants.gridMatrix == 0)
                    {
                        if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.gridMatrix) //Index 3
                        {
                            int _index = existedBlockList[i];
                            if (CheckBlockExistedAtSpecifiedIndex(_index))
                            {
                                int _nextIndex = _index + PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                                {
                                    checkUpIndexes = true;
                                }
                            }
                        }
                        else if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.initialBlockCount) //Index 6
                        {
                            int _index = existedBlockList[i];
                            if (CheckBlockExistedAtSpecifiedIndex((_index)))
                            {
                                int _nextIndex = _index - PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                                {
                                    checkUpIndexes = true;
                                }
                            }
                        }

                    }

                }
            }
            return checkUpIndexes;
        }
        private bool CheckTwoUpIndexs()
        {
            for (int i = 0; i < existedBlockList.Count; i++)
            {
                if (PZInputManager.instance.IsUp)
                {
                    if (existedBlockList[i] % PZConstants.gridMatrix == 1)
                    {
                        if (existedBlockList[i] % PZConstants.secondRowMultiplier == 0) //Index 4
                        {
                            int _index = existedBlockList[i];
                            if (CheckBlockExistedAtSpecifiedIndex(_index))
                            {
                                int _nextIndex = _index - PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                                {
                                    checkDownIndexs = true;
                                }
                            }
                        }
                        else if (existedBlockList[i] % PZConstants.secondRowMultiplier == 1) //Index 1
                        {
                            int _index = existedBlockList[i];
                            if (CheckBlockExistedAtSpecifiedIndex(_index))
                            {
                                int _nextIndex = _index + PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                                {
                                    checkDownIndexs = true;
                                }
                            }
                        }
                    }
                    else if (existedBlockList[i] % PZConstants.gridMatrix == 0)
                    {
                        if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.gridMatrix) //Index 3
                        {
                            int _index = existedBlockList[i];
                            if (CheckBlockExistedAtSpecifiedIndex(_index))
                            {
                                int _nextIndex = _index - PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                                {
                                    checkDownIndexs = true;
                                }
                            }
                        }
                        else if (existedBlockList[i] % PZConstants.secondRowMultiplier == 0) //Index 0
                        {
                            int _index = existedBlockList[i];
                            if (CheckBlockExistedAtSpecifiedIndex(_index))
                            {
                                int _nextIndex = _index + PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                                {
                                    checkDownIndexs = true;
                                }
                            }
                        }
                    }
                    else if (existedBlockList[i] % PZConstants.gridMatrix == 2)
                    {
                        if (existedBlockList[i] % PZConstants.secondRowMultiplier == 1)
                        {
                            int _index = existedBlockList[i];
                            if (CheckBlockExistedAtSpecifiedIndex(_index))
                            {
                                int _nextIndex = _index - PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                                {
                                    checkDownIndexs = true;
                                }
                            }
                        }
                        else if (existedBlockList[i] % PZConstants.secondRowMultiplier == PZConstants.initialBlockCount)
                        {
                            int _index = existedBlockList[i];
                            if (CheckBlockExistedAtSpecifiedIndex(_index))
                            {
                                int _nextIndex = _index + PZConstants.gridMatrix;
                                if (CheckBlockExistedAtSpecifiedIndex(_nextIndex))
                                {
                                    checkDownIndexs = true;
                                }
                            }
                        }

                    }

                }
            }
            return checkDownIndexs;
        }
        private void ResetBoolValues()
        {
            checkUpIndexes = false;
            checkRightIndexes = false;
            checkDownIndexs=false;
            checkLeftIndexs=false;
        }
        int checkExistedIndex = 0;
        private int CheckSameIndexsInExistedList(int _checkValue)
        {
            for(int i = 0; i < existedBlockList.Count; i++)
            {
                if (existedBlockList[i] % PZConstants.gridMatrix == _checkValue)
                {
                    checkExistedIndex = checkExistedIndex + 1;
                }
            }
            return checkExistedIndex;
        }
        private List<int> emptyBlockList = new List<int>();
        private void GetEmptyBlockList()
        {
            for(int i = 0; i<blocksCount; i++)
            {
                int _count = blockArea.transform.GetChild(i).gameObject.transform.childCount;
                if (_count == 0)
                {
                    emptyBlockList.Add(i);
                }
            }
        }
        private int GetRandomIndexFromEmptyBlockList()
        {
            return emptyBlockList[Random.Range(0, emptyBlockList.Count)];
        }
        private void MergeBlocks(int _tileNumber, int _destinationIndex, bool _destinationBlock)
        {
            if(_destinationBlock)
            {
                Destroy(blockArea.transform.GetChild(_destinationIndex).gameObject.transform.GetChild(0).gameObject);
            }
            Transform mergeBlockPosition = blockArea.transform.GetChild(_destinationIndex);
            PZTileObject mergeTileObject = Instantiate(_tile, mergeBlockPosition);
            PZGameManager.Number _assignNumber = PZGameManager.Instance.GetNumberType(_tileNumber * 2);
            mergeTileObject.AssignData(_tileNumber * 2, PZGameManager.Instance.AssignNumberColorForBg(_assignNumber));

            ResetEmptyBlockListData();
            GetEmptyBlockList();
            int _nextIndex = GetRandomIndexFromEmptyBlockList();
            Debug.LogError("Empty Block Index" + _nextIndex);

            Transform newBlockPosition = blockArea.transform.GetChild(_nextIndex);
            PZTileObject newTileObject = Instantiate(_tile, newBlockPosition);
            newTileObject.InitData(true);
            
        }
        private void DisplayEmptyBlockList()
        {
            for(int i = 0; i < emptyBlockList.Count; i++)
            {
                Debug.LogError("EmptyBlockList" + emptyBlockList[i]);
            }
        }
        #endregion
    }
}
