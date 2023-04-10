using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CheckWhichTeam : MonoBehaviour
{
    [SerializeField] private Material _blue;
    [SerializeField] private Material _red;
    [SerializeField] private Material _white;

    private Renderer _bodyRender;
    private Renderer _headRender;

    // Start is called before the first frame update
    void Start()
    {
        Transform Body = this.transform.Find("Body");
        Transform Head = this.transform.Find("Head");

        _bodyRender = Body.GetComponent<Renderer>();
        _headRender = Head.GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (SceneManager.GetActiveScene().name == "ChooseTeamScene")
        {
            if (other.gameObject.CompareTag("BlueZone"))
            {
                if (GameManager.BlueTeam.Count < GameManager.BlueTeam.Capacity)
                {
                    if (!GameManager.BlueTeam.Find(gm => gm == this.gameObject))
                        GameManager.BlueTeam.Add(this.gameObject);
                    SetColor(_blue);
                }
            }
            else if (other.gameObject.CompareTag("RedZone"))
            {
                if (GameManager.RedTeam.Count < GameManager.RedTeam.Capacity)
                {
                    if (!GameManager.RedTeam.Find(gm => gm == this.gameObject))
                        GameManager.RedTeam.Add(this.gameObject);
                    SetColor(_red);
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (SceneManager.GetActiveScene().name == "ChooseTeamScene")
        {
            if (other.gameObject.CompareTag("BlueZone"))
            {
                if (GameManager.BlueTeam.Count > 0)
                {
                    GameManager.BlueTeam.Remove(this.gameObject);
                }
            }
            else if (other.gameObject.CompareTag("RedZone"))
            {
                if (GameManager.RedTeam.Count > 0)
                {
                    GameManager.RedTeam.Remove(this.gameObject);
                }
            }

            SetColor(_white);
        }
    }

    private void SetColor(Material p_material )
    {
        _bodyRender.material = p_material;
        _headRender.material = p_material;
    }
}
