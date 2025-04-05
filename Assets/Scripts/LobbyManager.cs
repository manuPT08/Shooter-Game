using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks


{
    [SerializeField] TMP_Text ChatText;
    [SerializeField] TMP_InputField InputText;
    [SerializeField] TMP_Text PlayersText;
    [SerializeField] GameObject startButton;

    // Start is called before the first frame update
    void Start()
    {
        RefreshPlayers();

        if(!PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(false);
        }
        if (PlayerPrefs.HasKey("Winner") && PhotonNetwork.IsMasterClient)
        {
            string winner = PlayerPrefs.GetString("Winner");
            photonView.RPC("ShowMessage", RpcTarget.All, "Last winner: " + winner);
            PlayerPrefs.DeleteAll();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Log(string message)
    {
        ChatText.text += "\n";
        ChatText.text += message;
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
    
        Log(otherPlayer.NickName + " left the room");
        RefreshPlayers();
        if(PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
   
        Log(newPlayer.NickName + " entered the room");
        RefreshPlayers();
    }
    [PunRPC]
    public void ShowMessage(string message)
    {   
        ChatText.text += "\n";
        ChatText.text += message;
        
    }
    public void send()
    {
       if (string.IsNullOrWhiteSpace(InputText.text))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            photonView.RPC("ShowMessage", RpcTarget.All, PhotonNetwork.NickName + ": " + InputText.text);
            InputText.text = string.Empty;
        }
    }
    [PunRPC]
    public void ShowPlayers()
    {
    // Annullamento dell'elenco dei giocatori, lasciando solo la riga 'Giocatori: '.
     PlayersText.text = "Players: ";
    // Avvio di un ciclo che attraversa tutti i giocatori sul server
     foreach (Photon.Realtime.Player otherPlayer in PhotonNetwork.PlayerList)
     {
        // Passaggio alla nuova linea
        PlayersText.text += "\n";
        // Emissione del soprannome del giocatore
        PlayersText.text += otherPlayer.NickName;
     }
    }
    void RefreshPlayers()
    {
    // La chiamata può essere effettuata solo dal Master Client (il giocatore che ha creato il server).
     if (PhotonNetwork.IsMasterClient)
     {
        
        photonView.RPC("ShowPlayers", RpcTarget.All);
     }
    }
    public void StartGame()
    {
      PhotonNetwork.LoadLevel("Game");
    }

    
}
