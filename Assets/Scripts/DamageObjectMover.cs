using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DamageObjectMover : MonoBehaviour
{
    [SerializeField] float _maxTimeLive = 3f;
    [SerializeField] float _maxDistance = 3f;
    [SerializeField] TextMeshPro _text;
    private float _speed;
    private float _speedChangeColor;
    private bool _isInit = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isInit)
        {
            float deltaTime = Time.fixedDeltaTime;
            Color oldColor = _text.color;
            Color color = new Color(oldColor.r, oldColor.g, oldColor.b, 
                Mathf.Clamp(oldColor.a - deltaTime * _speedChangeColor, 0f, 1f));
            _text.color = color;
            transform.position = transform.position + new Vector3(0, deltaTime * _speed, 0);
        }
    }

    public void Initialize(int damage)
    {
        _text.text = damage.ToString();
        _speed = _maxDistance / _maxTimeLive;
        _speedChangeColor = 1f / _maxTimeLive;
        _isInit = true;
        StartCoroutine(InvokeDestroy());
    }

    private IEnumerator InvokeDestroy()
    {
        yield return new WaitForSeconds(_maxTimeLive);
        Destroy(gameObject);
    }
}
