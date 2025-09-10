using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private List<CinemachineImpulseSource> _cciss;
    
    public float Speed { get; set; }
    public float OriginSpeed => 8f;
    public float HalfSpeed => OriginSpeed * 0.3f;
    public Action OnInGameStartAniEnd;
    public AniController Animator { get; set; }

    private bool _isMove;
    private Vector3 _movePosition;
    private Transform _target;
    private Action _onStop;

    private int _addFieldOfView = 4;
    private float _noneLength = 0.1f;
    private float _targetDistance = 0;
    private float _originFieldOfView;
    private CinemachineVirtualCamera _camera;
    
    private readonly int _inGameStartAni = UnityEngine.Animator.StringToHash("inGameStart");
    
    public void Initialize()
    {
        _camera = this.GetComponent<CinemachineVirtualCamera>();
        _originFieldOfView = _camera.m_Lens.FieldOfView;

        this.Speed = OriginSpeed;
        this.Animator = this.gameObject.GetComponent<Animator>().Initialize();
        
        Animator.SetEndFunc("InGameStart", (clipName) => InGameStartAniEnd());

        // 화면 비율에 따른 카메라 시야각 조정
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // float targetAspect = 9.0f / 16.0f; // 세로 화면 비율로 변경
            // float currentAspect = (float)Screen.width / Screen.height;
            // float multiplier = currentAspect / targetAspect;
            
            // if (multiplier < 1)
            // {
            //     // 화면이 더 가로로 긴 경우, 시야각을 넓혀서 세로 시야 확보
            //     _camera.m_Lens.FieldOfView = Mathf.Clamp(_originFieldOfView / multiplier, _originFieldOfView, 90f);
            // }
            // else
            // {
            //     // 기본 시야각 사용
            //     mainCamera.fieldOfView = _originFieldOfView;
            // }

            SetFieldOfView();
        }
    }
    public void SetPosition(float moveX)
    {
        Vector3 pos = this.transform.position;
        pos.x = moveX;
        this.transform.position = pos;
        _isMove = false;
        _target = null;
    }
    public void SetMove(float moveX, Action onStop = null)
    {
        _movePosition = this.transform.position;
        _movePosition.x = moveX;
        this._onStop = onStop;
        _isMove = true;
        _target = null;
    }
    public void SetTarget(Transform target, float targetDistance = 0)
    {
        this._target = target;
        _isMove = false;
        _targetDistance = targetDistance;
    }

    private void OnMoveStop()
    {
        _isMove = false;
        _onStop?.Invoke();
        _onStop = null;
    }

    public void SetFieldOfView(int index = 0, bool isTween = false)
    {
        float targetAspect = 9.0f / 16.0f; // 세로 화면 비율로 변경
        float currentAspect = (float)Screen.width / Screen.height;
        float multiplier = currentAspect / targetAspect;
        int addFieldOfView = 8;

        float fov = (multiplier >= 1) ? _originFieldOfView + index * addFieldOfView : Mathf.Clamp((_originFieldOfView + index * addFieldOfView) / multiplier, _originFieldOfView, 90f);

        if (isTween)
        {
            Managers.Tween.TweenCameraFieldOfView(_camera, _camera.m_Lens.FieldOfView, fov, 0.5f);
        }
        else
        {
            _camera.m_Lens.FieldOfView = fov;
        }
    }
    public void SetFieldOfViewByTargets(Character targetA, Character targetB, bool isTween = false)
    {
        if (targetA == null || targetB == null || _camera == null)
            return;

        Vector3 positionA = targetA.transform.position;
        Vector3 positionB = targetB.transform.position;

        positionA.x -= targetA.BoxWeidth * 0.5f;
        positionB.x += targetB.BoxWeidth * 0.5f;

        SetFieldOfViewByPosition(positionA, positionB, isTween);
    }
    public void SetFieldOfViewByPosition(Vector3 positionA, Vector3 positionB, bool isTween = false)
    {
        if (_camera == null)
            return;

        float distance = UnityHelper.GetDistanceX(positionA, positionB);

        // 카메라 가운데로 이동
        SetMove(Mathf.Lerp(positionA.x, positionB.x, 0.5f));

        float targetAspect = 9.0f / 16.0f; // 세로 화면 비율로 변경
        float currentAspect = (float)Screen.width / Screen.height;
        float multiplier = currentAspect / targetAspect;

        // 기본 FOV 기준값
        float baseDistance = 9.4f;       // 기준 거리
        float baseFOV = (multiplier >= 1) ? _originFieldOfView : Mathf.Clamp(_originFieldOfView / multiplier, _originFieldOfView, 90f); // 기준 거리일 때 FOV
        float scale = 3.6f;             // 민감도 조절 (커질수록 FOV 많이 변함)

        // 비례 계산 (간단한 선형 증가)
        float targetFOV = baseFOV + (distance - baseDistance) * scale;

        if (isTween)
        {
            Managers.Tween.TweenCameraFieldOfView(_camera, _camera.m_Lens.FieldOfView, targetFOV, 0.5f);
        }
        else
        {
            _camera.m_Lens.FieldOfView = targetFOV;
        }
    }

    void Update()
    {
        if (_isMove)
        {
            Vector3 destPos = _movePosition;
            if (this.transform.position.GetDistance(destPos) > _noneLength)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, destPos, Speed * Managers.Time.DeltaTime);
            }
            else
            {
                OnMoveStop();
            }
        }
        else
        {
            if (_target)
            {
                Vector3 destPos = this.transform.position;
                destPos.x = _target.position.x + _targetDistance;
                if (this.transform.position.GetDistance(destPos) > _noneLength)
                {
                    this.transform.position = Vector3.Lerp(this.transform.position, destPos, Speed * Managers.Time.DeltaTime);
                }
            }
        }
    }

    [Button]
    public void InGameStartAni()
    {
        _isMove = false;
        Animator.SetTrigger(_inGameStartAni);
    }

    public void InGameStartAniEnd()
    {
        OnInGameStartAniEnd?.Invoke();
        Animator.anim.enabled = false;
    }

    public void Shake(int index)
    {
        _cciss[index].GenerateImpulse();

        Managers.Observer.UIPuzzle.BoomAni(index);
    }
}
