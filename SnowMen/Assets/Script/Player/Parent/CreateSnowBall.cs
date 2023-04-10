 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class CreateSnowBall : MonoBehaviour
{
    /* var for checking if near snow to create snowball */
    [SerializeField] private PlayerInfo _playerInfo;

    [Header("Snowball creation settings")]
    [SerializeField] private float AngleCheckUpdateTimer; //frequency of spin check in second
    [SerializeField] private float MinValidAngle; //minimum angle needed between old and new input to valid the spin
    [SerializeField] private float CreationCooldown; //time to wait after creation of a snowball to create a new one in seconds


    private bool _isCheckingSpinning = false;
    private bool _isInCoolDown = false;


    private readonly float _fullRotation = 360;

    private float _angleCounter = 0;

    private Vector2 _joystickInput; // Register the input value of the joystick
    private Vector2 _oldJoystickInput;

    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        CheckJoystickIsSpinning(); //check if joystick spinning

        //check if joystick have done a full rotation
        if (Mathf.Abs(_angleCounter) >= _fullRotation)
        {
            //increase snow ball counter, reset process & throw a cooldown
            _isInCoolDown = true;

            ResetProcess();
            _playerInfo.AddSnowball();

        }
    }

    private void ResetProcess()
    {
        _angleCounter = 0;
    }

    private void CheckJoystickIsSpinning()
    {
        //entering only if input is changing & spinning is not currently being checked, to not allow two coroutine at the same time
        if (_joystickInput != _oldJoystickInput && !_isCheckingSpinning)
        {
            _isCheckingSpinning = true;

            StartCoroutine(JoystickSpinningDetection());
        }
    }

    private IEnumerator JoystickSpinningDetection()
    {
        //start cooldown if snowball has been recently created
        if (_isInCoolDown)
        {
            yield return new WaitForSeconds(CreationCooldown);
            _isInCoolDown = false;
        }

        //copie most recent input into old to check later
        _oldJoystickInput = _joystickInput;

        yield return new WaitForSeconds(AngleCheckUpdateTimer);

        //check if angle between old and new input is enough to increment the check counter by one
        if (Vector2.Angle(_oldJoystickInput, _joystickInput) >= MinValidAngle)
        {
            //if joystick is spinning add angle between old and recent input to angle counter
            _angleCounter += Vector2.Angle(_oldJoystickInput, _joystickInput);
        }
        //angle is not wide enough
        else
        {
            //reset angle counter if joystick stop spinning
            _angleCounter = 0;
        }

        //coroutine is done
        _isCheckingSpinning = false;
    }
    public void RJoystickRotation(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0)
            _joystickInput = context.ReadValue<Vector2>();
    }
}
