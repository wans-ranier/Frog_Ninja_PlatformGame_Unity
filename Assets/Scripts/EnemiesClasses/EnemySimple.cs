using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemySimple : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rig;
    public float speed = 2f;
    public Transform Rightcol;
    public Transform Leftcol;
    public Transform headPoint;
    protected bool colliding;
    public LayerMask collidingWall;

    public BoxCollider2D boxCollider2D;
    public CircleCollider2D circleCollider2D;
    protected bool playerDestroyed = false; // Define a velocidade do Rigidbody2D
    virtual protected void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    //virtual-> Update permite que o método seja sobrescrito na classe filha
    virtual protected void Update()
    {
        Moviment();
    }
    // para não chamarmos o gameover mais de uma vez
    virtual protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float height = collision.contacts[0].point.y - headPoint.position.y;

            if (height > 0 && !playerDestroyed)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                speed = 0;
                anim.SetTrigger("die");
                boxCollider2D.enabled = false;
                circleCollider2D.enabled = false;
                rig.bodyType = RigidbodyType2D.Kinematic;
                Destroy(gameObject, 0.33f);
            }
            else
            {
                playerDestroyed = true;
                GameController.instance.ShowGameOver();
                Destroy(collision.gameObject);
            }
        }
    }

    virtual protected void Moviment()
    {
        rig.linearVelocity = new Vector2(speed, rig.linearVelocity.y);
        colliding = Physics2D.Linecast(Rightcol.position, Leftcol.position, collidingWall); // verifica se está colidindo com algo

        if (colliding)
        {
            InvertDirection();
        }

        //  RaycastHit2D hit = Physics2D.Linecast(Rightcol.position, Leftcol.position, collidingWall);
        // colliding = hit && hit.collider != null && hit.collider.gameObject != this.gameObject;

        // if (colliding) rig.linearVelocity = new Vector2(speed, rig.linearVelocity.y);

        // {
        //     InvertDirection();
        // }
    }
    
    virtual protected void InvertDirection()
    {
       // Inverte a direção do sprite
            transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);

            // Inverte a direção do movimento
            speed *= -1f;
    }

}
