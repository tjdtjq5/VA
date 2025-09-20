#if UNITY_EDITOR
using UnityEngine;
using System.Net;
using System.Net.Security;

[DefaultExecutionOrder(-100)]
public class DevCertTrustAll : MonoBehaviour
{
    private RemoteCertificateValidationCallback _callback;

    void Awake()
    {
        // 한 번만 등록
        _callback = (sender, cert, chain, errors) => true;
        ServicePointManager.ServerCertificateValidationCallback += _callback;
    }

    void OnDestroy()
    {
        // 안전하게 해제
        if (_callback != null)
            ServicePointManager.ServerCertificateValidationCallback -= _callback;
    }
}
#endif
