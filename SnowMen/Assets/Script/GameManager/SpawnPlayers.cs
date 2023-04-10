using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    Vector3 _origin;
    Vector3 _range;
    Vector3 _randomRange = Vector3.zero;
    Vector3 _randomCoordinate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FirstSpawn(List<GameObject> p_listPlayers, GameObject p_spawnZone, string p_teamColor, Material p_snowmanColor)
    {
        foreach (GameObject player in p_listPlayers)
        {
            PlayerInfo playerInfo = player.GetComponent<PlayerInfo>();

            SpawnPlayer(player, p_spawnZone);

            SetPlayerFirstSpawn(player, p_teamColor, p_snowmanColor, playerInfo);
        }
    }

    public void SpawnPlayer(GameObject p_player, GameObject p_spawnZone)
    {
        SpawnZoneSet(p_spawnZone);

        //calcul random pos
        _randomRange = new Vector3(Random.Range(-_range.x, _range.x),
                                    1,
                                    Random.Range(-_range.z, _range.z));


        _randomCoordinate = _origin + _randomRange;

        p_player.transform.position = _randomCoordinate;
    }

    private void SpawnZoneSet(GameObject p_spawnZone)
    {
        _origin = p_spawnZone.transform.position;
        _range = p_spawnZone.transform.localScale / 2.0f;
    }

    private void SetPlayerFirstSpawn(GameObject p_player, string p_teamColor, Material p_snowmanColor, PlayerInfo p_playerInfo)
    {
        Transform Head = p_player.transform.Find("Head");
        MeshRenderer Body = p_player.transform.Find("Body").GetComponent<MeshRenderer>();

        p_player.GetComponent<CheckWhichTeam>().enabled = false;

        p_player.transform.Find("WhichTeamTrigger").GetComponent<SphereCollider>().enabled = false;
        p_player.transform.Find("TriggerRange").GetComponent<SphereCollider>().enabled = true;

        //set snowmen color & team
        Head.GetComponent<MeshRenderer>().material = p_snowmanColor;
        Body.material = p_snowmanColor;
        p_playerInfo.PlayerTeam = p_teamColor;
        p_playerInfo.CurrHP = p_playerInfo.GetBaseHP;
        Head.GetComponent<HeadBehaviour>().HeadTeam = p_playerInfo.PlayerTeam;

        p_playerInfo.ListSnowball.Clear();

        if (GameObject.FindGameObjectWithTag("FireTrap") != null)
            p_player.GetComponent<PlayerInteractions>().FireTrap = GameObject.FindGameObjectWithTag("FireTrap").GetComponent<FireTrap>();

    }

    public void RespawnPlayer(GameObject p_player, GameObject p_spawnZone, PlayerInfo p_playerInfo)
    {
        if (p_spawnZone != null)
        {
            SpawnPlayer(p_player, p_spawnZone);

            PlayerEnable(p_player, p_playerInfo);
        }
    }

    private void PlayerEnable(GameObject p_player, PlayerInfo p_playerInfo)
    {
        PlayerDeath playerDeath = p_player.GetComponent<PlayerDeath>();

        foreach (Renderer renderer in playerDeath.SnowmenRenderer)
        {
            renderer.enabled = true;
        }

        foreach (MonoBehaviour scripts in playerDeath.PlayerScripts)
        {
            scripts.enabled = true;
        }

        playerDeath.CollisionCollider.enabled = true;

        p_playerInfo.CurrHP = p_playerInfo.GetBaseHP;
        p_playerInfo.IsDead = false;
    }
}
