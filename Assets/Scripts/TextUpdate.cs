using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class TextUpdate : MonoBehaviourPunCallbacks, IPunObservable
{   
    [SerializeField] TMP_Text playerNickName;
    int health = 100;
    // Start is called before the first frame update
    void Start()
    {   
        if( photonView.IsMine)
        {    
            playerNickName.text = photonView.Controller.NickName + "\n" + "Health: " + health.ToString();
      
            photonView.RPC("RotateName", RpcTarget.Others);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetHealth(int newHealth)
    {    
    // aggiornare il valore della salute
     health = newHealth;
    // aggiornamento del testo dell'interfaccia utente
     // ricevere un nickname da Photon, passare alla riga successiva e visualizzare il valore di salute
     playerNickName.text = photonView.Controller.NickName + "\n" + "Health: " + health.ToString();
    }   
    [PunRPC]
    public void RotateName()
    {
      playerNickName.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
     if (stream.IsWriting)
     {
        stream.SendNext(health);
     }
     else
     {        
        health = (int)stream.ReceiveNext();
// Richiamo del metodo Replace con il nuovo valore della variabile isMale
        playerNickName.text = photonView.Controller.NickName + "\n" + "Health: " + health.ToString();
     }
    }
}
