using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float shiftSpeed = 10f;
    [SerializeField] GameObject pistol, rifle, miniGun;
    [SerializeField] Image pistolUI, rifleUI, miniGunUI, cursor;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioSource characterSound;
    [SerializeField] GameObject damageUi;
    bool isGrounded = true;
    bool isPistol , isRifle, isMiniGun;

    float stamina = 5f;
    float currentSpeed;
    int health;

    Rigidbody rb;
    Vector3 direction;
    Animator animator;

    public enum Weapons
    {
        None,
        Pistol,
        Rifle,
        MiniGun,
        
    }
    public bool dead;

    TextUpdate textUpdate;
    // Start is called before the first frame update
    GameManager gameManager;
    void Start()
    {   
        if(!photonView.IsMine)
        {
            transform.Find("Main Camera").gameObject.SetActive(false);
            transform.Find("Canvas").gameObject.SetActive(false);

    // Disabilitare lo script PlayerController
            this.enabled = false;
        }
        rb = GetComponent<Rigidbody>();
        currentSpeed = movementSpeed;
        animator = GetComponent<Animator>();
        health = 100;
        textUpdate = GetComponent<TextUpdate>();

        gameManager = FindObjectOfType<GameManager>();
        gameManager.ChangePlayersList();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
        direction = transform.TransformDirection(direction);
        if(direction.x != 0 || direction.z != 0)
        {
            animator.SetBool("Run", true);
            if(!characterSound.isPlaying && isGrounded)
            {
                characterSound.Play();
            }
        }
        if(direction.x == 0 && direction.z == 0)
        {
            animator.SetBool("Run", false);
            characterSound.Stop();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentSpeed = shiftSpeed;
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = movementSpeed;
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
         if(stamina > 0)
         {
            stamina -= Time.deltaTime;
            currentSpeed = shiftSpeed;
          }
          else
          {
            currentSpeed = movementSpeed;
          }
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {            
          stamina += Time.deltaTime;                      
          currentSpeed = movementSpeed;
        }
        if(stamina > 5f)
        {
           stamina = 5f;
        }
        else if (stamina < 0)
        {
           stamina = 0;
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) && isPistol)
        {
            photonView.RPC("ChooseWeapon", RpcTarget.All, Weapons.Pistol);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && isRifle)
        {
            photonView.RPC("ChooseWeapon", RpcTarget.All, Weapons.Rifle);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3) && isMiniGun)
        {
            photonView.RPC("ChooseWeapon", RpcTarget.All, Weapons.MiniGun);

        }
        if(Input.GetKeyDown(KeyCode.Alpha0))
            photonView.RPC("ChooseWeapon", RpcTarget.All, Weapons.None);

    }

    void FixedUpdate()
    {
      rb.MovePosition(transform.position + direction * currentSpeed * Time.deltaTime);
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
            animator.SetBool("Jump", true);
            characterSound.Stop();
            AudioSource.PlayClipAtPoint( jump, transform.position );
            
        }
    }
        
    
    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        animator.SetBool("Jump", false);
    }
    [PunRPC]

    public void ChooseWeapon(Weapons weapons)
    {
        animator.SetBool("Pistol", weapons == Weapons.Pistol);
        animator.SetBool("Assault", weapons == Weapons.Rifle);
        animator.SetBool("MiniGun", weapons == Weapons.MiniGun);
        animator.SetBool("NoWeapon", weapons == Weapons.None);
        pistol.SetActive(weapons == Weapons.Pistol);
        rifle.SetActive(weapons == Weapons.Rifle);
        miniGun.SetActive(weapons == Weapons.MiniGun);
        if (weapons != Weapons.None)
        {
            cursor.enabled = true;
        }
        else
        {
            cursor.enabled = false;
        }


    }
    
    void OnTriggerEnter(Collider other)
    {
      switch (other.gameObject.tag)
      {
        case "pistol":
            if (!isPistol)
            {
                isPistol = true;
                ChooseWeapon(Weapons.Pistol);
                pistolUI.color = Color.white;
            }
            break;
        case "rifle":
            if (!isRifle)
            {
                isRifle = true;
                ChooseWeapon(Weapons.Rifle);
                rifleUI.color = Color.white;
            }
            break;
        case "minigun":
            if (!isMiniGun)
            {
                isMiniGun = true;
                ChooseWeapon(Weapons.MiniGun);
                miniGunUI.color = Color.white;
            }
            break;
        default:
            break;
      }
      Destroy(other.gameObject);
   
    }
  public void GetDamage(int count)
  {
    photonView.RPC("ChangeHealth", RpcTarget.All, count);
  }

 [PunRPC]

 public void ChangeHealth(int count)
 {
    health -= count;
    textUpdate.SetHealth(health);
    damageUi.SetActive(true);
    Invoke("RemoveDamageUI" , 0.1f);

    if (health <= 0)
    {
        dead = true;
        animator.SetBool("Die", true);
        transform.Find("Main Camera").GetComponent<ThirdPersonCamera>().isSpectator = true;
        gameManager.ChangePlayersList();
        ChooseWeapon(Weapons.None);
        this.enabled = false;
        
    } 
 }
 void RemoveDamageUI()
 {
    damageUi.SetActive(false);
 }
}

   

