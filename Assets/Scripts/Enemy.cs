
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class Enemy : MonoBehaviourPunCallbacks
{
    [SerializeField] protected int health = 100;
    [SerializeField] protected float attackDistance = 3;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float cooldown = 2;
    [SerializeField] Image healthBar;
    protected GameObject player;
    protected Animator anim;
    protected Rigidbody rb;
    protected float distance;
    protected float timer;
    bool dead = false;


    protected GameObject[] players;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        CheckPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (!dead)
        {
            Attack();
        }
        
        //Dichiarare una variabile che memorizzerà la distanza minima
        //Mathf.Infinity - infinito positivo
        float closestDistance = Mathf.Infinity;
        //Scorrendo l'elenco dei giocatori
        foreach (GameObject closestPlayer in players)
        {
            //Calcolo della distanza tra il nemico e il giocatore
            float checkDistance = Vector3.Distance(closestPlayer.transform.position, transform.position);
            //Se la distanza da questo giocatore è inferiore alla distanza dal giocatore precedente, allora....
            if (checkDistance < closestDistance)
            {
                //Se il giocatore precedente è vivo
                if (closestPlayer.GetComponent<PlayerController>().dead == false)
                {
                    //Salvataggio del giocatore corrente come giocatore più vicino 
                    player = closestPlayer;
                    //Modifica del valore di closestDistance in base alla distanza da questo giocatore.
                    closestDistance = checkDistance;
                }
            }
        }
        if (player != null)
        {
        //Il resto della sceneggiatura non è cambiato rispetto alle lezioni precedenti.
        distance = Vector3.Distance(transform.position, player.transform.position);
         if (!dead && !player.GetComponent<PlayerController>().dead == false)
         {
            Attack();
                 
         }
        }
    }

    void FixedUpdate()
    {
        if (!dead && player !=null)
        {
            Move();
        }
    }

    public virtual void Move()
    {

    }

    public virtual void Attack()
    {

    }
    [PunRPC]
    public void ChangeHealth(int count)
    {
        health -= count;
        float fillPercent = health / 100f;
        healthBar.fillAmount = fillPercent;

        if (health <= 0)
        {
            dead = true;
            anim.enabled = true;
            anim.SetBool("Die", true);
            GetComponent<Collider>().enabled = false;
           
        }
    }
    public void GetDamage(int count)
    {
        photonView.RPC("ChangeHealth", RpcTarget.All, count);
    }
    void CheckPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        Invoke("CheckPlayers", 3f);
    }
}
