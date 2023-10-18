using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerManager : MonoBehaviour, ISpawner
{
    [SerializeField] private List<GameObject> listChessPieces;
    [SerializeField] private List<Transform> markerLevel1;
    [SerializeField] private Transform MarkerCenter;
    
    private ChapterData chapterData;

    private List<GameObject> listChessAvailable;
    private List<GameObject> lissChessSpawn;

    private void Awake()
    {
        listChessAvailable = new List<GameObject>();
        lissChessSpawn = new List<GameObject>();
        EventHandle.OnMainMenu += onMainMenu;
    }

    private void onMainMenu()
    {
        destroyChessOldLevel();
        
        if (listChessAvailable.Count > 0)
        {
            listChessAvailable.Clear();
        }
    }

    public void AssignData(ChapterData _chapterData)
    {
        chapterData = _chapterData;
    }

    public void SpawnerAt(int _level)
    {
        destroyChessOldLevel();

        if (listChessAvailable.Count > 0)
        {
            listChessAvailable.Clear();
        }
        
        for (var i = 0; i < chapterData.chapter.Length; i++)
        {
            var chapter = chapterData.chapter[i];
            if (_level == chapter.level)
            {
                getChessPiecesAvailable(chapter);
                createChessPieces(chapter);
            }
        }
    }

    private void destroyChessOldLevel()
    {
        for (var i = lissChessSpawn.Count - 1; i >= 0; i--)
        {
            var chess = lissChessSpawn[i];
            lissChessSpawn.RemoveAt(i);
            Destroy(chess);
        }
    }

    public List<GameObject> GetListChess()
    {
        return lissChessSpawn;
    }

    private void createChessPieces(LevelData _chapter)
    {
        var numOfSpawn = 3;
        var slot = 0;
        
        for (var iChessPiece = 0; iChessPiece < listChessAvailable.Count; iChessPiece++)
        {
            for (int i = 0; i < numOfSpawn; i++)
            {
                var chessObj = Instantiate(listChessAvailable[iChessPiece]);
                IChessItem chessItem = chessObj.GetComponent<IChessItem>();
                chessItem.AssignData(iChessPiece);
                if (_chapter.level == 1)
                {
                    chessObj.transform.position =
                        new Vector3(markerLevel1[slot].position.x, 5.1f, markerLevel1[slot].position.z);
                }
                else
                {
                    var x = Random.Range(MarkerCenter.position.x + 30, -(MarkerCenter.position.x + 30));
                    var z = Random.Range(MarkerCenter.position.z + 30, -(MarkerCenter.position.z + 30));
                    chessObj.transform.position = new Vector3(x,4f,z);
                }

                var rotationChess = chessObj.transform.rotation;
                if (rotationChess.x <= -150)
                {
                    rotationChess.x = 0;
                    chessObj.transform.rotation = rotationChess;
                }
                slot++;
                lissChessSpawn.Add(chessObj);
            }
        }
    }

    private void getChessPiecesAvailable(LevelData chapter)
    {
        for (var j = 0; j < chapter.typesChessPieces.Length; j++)
        {
            var chessPiece = listChessPieces[chapter.typesChessPieces[j] - 1];
            if (chessPiece != null)
            {
                listChessAvailable.Add(chessPiece);
            }
        }
    }

    public void ReSpawn()
    {
    }
}