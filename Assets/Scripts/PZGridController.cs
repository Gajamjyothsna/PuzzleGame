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
                Debug.Log("both are indexs are same");
                randomIndexList.RemoveAt(randomIndexList.Count - 1);
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
            bool currentIndexStatus;
            ResetData();
            GetAlreadyExistedBlocks();
            ResetBoolValues();
            bool status = CheckTwoLeftIndexs();
            Debug.LogError("status " + status);
            if (status)
            {
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
                                randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                tileObj.AssignData(_tileNumber, _tileColor);

                                int nextIndex = existedBlockList[i + 1];
                                PZTileObject obj1 = blockArea.transform.GetChild(nextIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                int tileNumber = obj1.GetTileNumber();
                                Color tileColor = obj1.GetTileColor();
                                Destroy(blockArea.transform.GetChild(nextIndex).gameObject.transform.GetChild(0).gameObject);
                                Transform nextRandomPosition = blockArea.transform.GetChild(previousIndex);
                                PZTileObject nextTileObj = Instantiate(_tile, nextRandomPosition);
                                nextTileObj.AssignData(tileNumber, tileColor);
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
                        bool thirdIndexStatus = CheckBlockExistedAtSpecifiedIndex(currentindex);
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
            ResetData();
            GetAlreadyExistedBlocks();
            ResetBoolValues();
            bool status = CheckTwoRightIndexs();
            Debug.LogError("status " + status);
            if (status)
            {
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
                        Debug.Log("count " + blockArea.transform.GetChild(currentIndex).gameObject.transform.childCount);
                        int previousIndex = existedBlockList[i];
                        PZTileObject obj = blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                        Color _tileColor = obj.GetTileColor();
                        int _tileNumber = obj.GetTileNumber();
                        Destroy(blockArea.transform.GetChild(previousIndex).gameObject.transform.GetChild(0).gameObject);
                        Debug.Log("current index" + currentIndex);
                        if (!CheckBlockExistedAtSpecifiedIndex(currentIndex))
                        {
                            int nextToCurrentIndex = existedBlockList[i] - 1;
                            if (CheckBlockExistedAtSpecifiedIndex(nextToCurrentIndex))
                            {
                                Debug.LogError("if case - index 1");
                                int nextIndex = existedBlockList[i] - 1;
                                PZTileObject obj1 = blockArea.transform.GetChild(nextIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                int tileNumber = obj1.GetTileNumber();
                                Color tileColor = obj1.GetTileColor();
                                Destroy(blockArea.transform.GetChild(nextIndex).gameObject.transform.GetChild(0).gameObject);

                                randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                tileObj.AssignData(_tileNumber, _tileColor);

                                Transform nextRandomPosition = blockArea.transform.GetChild(previousIndex);
                                PZTileObject nextTileObj = Instantiate(_tile, nextRandomPosition);
                                nextTileObj.AssignData(tileNumber, tileColor);


                            }
                            else
                            {
                                Debug.LogError("else case - index 1");
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
                                Debug.LogError("if case - 0");
                                int nextIndex = existedBlockList[i] + 1;
                                PZTileObject obj1 = blockArea.transform.GetChild(nextIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                int tileNumber = obj1.GetTileNumber();
                                Color tileColor = obj1.GetTileColor();
                              Destroy(blockArea.transform.GetChild(nextIndex).gameObject.transform.GetChild(0).gameObject);

                                randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                tileObj.AssignData(_tileNumber, _tileColor);

                                Transform nextRandomPosition = blockArea.transform.GetChild(nextIndex);
                                PZTileObject nextTileObj = Instantiate(_tile, nextRandomPosition);
                                nextTileObj.AssignData(tileNumber, tileColor);


                            }
                            else
                            {
                                Debug.LogError("else case - 0");
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
            ResetData();
            GetAlreadyExistedBlocks();
            ResetBoolValues();
            bool checkUpBlockStatus = CheckTwoUpIndexs();
            Debug.LogError("CheckUpBlockStatus" + checkUpBlockStatus);
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
                                    Debug.LogError("index-4 if");
                                    PZTileObject tempObj = blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                    Color tileColor = tempObj.GetTileColor();
                                    int tileNumber = tempObj.GetTileNumber();
                                 Destroy(blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).gameObject);

                                    randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(tileNumber, tileColor);

                                    Transform previousRandomPosition = blockArea.transform.GetChild(previousIndex);
                                    PZTileObject tileObject = Instantiate(_tile, previousRandomPosition);
                                    tileObject.AssignData(_tileNumber, _tileColor);

                                }
                                else
                                {
                                    Debug.LogError("Index - 4 if else");
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
                                    Debug.LogError("Index - 7 if");
                                    randomBlockPosition = blockArea.transform.GetChild(nextCurrentIndex);
                                    PZTileObject tileObj1 = Instantiate(_tile, randomBlockPosition);
                                    tileObj1.AssignData(_tileNumber, _tileColor);
                                }
                            }
                            else
                            {
                                Debug.LogError("Index - 7 else");
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
                                    Debug.LogError("index 3 if");                                                                                    
                                    PZTileObject tempObj = blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                    Color tileColor = tempObj.GetTileColor();
                                    int tileNumber = tempObj.GetTileNumber();
                                    Destroy(blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).gameObject);

                                    randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(tileNumber, tileColor);

                                    Transform previousRandomPosition = blockArea.transform.GetChild(previousIndex);
                                    PZTileObject tileObject = Instantiate(_tile, previousRandomPosition);
                                    tileObject.AssignData(_tileNumber, _tileColor);

                                }
                                else
                                {
                                    Debug.LogError("Index - 3 else");

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
                                    Debug.LogError("Index -6 if");
                                    randomBlockPosition = blockArea.transform.GetChild(nextCurrentIndex);
                                    PZTileObject tileObj1 = Instantiate(_tile, randomBlockPosition);
                                    tileObj1.AssignData(_tileNumber, _tileColor);
                                }
                            }
                            else
                            {
                                Debug.LogError("Index - 6 else");
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
                                    Debug.LogError("Index -5 if");
                                    PZTileObject tempObj = blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                    Color tileColor = tempObj.GetTileColor();
                                    int tileNumber = tempObj.GetTileNumber();
                                    Destroy(blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).gameObject);

                                    randomBlockPosition = blockArea.transform.GetChild(currentIndex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(tileNumber, tileColor);

                                    Transform previousRandomPosition = blockArea.transform.GetChild(previousIndex);
                                    PZTileObject tileObject = Instantiate(_tile, previousRandomPosition);
                                    tileObject.AssignData(_tileNumber, _tileColor);

                                }
                                else
                                {
                                    Debug.LogError("Index -5 else");
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
                                    Debug.LogError("Index - 8 if");
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
            ResetData();
            GetAlreadyExistedBlocks();
            ResetBoolValues();
            bool checkDownBlockStatus = CheckTwoDownIndexs();
            Debug.LogError("checkDownBlockStatus" + checkDownBlockStatus);
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
                                Debug.LogError("Index - 4");
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
                            Debug.LogError("Index - 1");
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

                                    randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                    PZTileObject tempTileObj = Instantiate(_tile, randomBlockPosition);
                                    tempTileObj.AssignData(tileNumber, tileColor);

                                   Transform nextRandomPosition = blockArea.transform.GetChild(nextCurrentIndex);
                                   PZTileObject tempNextTileObj = Instantiate(_tile, nextRandomPosition);
                                   tempNextTileObj.AssignData(_tileNumber, _tileColor);
                                }
                                else
                                {
                                    Debug.LogError("Index - 1 if else");
                                    randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);
                                }
                            }
                            else
                            {
                                Debug.LogError("Index - 1 else");
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
                                Debug.LogError("Index - 3");
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
                                            Debug.Log("index count " + _indexCount);
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
                                    Debug.LogError("Index - 0 if");
                                    PZTileObject tileObj = blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                    Color tileColor = tileObj.GetTileColor();
                                    int tileNumber = tileObj.GetTileNumber();
                                    Destroy(blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).gameObject);

                                    randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                    PZTileObject tempTileObj = Instantiate(_tile, randomBlockPosition);
                                    tempTileObj.AssignData(tileNumber, tileColor);

                                    Transform nextRandomPosition = blockArea.transform.GetChild(nextCurrentIndex);
                                    PZTileObject tempNextTileObj = Instantiate(_tile, nextRandomPosition);
                                    tempNextTileObj.AssignData(_tileNumber, _tileColor);
                                    
                                }
                                else
                                {
                                    Debug.LogError("INdex - 0 if else");
                                    randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);
                                }
                            }
                            else
                            {
                                Debug.LogError("Index - 0 else");
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
                                Debug.LogError("Index - 5");
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
                                            Debug.Log("index count " + _indexCount);
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
                                    Debug.LogError("Index - 2 if");
                                    PZTileObject tileObj = blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).GetComponent<PZTileObject>();
                                    Color tileColor = tileObj.GetTileColor();
                                    int tileNumber = tileObj.GetTileNumber();
                                    Destroy(blockArea.transform.GetChild(nextCurrentIndex).gameObject.transform.GetChild(0).gameObject);
                                    Transform nextRandomPosition = blockArea.transform.GetChild(nextCurrentIndex);
                                    PZTileObject tempNextTileObj = Instantiate(_tile, nextRandomPosition);
                                    tempNextTileObj.AssignData(_tileNumber, _tileColor);

                                    randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                    PZTileObject tempTileObj = Instantiate(_tile, randomBlockPosition);
                                    tempTileObj.AssignData(tileNumber, tileColor);

                                    
                                }
                                else
                                {
                                    Debug.LogError("Index - 2 if else");
                                    randomBlockPosition = blockArea.transform.GetChild(currentindex);
                                    PZTileObject tileObj = Instantiate(_tile, randomBlockPosition);
                                    tileObj.AssignData(_tileNumber, _tileColor);
                                }
                            }
                            else
                            {
                                Debug.LogError("Index - 2 else");
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
        private void ResetData()
        {
            existedBlockList.Clear();
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
        #endregion
    }
}
