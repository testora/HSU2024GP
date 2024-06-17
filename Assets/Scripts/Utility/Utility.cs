using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI;
public class Utility 
{
    internal static float ProportionalRatio(float value, float min, float max)
    {
        if (min == max) return 0f;
        return (value - min) / (max - min);
    }

    internal static Vector3 Diffusion(Vector3 reference, float theta, float phi)
    {
        // θ와 φ 사이의 랜덤한 α값 생성
        float alpha = theta + Random.Range(0f, 1f) * (phi - theta);
        // 0부터 2π 사이의 랜덤한 β값 생성
        float beta = 2f * Random.Range(0f, 1f) * Mathf.PI;

        // 새로운 벡터의 x, y, z 좌표 계산
        float x = Mathf.Sin(alpha) * Mathf.Cos(beta);
        float y = Mathf.Sin(alpha) * Mathf.Sin(beta);
        float z = Mathf.Cos(alpha);

        // 벡터 생성
        Vector3 randomVector = new Vector3(x, y, z);

        // 참조 벡터와 (0,0,1) 벡터 사이의 회전축과 각도 계산
        Vector3 zAxis = new Vector3(0f, 0f, 1f);
        Vector3 axis = Vector3.Cross(zAxis, reference).normalized;
        float angle = Vector3.Angle(zAxis, reference);

        // 회전 행렬을 이용해 벡터 회전
        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
        Vector3 resultVector = rotation * randomVector;

        return resultVector;

    }

    //  XMVECTOR Cone_DiffusionAngle(FXMVECTOR _vReference, _float _fTheta, _float _fPhi)
    //  {
    //      XMFLOAT3 vVector;
    //
    //      _float fAlpha = _fTheta + RandomFloat(0.f, 1.f) * (_fPhi - _fTheta);
    //      _float fBeta = 2.f * RandomFloat(0.f, 1.f) * XM_PI;
    //
    //      vVector.x = sin(fAlpha) * cos(fBeta);
    //      vVector.y = sin(fAlpha) * sin(fBeta);
    //      vVector.z = cos(fAlpha);
    //
    //      XMVECTOR vAxis = XMVector3Cross(XMVectorSet(0.f, 0.f, 1.f, 0.f), _vReference);
    //      _float fAngle = XMVectorGetX(XMVector3AngleBetweenVectors(XMVectorSet(0.f, 0.f, 1.f, 0.f), _vReference));
    //
    //      return XMVector3Transform(XMLoadFloat3(&vVector), XMMatrixRotationAxis(vAxis, fAngle));
    //  }
}
