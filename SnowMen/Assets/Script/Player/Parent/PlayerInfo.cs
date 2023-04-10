using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class PlayerInfo : MonoBehaviour
{
    #region ID
    private int _playerID;
    [SerializeField] private string _team;
    [SerializeField] private GameObject _head;
    #endregion

    private SphereCollider _headCollider;

    [Header("Health Settings")]
    [SerializeField] private float BaseHP;
    private float _currHP;
    private bool _isDead = false;


    /* ------- SnowBall manager ------- */
    //array that contains 1 and 2, 1 for normal snowball and 2 for snowballcreated from body
    private List<int> _snowballStock;
    //contains next snowball type to throw, -1 if there is none

    /* -------------------------------------------------- */


    //to know whether or not player has a head as a snow ball
    private bool _hasAHeadAsSnowball = false;

    //as own head either attached or grabbed
    private bool _possessOwnHead = true;

    [Header("Create Snowball Settings")]
    [SerializeField] private int MaxSnowBall;
    [SerializeField] private GameObject _snowpileParticles;
    private bool _isTriggerPressed = false;
     private readonly float _minHPToCreateSnowBallFromBody = 2;
    private bool _IsOnSnowPile = false;
    private GameObject _snowPile;

    private AudioSource _createSound;

    [Header("Gamepad Rumble")]
    [SerializeField] private float Lfr;
    [SerializeField] private float Hfr;
    [SerializeField] private float _duration;
    private Gamepad _playerGamepad;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        _snowballStock = new List<int>(MaxSnowBall);

        _currHP = BaseHP;
        _headCollider = _head.GetComponent<SphereCollider>();

        _createSound = this.transform.Find("CreateSnowballSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        HeadColliderDisable();
    }

    //disable head collider while attached to body
    private void HeadColliderDisable()
    {
        if (_possessOwnHead)
        {
            _headCollider.enabled = false;
        }
        else
        {
            _headCollider.enabled = true;
        }
    }

    public void AddSnowball()
    {
        /* Add a snowball */

        //in list if total of snowball is less than maximum
        if (ListSnowball.Count < MaxSnowBall)
        {
            if (_isTriggerPressed && _currHP >= _minHPToCreateSnowBallFromBody)
            {
                _currHP--;
                ListSnowball.Add(2);
            }
            else if (_IsOnSnowPile && _snowPile != null)
            {
                ListSnowball.Add(1);

                Vector3 vector3 = new Vector3(_snowPile.transform.position.x, _snowPile.transform.position.y + 0.5f,
                     _snowPile.transform.position.z);

                GameObject gameObject = Instantiate(_snowpileParticles, vector3, Quaternion.identity);
                Destroy(gameObject, 2);
                _createSound.Play();
            }

            if (ListSnowball.Count == ListSnowball.Capacity)
            {
                StartCoroutine(Rumble());
            }
        }
    }

    IEnumerator Rumble()
    {
        _playerGamepad.SetMotorSpeeds(Lfr, Hfr);

        yield return new WaitForSeconds(_duration);

        _playerGamepad.ResetHaptics();
    }

    public void LTrigger(InputAction.CallbackContext context)
    {
        if (context.interaction is PressInteraction)
        {
            if (context.started)
                _isTriggerPressed = true;
            else if (context.performed)
                _isTriggerPressed = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //check if player is on snow pile
        if (other.gameObject.layer == 6)
        {
            _IsOnSnowPile = true;
            _snowPile = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _IsOnSnowPile = false;
            _snowPile = null;
        }
    }

    public float GetBaseHP { get => BaseHP; set => BaseHP = value; }

    public float CurrHP { get => _currHP; set => _currHP = value; }

    public List<int> ListSnowball { get => _snowballStock; set => _snowballStock = value; }

    public bool HasAHeadAsSnowball { get => _hasAHeadAsSnowball; set => _hasAHeadAsSnowball = value; }

    public bool PossessOwnHead { get => _possessOwnHead; set => _possessOwnHead = value; }

    public bool IsDead { get => _isDead; set => _isDead = value; }

    public string PlayerTeam { get => _team; set => _team = value; }

    public int PLayerID { get => _playerID; set => _playerID = value; }

    public Gamepad PlayerGamePad { get => _playerGamepad; set => _playerGamepad = value; }
    public GameObject PlayerHead { get => _head; }
}
