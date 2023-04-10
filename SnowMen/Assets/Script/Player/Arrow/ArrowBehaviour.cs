using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerInfo _playerInfo;

    private Transform _snowman;

    // Start is called before the first frame update
    void Start()
    {
        _snowman = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerInfo.PossessOwnHead)
        {
            this.transform.RotateAround(_snowman.position, this.transform.up, 360 * Time.deltaTime);
        }
    }

    public void ResetArrowPos()
    {
        this.transform.rotation = _snowman.transform.rotation;

        this.transform.position = _snowman.transform.position + _snowman.transform.forward;
    }
}
