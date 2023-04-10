using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReactionToShoot : MonoBehaviour
{
    [SerializeField] private float _period;
    [SerializeField] private float _invincibleDuration;
    [SerializeField] private float _cooldown;
    private bool _isInCooldown = false;
    private bool _isInvincible = false;
    private float _timeElapsed;
    Color transparant;
    [SerializeField] PlayerInfo _playerInfo;
    private float _preservedLife;
    Renderer[] _childRenderer;
    // Start is called before the first frame update
    void Start()
    {
        transparant = Color.clear;
        transparant.a = 127.5f;
        _childRenderer = this.GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isInvincible && !_isInCooldown)
        {
            _timeElapsed += Time.deltaTime;
        }

        PreserveLife();
    }

    IEnumerator BlinkRoutine()
    {
        _preservedLife = _playerInfo.CurrHP;

        if (_childRenderer.Length == 0)
            yield break;

        Color[] BaseColors = new Color[_childRenderer.Length];

        for (int i = 0; i < _childRenderer.Length; ++i)
        {
            BaseColors[i] = _childRenderer[i].material.color;
        }

        while (_timeElapsed < this._invincibleDuration)
        {
            for (int i = 0; i < _childRenderer.Length; i++)
            {
                _childRenderer[i].material.color = transparant;
            }

            yield return new WaitForSeconds(_period);

            for (int i = 0; i < _childRenderer.Length; i++)
            {
                _childRenderer[i].material.color = BaseColors[i];
            }

            yield return new WaitForSeconds(_period);
        }

        _isInvincible = false;
        _isInCooldown = true;

        StartCoroutine(CoolDown());

    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(_cooldown);

        _timeElapsed = 0;
        _isInCooldown = false;
    }
    public void Invincibility()
    {
        if(!_isInvincible && !_isInCooldown && !_playerInfo.IsDead)
        {
            _isInvincible = true;
            StartCoroutine(BlinkRoutine());
        }
    }

    private void PreserveLife()
    {
        if (_isInvincible && !_isInCooldown && !_playerInfo.IsDead)
        {
            _playerInfo.CurrHP = _preservedLife;
        }
    }

    public bool IsInvincible { get => _isInvincible; }

}
