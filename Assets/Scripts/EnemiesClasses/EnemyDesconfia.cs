using UnityEngine;

public class EnemyDesconfia : Enemy
{
    public float DistanceBeam = 8f;
    protected bool movingRight = true; // Indica se o inimigo está se movendo para a direita
    public Transform groundDetection; // Ponto de detecção de chão
    protected Transform player; // Referência ao jogador
    public LayerMask whatIsGround; // Camada do chão
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        Moviment();
        // verificarPosicao();
    }


    protected override void Moviment()
    {

       // Lança um Raycast para baixo a partir do ponto de detecção de chão
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, DistanceBeam, whatIsGround);
        // Se o Raycast não detectar chão, o inimigo muda de direção  
        base.Moviment();
       
        if (groundInfo.collider == false)
        {
            if (movingRight)
            {
                // Muda a direção para a esquerda
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                speed *= -1f;
                movingRight = false;
            }
            else
            {
                // Muda a direção para a direita
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                speed *= -1f;
                movingRight = true;
            }
        }
    }

}
