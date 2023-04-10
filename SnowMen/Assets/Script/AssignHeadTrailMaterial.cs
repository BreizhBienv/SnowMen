using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignHeadTrailMaterial : MonoBehaviour
{
    PlayerInfo _player;
    [SerializeField] Material _white;
    [SerializeField] Material _blue;
    [SerializeField] Material _red;

    [SerializeField] Outline _outline;
    [SerializeField] TrailRenderer _component;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponentInParent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.PlayerTeam == "None")
        {
            _outline.OutlineColor = new Color(1f, 1f, 1f, 1);
            _component.GetComponent<TrailRenderer>().material = _white;
        }
        if (_player.PlayerTeam == "Red")
        {
            _outline.OutlineColor = new Color(1f, 0f, 0f, 1);
            _component.GetComponent<TrailRenderer>().material = _red;
         }
        if (_player.PlayerTeam == "Blue")
        {
            _outline.OutlineColor = new Color(0.4f, 0.69f, 1f,1); 
           _component.GetComponent<TrailRenderer>().material = _blue;
        }
        
    }
}
