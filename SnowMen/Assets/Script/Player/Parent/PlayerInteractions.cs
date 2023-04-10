using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Grabb Head essentials")]
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private HeadControls _headControls;
    [SerializeField] private ArrowBehaviour _arrowBehaviour;
    [SerializeField] private Vector3 PosGrabbedHead;
    [SerializeField] private float AngleRotateArms;

    private Transform _armL;
    private Transform _armR;
    private Transform _pivotL;
    private Transform _pivotR;

    private int _headID;

    /* Fire trap essentials */
    private FireTrap _fireTrap;
    private bool _inRange = false;
    private GameObject _lever;


    // Start is called before the first frame update
    void Start()
    {
        Transform Body = this.transform.Find("Body");

        _armL = Body.transform.Find("ArmL");
        _armR = Body.transform.Find("ArmR");
        _pivotL = _armL.transform.Find("PivotL");
        _pivotR = _armR.transform.Find("PivotR");

        Transform Head = this.transform.Find("Head");

        _headID = Head.transform.GetInstanceID();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0)
        {
            if (context.interaction is TapInteraction)
            {
                if (context.performed)
                {
                    if (_headControls.IsTeamMateIn && !_playerInfo.HasAHeadAsSnowball)
                    {
                        //pick up head as snowball
                        GrabbHead();
                    }

                    if (_inRange && !_fireTrap.IsActivated && !_fireTrap.IsRealoading)
                    {
                        _fireTrap.IsActivated = true;
                        StartCoroutine(_fireTrap.DamageOverTimeCoroutine(_lever));
                    }
                }
            }
        }
    }

    private void GrabbHead()
    {
        if (_headControls.GrabbedHead == null)
            return;

        //get script of grabbed head
        HeadBehaviour headBehaviour = _headControls.GrabbedHead.GetComponent<HeadBehaviour>();

        if (!headBehaviour.IsHeadAttached && !headBehaviour.IsHeadSnowball)
        {
            if (_headControls.GrabbedHead.GetInstanceID() == _headID)
            {
                _playerInfo.PossessOwnHead = true;

                _arrowBehaviour.ResetArrowPos();
            }

            _playerInfo.HasAHeadAsSnowball = true;
            headBehaviour.IsHeadSnowball = true;
            headBehaviour.IsHeadThrown = false;

            //rotate left and right arms to hold grabbed head
            _armL.RotateAround(_pivotL.position, this.transform.forward, -AngleRotateArms);
            _armR.RotateAround(_pivotR.position, this.transform.forward, AngleRotateArms);

            //freeze head
            _headControls.GrabbedHead.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            //position the grabbed head on player
            _headControls.GrabbedHead.position = this.transform.position + PosGrabbedHead;

            _headControls.GrabbedHead.parent = this.transform;

        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            _inRange = true;
            _lever = other.gameObject.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            _inRange = false;
            _lever = null;
        }
    }

    public FireTrap FireTrap { get => _fireTrap; set => _fireTrap = value; }
}