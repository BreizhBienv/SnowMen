using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private PlayerInfo _playerInfo;
    private GameManager _gameManager;
    private SpawnPlayers _spawnPlayer;

    [SerializeField] private int _pointsUponDeath;
    [SerializeField] private int _respawnDelay;

    //needed to disable & enable player
    private HeadControls _headControls;
    private Renderer[] _snowmenRenderer;
    private Rigidbody _snowmenRigibody;
    private SphereCollider _collisionCollider;
    private List<MonoBehaviour> _snowmenScripts = new List<MonoBehaviour>();

    //particules
    [SerializeField] private ParticleSystem _deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
        {
            GameObject GameMaster = GameObject.FindGameObjectWithTag("GameManager");

            _gameManager = GameMaster.GetComponent<GameManager>();
            _spawnPlayer = GameMaster.GetComponent<SpawnPlayers>();
        }

        _headControls = this.GetComponent<HeadControls>();
        _snowmenRenderer = this.GetComponentsInChildren<Renderer>();
        _snowmenRigibody = this.GetComponent<Rigidbody>();
        _collisionCollider = this.GetComponent<SphereCollider>();

        _snowmenScripts.Add(this.GetComponent<CreateSnowBall>());
        _snowmenScripts.Add(this.GetComponent<PlayerShoot>());
        _snowmenScripts.Add(this.GetComponent<PlayerInteractions>());
        _snowmenScripts.Add(this.GetComponent<HeadControls>());
        _snowmenScripts.Add(this.GetComponent<PlayerMove>());
    }

    // Update is called once per frame
    void Update()
    {
        PlayerUponDeath();
    }

    private void PlayerUponDeath()
    {

        if (_playerInfo.CurrHP <= 0 && !_playerInfo.IsDead)
        {
            _playerInfo.IsDead = true;

            _playerInfo.CurrHP = _playerInfo.GetBaseHP;

            var Instant = Instantiate(_deathParticles, this.transform.position, Quaternion.identity);
            Destroy(Instant, 0.5f);

            DropAlliedHead();

            DisablePlayer();

            if (_gameManager.BlueSpawn == null || _gameManager.RedSpawn == null)
                return;

            if (_playerInfo.PlayerTeam == "Blue")
            {
                _gameManager.RedScore += _pointsUponDeath;
            }
            else if (_playerInfo.PlayerTeam == "Red")
            {
                _gameManager.BlueScore += _pointsUponDeath;
            }

            StartCoroutine(RespawnAfterDelay());
        }
    }
    IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(_respawnDelay);

        if (_playerInfo.PlayerTeam == "Blue")
            _spawnPlayer.RespawnPlayer(this.gameObject, _gameManager.BlueSpawn, _playerInfo);
        else if (_playerInfo.PlayerTeam == "Red")
            _spawnPlayer.RespawnPlayer(this.gameObject, _gameManager.RedSpawn, _playerInfo);
    }

    private void DisablePlayer()
    {
        _headControls.PickUpHead();

        foreach (Renderer renderer in _snowmenRenderer)
        {
            renderer.enabled = false;
        }

        foreach (MonoBehaviour scripts in _snowmenScripts)
        {
            scripts.enabled = false;
        }

        _collisionCollider.enabled = false;
    }

    private void DropAlliedHead()
    {
        if (_headControls.GrabbedHead != null)
        {
            if (!_playerInfo.PlayerHead.Equals(_headControls.GrabbedHead.gameObject))
            {
                Transform OtherHead = _headControls.GrabbedHead;

                OtherHead.parent = null;

                OtherHead.GetComponent<HeadBehaviour>().IsHeadSnowball = false;

                OtherHead.GetComponent<SphereCollider>().enabled = true;
            }
        }
    }

    public void Leave(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0 && !_playerInfo.IsDead && GameManager.Instance._currState == GameManager.GameState.SelectTeams)
        {
            if (context.interaction is TapInteraction)
            {
                if (GameManager.BlueTeam.Find(gm => gm == this.gameObject))
                {
                    GameManager.BlueTeam.Remove(this.gameObject);
                }
                else if (GameManager.RedTeam.Find(gm => gm == this.gameObject))
                {
                    GameManager.RedTeam.Remove(this.gameObject);
                }

                Destroy(this.gameObject);
            }
        }
    }

    public Renderer[] SnowmenRenderer { get => _snowmenRenderer; }

    public Rigidbody SnowmenRigidbody { get => _snowmenRigibody; }

    public SphereCollider CollisionCollider { get => _collisionCollider; }

    public List<MonoBehaviour> PlayerScripts { get => _snowmenScripts; }
}
