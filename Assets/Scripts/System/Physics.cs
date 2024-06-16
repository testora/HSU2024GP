using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics : MonoBehaviour
{
    public bool _enableGravity = false;
    public float _maxSpeed = 100f;
    public Vector3 _resist = Vector3.zero;

    Vector3 _velocity = Vector3.zero;
    Transform _target;

    static float gravity = 9.8f;
    static float tolerance = 1e-5f;

    void Start()
    {
        _target = GetComponent<Transform>();
    }

    void Update()
    {
        ExecuteGravity();
        Resist();
        Terminate();
        _target.Translate(_velocity * Time.deltaTime);
    }

    void LateUpdate()
    {
        if (_velocity.magnitude > tolerance)
            _velocity = Vector3.zero;
    }

    public void Force(Vector3 force)
    {
        _velocity += force;
    }

    public void Flattern(bool x, bool y, bool z)
    {
        if (x) _velocity.x = 0f;
        if (y) _velocity.y = 0f;
        if (z) _velocity.z = 0f;
    }

    void ExecuteGravity()
    {
        if (_enableGravity)
        {
            Force(new Vector3(0f, -gravity * 6f, 0f));
        }
    }

    void Resist()
    {
        _velocity.x *= Mathf.Pow(_resist.x, Time.deltaTime);
        _velocity.y *= Mathf.Pow(_resist.y, Time.deltaTime);
        _velocity.z *= Mathf.Pow(_resist.z, Time.deltaTime);
    }

    void Terminate()
    {
        if (_velocity.magnitude > _maxSpeed)
            _velocity = _velocity.normalized * _maxSpeed;
    }
}
