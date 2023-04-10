using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeamBlueSpawn : MonoBehaviour
{
    [SerializeField] private Color _gizmosColor;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    //display rectangle in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawCube(this.transform.position, this.transform.localScale);
    }
}
