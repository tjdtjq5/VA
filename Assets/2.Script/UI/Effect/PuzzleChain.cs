using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PuzzleChain : MonoBehaviour
{
    public void Chain(Transform t1, Transform t2)
    {
        // UI 이미지로 두 Transform을 연결하는 라인을 그립니다
        this.transform.localScale = Vector3.one;

        // 시작점을 첫번째 Transform 위치로 설정
        this.transform.position = t1.position;
        
        // 두번째 Transform을 향해 회전 (UI 이미지가 세로로 되어있어서 90도 회전)
        this.transform.LookAt_H(t2, 90);
        
        // 두 점 사이의 거리 계산 (UI 스케일에 맞게 14배 증폭)
        // Canvas가 Camera 모드일 때 거리 보정을 위해 Camera.main.orthographicSize 값으로 나눔
        float distance = t1.position.GetDistance(t2.position) * 14f / Camera.main.orthographicSize;

        // RectTransform의 높이를 거리만큼 늘려서 라인 연결
        RectTransform rectTransform = this.transform.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, distance);
    }
}
