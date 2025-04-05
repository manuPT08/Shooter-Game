using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] List<Transform> spawns = new List<Transform>();
    [SerializeField] List<Transform> spawnsWalk = new List<Transform>();
    [SerializeField] List<Transform> spawnsTurret = new List<Transform>();
    [SerializeField] TMP_Text playersText;
// Un array che memorizza tutti i giocatori 


    int randomSpawn;
    private int previousPlayerCount;

// Aggiungere questa riga al metodo Start()
// In esso, utilizziamo la funzione Photon() per ottenere il numero di giocatori sul server

    
    GameObject[] players;
// Un elenco che memorizza i nickname dei giocatori attivi.
    List<string> activePlayers = new List<string>();
    int checkPlayers = 0;


    // Start is called before the first frame update
    void Start()
    {
        randomSpawn = Random.Range(0, spawns.Count);
        PhotonNetwork.Instantiate("Player", spawns[randomSpawn].position, spawns[randomSpawn].rotation);
        Invoke("SpawnEnemy", 5f);
        previousPlayerCount = PhotonNetwork.PlayerList.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.PlayerList.Length < previousPlayerCount)
        {
            ChangePlayersList();
        }
        previousPlayerCount = PhotonNetwork.PlayerList.Length;
    }
    public void SpawnEnemy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < spawnsWalk.Count; i++)
            {
                PhotonNetwork.Instantiate("WalkEnemy", spawnsWalk[i].position, spawnsWalk[i].rotation);
            }
            for (int i = 0; i < spawnsTurret.Count; i++)
            {
                PhotonNetwork.Instantiate("Turret", spawnsTurret[i].position, spawnsTurret[i].rotation);
            }
        }
    }
    [PunRPC]
    public void PlayerList()
    {
      players = GameObject.FindGameObjectsWithTag("Player");
      activePlayers.Clear();

      foreach(GameObject player in players)
      {
    // Se il giocatore è vivo, allora
        if(player.GetComponent<PlayerController>().dead == false)
        {
        // Aggiunta dei loro nickname all'elenco activePlayers
            activePlayers.Add(player.GetComponent<PhotonView>().Owner.NickName);
        }
      }
      playersText.text = "Players in game : " + activePlayers.Count.ToString();

      if (activePlayers.Count <= 1 && checkPlayers > 0)
      {
    // Cercare tutti i nemici presenti nel gioco e memorizzarli in un array
        var enemies = GameObject.FindGameObjectsWithTag("enemy");
        PlayerPrefs.SetString("Winner", activePlayers[0]);
        foreach (GameObject enemy in enemies)
        {
        // Infligge 100 HP di danno a ogni nemico nello schieramento. Se i nemici hanno più di 100 HP, dovrete modificare questo valore di conseguenza!
           enemy.GetComponent<Enemy>().ChangeHealth(100);
        }
        Invoke("EndGame", 5f);
      }

      checkPlayers++;


    }
    public void ChangePlayersList()
    {
        photonView.RPC("PlayerList", RpcTarget.All);   
    }
    void EndGame()
    {
    // Ritorno alla lobby
        PhotonNetwork.LoadLevel("Lobby");
    }
    public void ExitGame()
    {
       PhotonNetwork.LeaveRoom();
    }
// Un metodo Photon che viene eseguito ogni volta che un giocatore esce di scena.
    public override void OnLeftRoom()
    {
    // Caricamento della scena con il menu
       SceneManager.LoadScene(0);
    // Aggiornamento dell'elenco dei giocatori
       ChangePlayersList();
    }


}

