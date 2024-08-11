using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSearcherTest : MonoBehaviour
{
    [SerializeField]
    private Entity requestEntity;
    [SerializeField]
    private float scale = 1f;
    [SerializeField]
    private TargetSearcher targetSearcher;

    [Button]
    public void ScaleUp()
    {
        scale += 0.1f;
        targetSearcher.Scale = scale;
    }
    [Button]
    public void ScaleDown()
    {
        scale -= 0.1f;
        targetSearcher.Scale = scale;
    }
    [Button]
    public void Search()
    {
        targetSearcher.ShowIndicator(requestEntity.gameObject);
        targetSearcher.SelectTarget(requestEntity, requestEntity.gameObject, (searcher, result) => 
        {
            targetSearcher.HideIndicator();

            switch (result.resultMessage)
            {
                case SearchResultMessage.Fail:
                    Debug.Log("<color=red>Select Failed.</color>");
                    break;
                case SearchResultMessage.OutOfRange:
                    Debug.Log("<color=yellow>Out Of Range</color>");
                    break;
                default:
                    if (result.selectedTarget)
                        Debug.Log($"<color=green>Selected Target: {result.selectedTarget.name}</color>");
                    else
                        Debug.Log($"<color=green>Selected Position: {result.selectedPosition}</color>");

                    var searchResult = targetSearcher.SearchTargets(requestEntity, requestEntity.gameObject);
                    if (searchResult.targets.Count > 0)
                    {
                        foreach (var target in searchResult.targets)
                            Debug.Log($"<color=#FF00FF>Searched Target: {target.name}</color>");
                    }
                    else
                    {
                        foreach (var targetPosition in searchResult.positions)
                            Debug.Log($"<color=#FF00FF>Searched Position: {targetPosition}</color>");
                    }
                    break;
            }
        });
    }
}
