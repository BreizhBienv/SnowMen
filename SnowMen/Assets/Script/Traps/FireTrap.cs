using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [Header("Fire Trap Settings")]
    [SerializeField] private float _reloadTime; //how much time before can be activate again

    List<GameObject> _onTrap = new List<GameObject>();

    private bool _isReloading = false;
    private bool _isActivated = false;

    [Header("Damage over time settings")]
    [SerializeField] private float _damPerSec;  //how much damage per seconds
    [SerializeField] private float _activeTime; //how much time trap activate in seconds

    private int _secondsPast;

    /* Fire trap lervers */
    Transform _stick;
    Transform _pivot;
    Outline _outline;

    private ParticleSystem[] _fireParticules;

    private AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        _fireParticules = this.GetComponentsInChildren<ParticleSystem>();
        _audio = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ActivateParticles();
    }

    private void ActivateParticles()
    {
        if (_isActivated)
        {
            foreach (ParticleSystem particleSystem in _fireParticules)
                if (!particleSystem.isPlaying) particleSystem.Play();

            if (!_audio.isPlaying)
                _audio.Play();
        }
        else
        {
            foreach (ParticleSystem particleSystem in _fireParticules)
                if (particleSystem.isPlaying) particleSystem.Stop();

            if (_audio.isPlaying)
                _audio.Stop();
        }
    }

    public IEnumerator DamageOverTimeCoroutine(GameObject p_lever)
    {

        _stick = p_lever.transform.Find("Lever");
        _pivot = p_lever.transform.Find("pivot");
        _outline = _stick.GetComponentInParent<Outline>();

        _outline.enabled = false;

        _stick.RotateAround(_pivot.position, _stick.forward, 90);

        while (_secondsPast <= _activeTime)
        {
            foreach (GameObject player in _onTrap)
            {
                player.GetComponent<PlayerInfo>().CurrHP -= _damPerSec;
            }

            yield return new WaitForSeconds(1);

            ++_secondsPast;
        }

        _secondsPast = 0;
        _isActivated = false;
        _isReloading = true;

        StartCoroutine(TrapReload());
    }

    private IEnumerator TrapReload()
    {
        yield return new WaitForSeconds(_reloadTime);

        _isReloading = false;
        _outline.enabled = true;

        _stick.RotateAround(_pivot.position, _stick.forward, -90);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && _isActivated)
        {
            _onTrap.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            _onTrap.Remove(other.gameObject);
        }
    }

    public bool IsActivated { get => _isActivated; set => _isActivated = value; }

    public bool IsRealoading { get => _isReloading; set => _isReloading = value; }
}
