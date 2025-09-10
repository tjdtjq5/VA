using System.Collections.Generic;
using UnityEngine;

public class UIPuzzleBase : UIFrame
{
    protected override void Initialize()
    {
		_puzzleMatrices = new GameObject[_puzzleMatricesSize, _puzzleMatricesSize];

		Spawn();
		SetPosition();

        base.Initialize();
    }
    
    [SerializeField] private Transform puzzleRoot;
    
    private readonly string _puzzlePrefab = "Prefab/UI/Etc/InGame/PuzzleBase";
    private readonly int _puzzleMatricesSize = 15;
    private readonly float _puzzleCellSize = 130f;
    
    private GameObject[,] _puzzleMatrices;

    public void Open()
    {
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleMatrices[j, i] != null)
			    {
				    _puzzleMatrices[j, i].gameObject.SetActive(true);
			    }
		    }
	    }
    }
    public void Close()
    {
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleMatrices[j, i] != null)
			    {
				    _puzzleMatrices[j, i].gameObject.SetActive(false);
			    }
		    }
	    }
    }
    public void Close(Vector3 position)
    {
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleMatrices[j, i] != null && _puzzleMatrices[j, i].transform.position == position)
			    {
				    _puzzleMatrices[j, i].gameObject.SetActive(false);
				    return;
			    }
		    }
	    }
    }
    public void Close(Vector2Int point)
    {
	    _puzzleMatrices[point.x, point.y].gameObject.SetActive(false);
    }
    public void Close(List<Vector2Int> points)
    {
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleMatrices[j, i] != null && points.Contains(new Vector2Int(i, j)))
			    {
				    _puzzleMatrices[j, i].gameObject.SetActive(false);
			    }
		    }
	    }
    }
    
    private void Spawn()
    {
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleMatrices[j, i] == null)
			    {
				    _puzzleMatrices[j, i] = Managers.Resources.Instantiate<GameObject>(_puzzlePrefab, puzzleRoot);
			    }
		    }
	    }
    }
    
    private void SetPosition()
    {
	    float startX = _puzzleCellSize * 0.5f + -_puzzleCellSize * _puzzleMatricesSize * 0.5f;
	    float startY = -_puzzleCellSize * 0.5f + _puzzleCellSize * _puzzleMatricesSize * 0.5f;
	    Vector3 startPos = new Vector3(startX, startY, 0);
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleMatrices[j, i] != null)
			    {
				    Vector3 pos = startPos;
				    pos.x += _puzzleCellSize * j;
				    pos.y -= _puzzleCellSize * i;
				    _puzzleMatrices[j, i].transform.localPosition = pos;
			    }
		    }
	    }
    }
}