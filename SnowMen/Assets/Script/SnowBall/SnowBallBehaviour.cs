using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallBehaviour : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private float Speed;
    private int _damage;
    private string _alliedTeam;

    private Rigidbody _rb;

    [SerializeField] private GameObject _particleDeath;

    [SerializeField] private GameObject _hitAudio;
    [SerializeField] private GameObject _shatterAudio;

    // Start is called before the first frame update
    void Start()
    {
        _rb = this.GetComponent<Rigidbody>();

        if (GameObject.FindGameObjectWithTag("GameManager") != null)
            _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }


    void FixedUpdate()
    {
        _rb.MovePosition(this.transform.position + this.transform.forward * Speed * Time.fixedDeltaTime);
    }

    private void OnDestroy()
    {
        GameObject Instant = Instantiate(_particleDeath, this.transform.position, this.transform.rotation);
        Destroy(Instant, 0.5f);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            GameObject gameObject = Instantiate(_shatterAudio, this.transform.position, Quaternion.identity);
            Destroy(gameObject, 1);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.layer == 10)
        {
            //get snowmen to have access to his team
            PlayerInfo ColliderInfo = collision.gameObject.GetComponent<PlayerInfo>();

            if (ColliderInfo.PlayerTeam != _alliedTeam)
            {
                ReactionToShoot reactionToShoot = collision.gameObject.GetComponent<ReactionToShoot>();

                if (!reactionToShoot.IsInvincible)
                {
                    //enemy loose HP
                    ColliderInfo.CurrHP -= _damage;

                    if (_alliedTeam == "Blue")
                    {
                        _gameManager.BlueScore += _damage;
                    }
                    else if (_alliedTeam == "Red")
                    {
                        _gameManager.RedScore += _damage;
                    }

                    GameObject gameObject = Instantiate(_shatterAudio, this.transform.position, Quaternion.identity);
                    GameObject gameObject2 = Instantiate(_hitAudio, this.transform.position, Quaternion.identity);

                    Destroy(gameObject, 1);
                    Destroy(gameObject2, 1);
                }

                reactionToShoot.Invincibility();
            }

            Destroy(this.gameObject);
        }
    }

    public int SnowballDamage { set => _damage = value; }

    public string AlliedTeam { set => _alliedTeam = value; }
}
