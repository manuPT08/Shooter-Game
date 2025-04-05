using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MiniMap : MonoBehaviourPunCallbacks
{
    [SerializeField] float scrollSpeed = 1f;
    [SerializeField] float minValue = 10f;
    [SerializeField] float maxValue = 60f;

    float currentValue;
    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Controllo dei valori di scorrimento del mouse
        // Controllo dei valori di scorrimento del mouse
     float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
    // Regolazione del valore della variabile zoom in base alla direzione dello scorrimento
     if (scrollDelta > 0)
     {
         currentValue += scrollSpeed;
     }
     else if (scrollDelta < 0)
     {
         currentValue -= scrollSpeed;
     }
     // Limitazione del valore di scorrimento tra i valori di zoom minimo e massimo introdotti nello script
    // Il valore attuale dello zoom viene -bloccato- tra i valori limite, il che significa che non è possibile ingrandire o ridurre lo zoom per sempre.
     currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
     gameObject.GetComponent<Camera>().orthographicSize = currentValue;
    }
}
