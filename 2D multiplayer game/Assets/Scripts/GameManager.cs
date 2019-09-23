using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float timeAmount = 5;
    private bool startRespawn;

    [HideInInspector]
    public GameObject localPlayer;

    public GameObject playerPrefab;
    public GameObject canvas;
    public GameObject sceneCam;
    public GameObject respawnUI;
    public Text pingRate;
    public Text spawnTimer;

    public GameObject leaveScreen;

    public static GameManager instance = null;

    void Awake()
    {
        instance = this;
        canvas.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleLeaveScreen();
        }
        if (startRespawn)
        {
            StartRespawn();
        }
        pingRate.text = "NetworkPing : " + PhotonNetwork.GetPing();
    }

    void StartRespawn()
    {
        timeAmount -= Time.deltaTime;
        spawnTimer.text = "Respawn in : " + timeAmount.ToString("F0");

        if (timeAmount <= 0)
        {
            respawnUI.SetActive(false);
            startRespawn = false;
            PlayerRelocation();
            localPlayer.GetComponent<Health>().EnableInputs();
            localPlayer.GetComponent<PhotonView>().RPC("Revive", RpcTarget.AllBuffered);        }
    }

    public void EnableRespawn()
    {
        timeAmount = 5;
        startRespawn = true;
        respawnUI.SetActive(true);
    }

    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-5, 5);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(playerPrefab.transform.position.x * randomValue, playerPrefab.transform.position.y), Quaternion.identity, 0);
        canvas.SetActive(false);
        sceneCam.SetActive(false);
    }

    public void ToggleLeaveScreen()
    {
        if (leaveScreen.activeSelf)
        {
            leaveScreen.SetActive(false);
        }
        else
        {
            leaveScreen.SetActive(true);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }

    public void PlayerRelocation()
    {
        float randomPosition = Random.Range(-5, 5);
        localPlayer.transform.localPosition = new Vector2(randomPosition, 2);
    }
}
