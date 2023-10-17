using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerManager : MonoBehaviour, ISpawner
{
    [SerializeField] private List<GameObject> listChessPieces;
    [SerializeField] private List<Transform> markerLevel1;

    private ChapterData chapterData;

    private List<GameObject> listChessAvailable;

    public void AssignData(ChapterData _chapterData)
    {
        chapterData = _chapterData;
        listChessAvailable = new List<GameObject>();
    }

    public void SpawnerAt(int _level)
    {
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
                        new Vector3(markerLevel1[slot].position.x, 3.1f, markerLevel1[slot].position.z);
                    slot++;
                }
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