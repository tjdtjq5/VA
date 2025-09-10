using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class DirectionMap : MonoBehaviour
{
    [SerializeField] UIScrollView _scrollView;
    [SerializeField] RectTransform _contents;
    [SerializeField] Transform _pointParent;
    [SerializeField] Transform _lineParent;
    [SerializeField] Transform _centerPoint;
    
    private Dictionary<DungeonNode, DirectionMapPoint> _points = new Dictionary<DungeonNode, DirectionMapPoint>();
    private List<DirectionMapLine> _lines = new List<DirectionMapLine>();
    private DungeonNode _currentNode;


    private readonly string _pointPrefabPath = "Prefab/UI/Etc/InGame/DirectionMapPoint";
    private readonly string _linePrefabPath = "Prefab/UI/Etc/InGame/DirectionMapLine";
    private readonly float _upSizeY = 100f;
    private readonly float _pointSizeY = 300f;
    private readonly float _pointSizeX = 145f;
    private readonly float _pointR = 30f;

    [Button]
    public void Test(RectTransform tr)
    {
        _scrollView.ScrollToChild(tr);
    }

    public void UISet(DungeonTree dungeonTree, DungeonNode currentNode)
    {
        _currentNode = currentNode;

        List<DungeonNode> nodes = dungeonTree.GetNodes().ToList();
        nodes = nodes.OrderBy(node => node.Stage).ThenBy(node => node.Index).ToList();

        SetContentsSize(nodes);
        SetPoint(nodes);
        SetCenterPoint(currentNode);
    }

    private void SetContentsSize(List<DungeonNode> nodes)
    {
        int stageCount = nodes.Last().Stage + 1;
        float contentsSizeY = stageCount * _pointSizeY;

        _contents.sizeDelta = new Vector2(_contents.sizeDelta.x, contentsSizeY);
    }

    private void SetPoint(List<DungeonNode> nodes)
    {
        foreach (var point in _points)
        {
            Managers.Resources.Destroy(point.Value.gameObject);
        }
        for (int i = 0; i < _lines.Count; i++)
        {
            Managers.Resources.Destroy(_lines[i].gameObject);
        }

        _points.Clear();
        _lines.Clear();

        for (int i = 0; i < nodes.Count; i++)
        {
            DirectionMapPoint point = Managers.Resources.Instantiate(_pointPrefabPath, _pointParent).GetComponent<DirectionMapPoint>();

            int stage = nodes[i].Stage;
            int index = nodes[i].Index;
            int stageIndexCount = nodes.Count(node => node.Stage == stage);

            float ry = 0;
            if (stageIndexCount > 1)
            {
                ry = (stage + index) % 2 == 0 ? _pointR : -_pointR;
            }

            point.transform.localPosition = new Vector3((index - (stageIndexCount - 1) / 2f) * _pointSizeX, stage * -_pointSizeY + _contents.sizeDelta.y / 2f - _upSizeY + ry, 0);
            point.transform.localScale = Vector3.one;

            List<DungeonNode> prevNodes = nodes[i].PreviousNodes;
            for (int j = 0; j < prevNodes.Count; j++)
            {
                Transform start = point.transform;
                Transform end = _points.ContainsKey(prevNodes[j]) ? _points[prevNodes[j]].transform : null;

                if(end == null)
                    continue;

                DirectionMapLine line = Managers.Resources.Instantiate(_linePrefabPath, _lineParent).GetComponent<DirectionMapLine>();

                line.UISet(start, end);
                _lines.Add(line);
            }

            bool isCurrent = nodes[i].Stage == _currentNode.Stage && nodes[i].Index == _currentNode.Index;

            point.UISet(nodes[i], isCurrent);
            _points.Add(nodes[i], point);
        }
    }
    
    private void SetCenterPoint(DungeonNode currentNode)
    {
        _centerPoint.position = _points.ContainsKey(currentNode) ? _points[currentNode].transform.position : Vector3.zero;

        _scrollView.ScrollToChild(_centerPoint.GetComponent<RectTransform>());
    }
}