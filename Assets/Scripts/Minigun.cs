using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        //Il ritardo tra i colpi (puoi mettere il valore che vuoi)
       cooldown = 0.1f;
    //Quest'arma sparerà in automatico; continuerà a sparare finché teniamo premuto il pulsante del mouse (non ti preoccupare: il ritardo che hai definito prima verrà tenuto in considerazione!)
       auto = true;
       ammoCurrent = 100;
       ammoMax = 100;
       ammoBackPack = 200;
    }

    protected override void OnShoot()
    {
        Vector3 rayStartPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        
        Vector3 drift = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15));

        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(rayStartPosition + drift);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject gameBullet = Instantiate(particle, hit.point, hit.transform.rotation);
            if(hit.collider.CompareTag("enemy"))
            {
                hit.collider.gameObject.GetComponent<Enemy>().GetDamage(10);
            }
            else  if (hit.collider.CompareTag("Player"))
            {
                hit.collider.gameObject.GetComponent<PlayerController>().GetDamage(10);
            }
            Destroy(gameBullet, 1);
        }
    }
}
