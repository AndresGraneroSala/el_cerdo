using System;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    private bool exploded = false;
    private void Start()
    {
        Manager1.instance.AddBalloon();
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        
        //porque si no me detecta más de uno como van muy rápidas las balas y son varias pues eso  
        if (exploded)
        {
            print("ex");
            return;
        }
        if (other.CompareTag("BulletP"))
        {
            exploded = true;
            print("explo");
            Manager1.instance.BalloonExplode();
            
            Destroy(gameObject);
        }
    }
}
