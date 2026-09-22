using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundObject : MonoBehaviour
{
    [SerializeField]
    public bool _lockY = false;

    [SerializeField]
    private float _cameraSensitivity = 3.0f;

    private float _currentVelocity = 0f;

    public bool CameraEnabled = false;

    public bool CameraLocked = false;

    public bool _cursorLocked = false;

    public Camera cam;

    private float _rotationY;
    private float _rotationX;


    [SerializeField] private Transform _cameratarget;
    [SerializeField] private Transform _cameratarget1;
    [SerializeField] private Transform _cameratarget2;

    public PlrMovement plrscript;

    [SerializeField] private float _distanceFromTarget = 5.0f;
    [SerializeField] private float _distanceFromTargetMax = 5.0f;
    [SerializeField] private float _distanceFromTargetMin = 1.0f;

    [SerializeField] private float _cameraSize1 = 5.0f;
    [SerializeField] private float _cameraSize2 = 50.0f;
    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity;

    [SerializeField]
    private float _smoothTime = 0.5f;

    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(40, 40);


    [SerializeField] private LayerMask _testlayer;

    public float mouseX;
    public float mouseY;

    public bool movecam;



    Vector3 testDir;



    void LateUpdate()
    {
        CameraTest();

    }

    void CameraClippingThing()
    {

        Vector3 rayDistance = transform.TransformPoint(testDir * _distanceFromTargetMax);
        RaycastHit hitInfo;

        if (Physics.Linecast(transform.position, rayDistance, out hitInfo))
        {
            _distanceFromTarget = Mathf.Clamp(hitInfo.distance, _distanceFromTargetMin, _distanceFromTargetMax);
        }
        else
        {
            _distanceFromTarget = _distanceFromTargetMax;
        }



    }

    void CameraTest()
    {

        if (CameraEnabled == true)
        {


            mouseX = Input.GetAxis("Mouse X") * _cameraSensitivity;
            mouseY = Input.GetAxis("Mouse Y") * -_cameraSensitivity;
            if (mouseX >= 0.09 || mouseX <= -0.09)
            {
                _rotationY += mouseX;
            }
            else if (mouseX <= 0.09 || mouseX >= -0.09)
            {
                mouseX = 0;
            }

            if (_lockY == true)
            {
                _rotationX += 0;
            }
            if (_lockY == false)
            {
                if (mouseY >= 0.09 || mouseY <= -0.09)
                {
                    _rotationX += mouseY;
                }
                else if (mouseY <= 0.09 || mouseY >= -0.09)
                {
                    mouseY = 0;
                }
            }



            _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

            Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

            if (CameraLocked == false)
            {
                _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
                transform.localEulerAngles = _currentRotation;

            }

            if (plrscript.slingshotState == false && plrscript.FiredState == false)
            {
                transform.position = _cameratarget.position - transform.forward * _distanceFromTarget;
                cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, _cameraSize1, ref _currentVelocity, _smoothTime);
                if (movecam == true)
                {
                    movecam = false;
                }    
            }
            else if (plrscript.slingshotState == false && plrscript.FiredState == true)
            {

                float distance = Vector2.Distance(transform.position, _cameratarget1.position);
                if (distance > 0.01f && movecam == false)

                {
                    transform.position = Vector2.Lerp(transform.position, _cameratarget1.position - transform.forward * _distanceFromTarget, 3 * Time.deltaTime);
                    if (plrscript.grounded == false)
                    {
                        
                    }
                    else if (plrscript.grounded == true)
                    {
                        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, _cameraSize1, ref _currentVelocity, _smoothTime);
                     
                    }
                }
                if (distance < 0.1f && movecam == false)

                {
                    plrscript.FiredState = false;
                    movecam = true;
                }

            }
            else if (plrscript.slingshotState == true || plrscript.FiredState == true)
            {

                float distance = Vector2.Distance(transform.position, _cameratarget2.position);
                if (distance > 0.01f && movecam == false)

                {
                    transform.position = Vector2.Lerp(transform.position, _cameratarget2.position - transform.forward * _distanceFromTarget, 2 * Time.deltaTime);
                }
                if (distance < 0.01f && movecam == false)

                {
                    movecam = true;
                }



                cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, _cameraSize2, ref _currentVelocity, _smoothTime);
            }
        }

    }





    void Start()
    {
        _smoothVelocity = Vector3.zero;
        CameraEnabled = true;
        testDir = transform.localPosition.normalized;
        cam = GetComponent<Camera>();
    }
    void Update()
    {

        if (plrscript.slingshotState == true && _cameratarget != _cameratarget2)
        {
            _cameratarget = _cameratarget2;
        }
        else if (plrscript.slingshotState == false && _cameratarget != _cameratarget1)
        {
            _cameratarget = _cameratarget1;
        }

        if (_cursorLocked == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

}