using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    [SerializeField]
    private float _moveSpeed = 5.0f;
    [SerializeField]
    private float _timeBeforeShooting = 2.0f;
    [SerializeField]
    private float _shootingTime = 5.0f;
    [SerializeField]
    private float _timeBeforeRetreating = 1.0f;
    [SerializeField]
    private Vector3 _targetPos = new Vector3(5, 0, 8);

    private Vector3 _gotoPos;
    private Vector3 _retreatPos;
    private float _step;

    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    public float TimeBeforeShooting
    {
        get { return _timeBeforeShooting; }
        set { _timeBeforeShooting = value; }
    }

    public float ShootingTime
    {
        get { return _shootingTime; }
        set { _shootingTime = value; }
    }

    public float TimeBeforeRetreating
    {
        get { return _timeBeforeRetreating; }
        set { _timeBeforeRetreating = value; }
    }

    public Vector3 GoToPosition
    {
        get { return _gotoPos; }
        set { _gotoPos = value; }
    }

    public Vector3 RetreatPosition
    {
        get { return _retreatPos; }
        set { _retreatPos = value; }
    }

    public float Step
    {
        get { return _step; }
        set { _step = value; }
    }
        

    void Start()
    {
        print(transform.position.ToString());
        _gotoPos = new Vector3(transform.position.x, transform.position.y, transform.position.z -8);
        _retreatPos = new Vector3(_gotoPos.x, _gotoPos.y, _gotoPos.z + 3.0f);
        _step = _moveSpeed * Time.deltaTime;
    }

}
