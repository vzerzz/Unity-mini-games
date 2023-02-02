using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    // Start is called before the first frame update
    void Awake()//实例化时不调用start
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (transform.position.magnitude > 1000.0f)
            Destroy(gameObject);
    }
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Projectile Collision with " + collision.gameObject);
        EnemyController e = collision.collider.GetComponent<EnemyController>();
        if (e != null)
            e.Fix();
        Destroy(gameObject);
    }
}
