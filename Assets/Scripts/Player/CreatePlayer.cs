using UnityEngine;
using System.Collections;

public class CreatePlayer : MonoBehaviour {

    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private Vector3 _startPos;

    public void Create()
    {
        Instantiate(_player, _startPos, Quaternion.identity);
    }
}
