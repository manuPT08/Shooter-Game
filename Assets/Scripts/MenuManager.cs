using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class MenuManager : MonoBehaviourPunCallbacks
{   
    [SerializeField] TMP_Text logText;
    [SerializeField] TMP_InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        // assegnare a un giocatore un soprannome con un numero casuale
        PhotonNetwork.NickName = "Player" + Random.Range(1, 9999);
        // visualizzazione del nickname nel campo Log
        Log("Player Name: " + PhotonNetwork.NickName);

        // configurazione del gioco
        PhotonNetwork.AutomaticallySyncScene = true; // passaggio automatico da una finestra all'altra
        PhotonNetwork.GameVersion = "1"; // impostazione della versione del gioco
        PhotonNetwork.ConnectUsingSettings(); // connessione al server Photon
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Log(string message)
    {
        logText.text += "\n";
        logText.text += message;
    }
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 15 });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        Log("Connesso al server");
    }

    public override void OnJoinedRoom()
    {
       Log("Entrato nella lobby");
       PhotonNetwork.LoadLevel("Lobby");
    }
    public void ChangeName()
    {
    //Lettura del testo digitato dal giocatore nell'InputField
       PhotonNetwork.NickName = inputField.text;
    //Emissione del nuovo nickname
       Log("New Player name: " + PhotonNetwork.NickName);
    }

}   


