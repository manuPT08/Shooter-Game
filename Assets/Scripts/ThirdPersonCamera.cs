using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour 
{        
    [SerializeField] GameObject player;
    [SerializeField][Range(0.5f, 2f)]
    float mouseSense = 1; 
    [SerializeField][Range(-20, -10)]
    int lookUp = -15;
    [SerializeField][Range(15, 25)]
    int lookDown = 20;
    public bool isSpectator;
// Script di volo della telecamera libera
    [SerializeField] float speed = 50f;
    private void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }
    void Update()
    {     
        float rotateX = Input.GetAxis("Mouse X") * mouseSense;
        float rotateY = Input.GetAxis("Mouse Y") * mouseSense;
        
        if(!isSpectator)
        {
         Vector3 rotCamera = transform.rotation.eulerAngles;
         Vector3 rotPlayer = player.transform.rotation.eulerAngles;

         rotCamera.x = (rotCamera.x > 180) ? rotCamera.x - 360 : rotCamera.x;
         rotCamera.x = Mathf.Clamp(rotCamera.x, lookUp, lookDown);
         rotCamera.x -= rotateY;               
        
         rotCamera.z = 0;
         rotPlayer.y += rotateX;

         transform.rotation = Quaternion.Euler(rotCamera);
         player.transform.rotation = Quaternion.Euler(rotPlayer);     
        }
        else
        {
    // Ottenere la rotazione corrente della telecamera
          Vector3 rotCamera = transform.rotation.eulerAngles;
    // Modifica della rotazione della telecamera in base al movimento del mouse
          rotCamera.x -= rotateY;
          rotCamera.z = 0;
          rotCamera.y += rotateX;
          transform.rotation = Quaternion.Euler(rotCamera);
    // Lettura della pressione dei tasti WASD 
          float x = Input.GetAxis("Horizontal");
          float z = Input.GetAxis("Vertical");

    // Impostazione del vettore di movimento della telecamera
          Vector3 dir = transform.right * x + transform.forward * z;
       
          transform.position += dir * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Se il cursore del mouse è bloccato, allora...
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                // Sblocco del cursore
                Cursor.lockState = CursorLockMode.None;
            }
            // Altrimenti...
            else
            {
                // Blocco del cursore
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        
    }
}