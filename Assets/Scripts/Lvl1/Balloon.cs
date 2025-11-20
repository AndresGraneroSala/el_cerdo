using UnityEngine;

public class Balloon : MonoBehaviour
{
    private bool _exploded;

    private void Start()
    {
        Manager1.Instance.AddBalloon();
    }

    private void OnTriggerEnter(Collider other)
    {
        //un if porque si no me detecta más de uno como van muy rápidas las balas y son varias eso es malo 
        if (_exploded)
        {
            return;
        }

        if (other.CompareTag("BulletP"))
        {
            _exploded = true;
            Manager1.Instance.BalloonExplode();

            Destroy(gameObject);
        }
    }
}