using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class PlayerShoot : MonoBehaviour
{

    [SerializeField] private GameObject SnowPrefabWhite;
    [SerializeField] private GameObject SnowPrefabBlue;
    [SerializeField] private GameObject SnowPrefabRed;

    [SerializeField] private GameObject SnowmanBody;
    [SerializeField] private GameObject Arrow;
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private HeadControls _headControls;
    [SerializeField] private float AngleRotateArms;

    private Transform _armL;
    private Transform _armR;
    private Transform _pivotL;
    private Transform _pivotR;

    private int _headID;

    private AudioSource _throwSound;

    private void Start()
    {
        Transform Body = this.transform.Find("Body");

        _armL = Body.transform.Find("ArmL");
        _armR = Body.transform.Find("ArmR");
        _pivotL = _armL.transform.Find("PivotL");
        _pivotR = _armR.transform.Find("PivotR");

        Transform Head = this.transform.Find("Head");

        _headID = Head.transform.GetInstanceID();

        _throwSound = this.transform.Find("ThrowSound").GetComponent<AudioSource>();
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0 && !_playerInfo.IsDead)
        {
            if (context.interaction is TapInteraction)
            {
                //throw head
                if (context.performed && _playerInfo.HasAHeadAsSnowball && _headControls.GrabbedHead != null)
                {
                    if (!_throwSound.isPlaying)
                        _throwSound.Play();

                    ThrowHead();
                }
                //throw snowball
                else if (context.performed && _playerInfo.ListSnowball.Count > 0)
                {
                    if (!_throwSound.isPlaying)
                        _throwSound.Play();

                    ThrowSnowball();
                }

            }
        }
    }

    private void ThrowSnowball()
    {
        int current = _playerInfo.ListSnowball[0];
        GameObject prefab = SnowPrefabWhite;
        //create snowBall in front of snowMan
        if (_playerInfo.PlayerTeam == "None")
        {
            prefab = SnowPrefabWhite;
        }
        if (_playerInfo.PlayerTeam == "Blue")
        {
            prefab = SnowPrefabBlue;
        }
        if (_playerInfo.PlayerTeam == "Red")
        {
            prefab = SnowPrefabRed;
        }

        GameObject snowball = Instantiate(prefab, this.transform.position + Arrow.transform.forward, Arrow.transform.rotation);

        snowball.transform.position += snowball.transform.forward;
        SnowBallBehaviour prefabScript = snowball.GetComponent<SnowBallBehaviour>();

        if (current == 2)
        {

            snowball.GetComponent<MeshRenderer>().material = SnowmanBody.GetComponent<MeshRenderer>().material;
        }

        prefabScript.AlliedTeam = _playerInfo.PlayerTeam;
        prefabScript.SnowballDamage = current;
        _playerInfo.ListSnowball.RemoveAt(0);
    }

    private void ThrowHead()
    {
        HeadBehaviour headBehaviour = _headControls.GrabbedHead.GetComponent<HeadBehaviour>();
        Rigidbody rigidbody = _headControls.GrabbedHead.GetComponent<Rigidbody>();

        if (_headID == _headControls.GrabbedHead.GetInstanceID())
        {
            _playerInfo.PossessOwnHead = false;
        }

        _playerInfo.HasAHeadAsSnowball = false;
        headBehaviour.IsHeadSnowball = false;
        headBehaviour.IsHeadThrown = true;

        //head no longer child
        _headControls.GrabbedHead.parent = null;

        //reset head rotation
        _headControls.GrabbedHead.rotation = Arrow.transform.rotation;

        //translate in front of snowman
        _headControls.GrabbedHead.position = _headControls.GrabbedHead.forward + this.transform.position + Arrow.transform.forward;

        //set arms back to position
        _armL.RotateAround(_pivotL.position, this.transform.forward, AngleRotateArms);
        _armR.RotateAround(_pivotR.position, this.transform.forward, -AngleRotateArms);

        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotation;
    }
}
