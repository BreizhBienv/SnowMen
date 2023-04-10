using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMaterial : MonoBehaviour
{
    PlayerInfo _player;
    [SerializeField] Material _arrowWhite;
    [SerializeField] Material _arrowBlue;
    [SerializeField] Material _arrowRed;

    [SerializeField] GameObject[] _components;

    // Start is called before the first frame update
    void Start()
    {
        
        _player = GetComponentInParent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject _component in _components)
        {
            if (_player.PlayerTeam == "None")
            {
                _component.GetComponent<MeshRenderer>().material = _arrowWhite;
            }
            if (_player.PlayerTeam == "Red")
            {
                _component.GetComponent<MeshRenderer>().material = _arrowRed;
            }
            if (_player.PlayerTeam == "Blue")
            {
                _component.GetComponent<MeshRenderer>().material = _arrowBlue;
            }
        }
    }
}
