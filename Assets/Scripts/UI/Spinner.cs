using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {

    [SerializeField]
    private GameObject _innerSpinner;
    [SerializeField]
    private GameObject _outerSpinner;

    private Animator _anim;
    private bool _shouldSpin;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _innerSpinner.SetActive(false);
        _outerSpinner.SetActive(false);
    }

    private void StartSpinner()
    {
        _innerSpinner.SetActive(true);
        _outerSpinner.SetActive(true);
        _anim.SetBool("StartSpin", true);
    }

    private void StopSpinner()
    {
        _anim.SetBool("StopSpin", false);
        _innerSpinner.SetActive(false);
        _outerSpinner.SetActive(false);
    }

}
