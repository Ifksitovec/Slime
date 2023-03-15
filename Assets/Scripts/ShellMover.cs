using UnityEngine;

public class ShellMover : MonoBehaviour
{
    [SerializeField] private float _radius = 10f;
    [SerializeField] private float _speed = 10f;
    private float _maxTime;
    private float _currentTime = 0f;
    private float _speedAngle;
    private bool _isInit = false;
    private int _damage;
    private Vector3 _startPoint, _endPoint, _centerCircle;
    private float _startAngle, _endAngle, _currentAngle;
    private Enemy _enemy;

    void FixedUpdate()
    {
        if (_isInit)
        {
            float deltaTime = Time.fixedDeltaTime;
            _currentTime += deltaTime;
            _currentAngle += _speedAngle * deltaTime;
            transform.position = _centerCircle + new Vector3(_radius * Mathf.Cos(_currentAngle),
                _radius * Mathf.Sin(_currentAngle), 0);
            if (_currentTime > _maxTime)
            {
                _enemy?.Damage(_damage);
                Destroy(gameObject);
            }
        }
    }

    public void Initialize(int damage, Enemy enemy)
    {
        _startPoint = transform.position;
        _endPoint = enemy.AttackPoint.position;
        float distance = Vector3.Distance(_startPoint, _endPoint);
        _startAngle = Mathf.PI / 2f + Mathf.Asin(distance / 2f / _radius);
        _endAngle = Mathf.PI / 2f - Mathf.Asin(distance / 2f / _radius);
        float catet = Mathf.Sqrt(Mathf.Pow(_radius, 2) - Mathf.Pow(distance / 2, 2));
        _centerCircle = _startPoint + new Vector3(distance / 2, -catet, 0);
        _currentAngle = _startAngle;
        _maxTime = distance / _speed;
        _speedAngle = (_endAngle - _startAngle) / _maxTime;
        _enemy = enemy;
        _isInit = true;
        _damage = damage;
    }
}
