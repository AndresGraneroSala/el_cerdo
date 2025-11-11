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
        //un if porque si no me detecta más de uno como van muy rápidas las balas y son varias eso es malo 
        if (exploded)
        { 
            return;
        }
        if (other.CompareTag("BulletP"))
        {
            exploded = true;
            Manager1.instance.BalloonExplode();
            
            Destroy(gameObject);
        }
    }
}
