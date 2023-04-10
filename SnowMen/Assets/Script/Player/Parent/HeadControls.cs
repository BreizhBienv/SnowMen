using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class HeadControls : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    [SerializeField] private HeadBehaviour _headBehaviour;
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private ArrowBehaviour _arrowBehaviour;
    [SerializeField] private Vector3 TransHeadFall;
    [SerializeField] private Vector3 PlaceOnHead;
    [SerializeField] private int _dragOnFall;
    [SerializeField] private int _dragOnGround;

    private Transform _childHead;
    private int _headID;
    private Rigidbody _rbHead;
    private Transform _grabbedHead; //contain the head that can be grabbed and thrown
    private bool _canPickUpHead = false;
    private bool _teamMateIn = false;

    private AudioSource _popAudio;

    // Start is called before the first frame update
    void Start()
    {
        _childHead = this.transform.Find("Head");
        _rbHead = this.transform.Find("Head").GetComponent<Rigidbody>();

        _headID = _childHead.GetInstanceID();

        _popAudio = this.transform.Find("PopSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //lower drag when head is falling
        if (_rbHead.velocity.y < 0)
            _rbHead.drag = _dragOnFall;
        else
            _rbHead.drag = _dragOnGround;
    }
    public void Head(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0)
        {
           if (context.interaction is TapInteraction)
            {
                if (context.performed && _headBehaviour.IsHeadAttached)
                {
                    DetachHead();
                    if (!_popAudio.isPlaying)
                        _popAudio.Play();
                }
            }

            if (context.interaction is HoldInteraction)
            {
                if (context.performed)
                {
                    //if distance body/head is lower than range -> pick up head
                    if (_canPickUpHead && !_headBehaviour.IsHeadSnowball)
                    {
                        PickUpHead();
                    }
                }
            }
        }

    }

    private void DetachHead()
    {

        //unfreeze rigidbody
        _rbHead.constraints = RigidbodyConstraints.None;

        //translate to make fall
        _childHead.transform.Translate(TransHeadFall);

        //detach from parent (SnowMen)
        _childHead.transform.parent = null;

        _headBehaviour.IsHeadAttached = false;

        _playerInfo.PossessOwnHead = false;
    }

    public void PickUpHead()
    {
        //freeze rigidbody
        _rbHead.constraints = RigidbodyConstraints.FreezeAll;

        //set the parent of head to SnowMen
        _childHead.transform.parent = this.transform;

        //place head on top of body
        _childHead.transform.position = this.transform.position + PlaceOnHead;

        //set head in correct rotation
        _childHead.transform.rotation = this.transform.rotation;

        _arrowBehaviour.ResetArrowPos();

        _playerInfo.PossessOwnHead = true;

        _playerInfo.HasAHeadAsSnowball = false;

        _headBehaviour.IsHeadAttached = true;

        _headBehaviour.IsHeadSnowball = false;

        _headBehaviour.IsHeadThrown = false;

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            //get head's team
            HeadBehaviour headBehaviour = other.GetComponent<HeadBehaviour>();

            if (other.gameObject.transform.GetInstanceID() == _headID)
            {
                _canPickUpHead = true;
            }

            if (headBehaviour.HeadTeam == _playerInfo.PlayerTeam)
            {
                _teamMateIn = true;

                if (_grabbedHead == null)
                {
                    _grabbedHead = other.transform;
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            //get head's team
            HeadBehaviour headBehaviour = other.GetComponent<HeadBehaviour>();

            if (other.gameObject.transform.GetInstanceID() == _headID)
            {
                _canPickUpHead = false;
            }

            if (headBehaviour.HeadTeam == _playerInfo.PlayerTeam)
            {
                _teamMateIn = false;
            }

            if (!_headBehaviour.IsHeadSnowball)
                _grabbedHead = null;
        }
    }

    public bool IsTeamMateIn { get => _teamMateIn; }

    public Transform GrabbedHead { get => _grabbedHead; set => _grabbedHead = value; }
}
