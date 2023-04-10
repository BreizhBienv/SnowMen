using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float MoveSpeed;
    [SerializeField] private Camera Camera;
    [SerializeField] private ParticleSystem _snowPowder;

    private Vector2 _joystickInput; // Register the input value of the joystick
    private float _smoothVelocity;

    private AudioSource _walkingAudio;

    // Start is called before the first frame update
    void Start()
    {
        _walkingAudio = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (_joystickInput != Vector2.zero && !_snowPowder.isPlaying)
            _snowPowder.Play();
        else if (_joystickInput == Vector2.zero && _snowPowder.isPlaying)
            _snowPowder.Stop();

        if (_joystickInput != Vector2.zero && !_walkingAudio.isPlaying)
            _walkingAudio.Play();
        else if (_joystickInput == Vector2.zero && _walkingAudio.isPlaying)
            _walkingAudio.Stop();
    }

    private void Movement()
    {
        Vector2 InputNormalized = _joystickInput.normalized;

        //translate snowman
        this.transform.Translate(this.transform.forward * (MoveSpeed * InputNormalized.magnitude) * Time.deltaTime, Space.World);

        //rotate snow man
        //if we don't move or change angle no need to rotate
        if (InputNormalized != Vector2.zero)
        {
            float rotation = Mathf.Atan2(_joystickInput.x, _joystickInput.y) * Mathf.Rad2Deg + Camera.transform.eulerAngles.y;
            this.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(this.transform.eulerAngles.y, rotation, ref _smoothVelocity, 0.2f);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0)
            _joystickInput = context.ReadValue<Vector2>();
    }
}
