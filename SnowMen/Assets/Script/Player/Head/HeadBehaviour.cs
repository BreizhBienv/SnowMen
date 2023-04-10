using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBehaviour : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private float _speed;
    [SerializeField] private int _headDamage;
    [SerializeField] private Outline Outline;
    [SerializeField] private TrailRenderer Trail;
    [SerializeField] private Animator _anim;

    Rigidbody _rb;
    [SerializeField] private string _headteam;
    private bool _isHeadAttached = true; //head is on original body
    private bool _isHeadSnowball = false; //head grabbed/prepared as a snowball
    private bool _isHeadThrown = false; //head has been thrown as a snowball


    private AudioSource _hitAudio;

    // Start is called before the first frame update
    void Start()
    {
        _rb = this.transform.GetComponent<Rigidbody>();
        if (GameManager.Instance != null)
            _gameManager = GameManager.Instance;

        _hitAudio = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (_isHeadAttached)
        {
            Outline.enabled = false;
            Trail.enabled = false;
            _anim.enabled = false;
        }
        if (_isHeadSnowball)
        {
            Outline.enabled = true;
            Trail.enabled = false;

            _anim.enabled = false;
        }
        if (_isHeadThrown)
        {
            _rb.MovePosition(this.transform.position + this.transform.forward * _speed * Time.fixedDeltaTime);
            Outline.enabled = true;
            Trail.enabled = true;
            _anim.enabled = true;

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isHeadThrown)
        {
            if (collision.gameObject.layer == 9)
            {
                HeadCollide();
            }

            if(collision.gameObject.tag == "Ground")
            {
                Trail.enabled = false;
                _anim.enabled = true;

            }

            if (collision.gameObject.layer == 10 && _isHeadThrown)
            {
                //get snowmen to have access to his team
                PlayerInfo ColliderInfo = collision.gameObject.GetComponent<PlayerInfo>();

                if (ColliderInfo.PlayerTeam != _headteam)
                {
                    ReactionToShoot reactionToShoot = collision.gameObject.GetComponent<ReactionToShoot>();

                    if (!reactionToShoot.IsInvincible)
                    {
                        //enemy loose HP
                        ColliderInfo.CurrHP -= _headDamage;

                        if (_headteam == "Blue")
                        {
                            _gameManager.BlueScore += _headDamage;
                        }
                        else if (_headteam == "Red")
                        {
                            _gameManager.RedScore += _headDamage;
                        }

                        if (!_hitAudio.isPlaying)
                            _hitAudio.Play();
                    }

                    reactionToShoot.Invincibility();
                }

                HeadCollide();
            }
        }
    }

    private void HeadCollide()
    {
        _isHeadThrown = false;
        _rb.constraints = RigidbodyConstraints.None;

    }

    public bool IsHeadAttached { get => _isHeadAttached; set => _isHeadAttached = value; }

    public bool IsHeadSnowball { get => _isHeadSnowball; set => _isHeadSnowball = value; }

    public bool IsHeadThrown { get => _isHeadThrown; set => _isHeadThrown = value; }

    public string HeadTeam { get => _headteam; set => _headteam = value; }
}
