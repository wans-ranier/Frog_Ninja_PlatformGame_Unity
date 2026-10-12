using UnityEngine;

public class Stalker : MonoBehaviour
{
    public float speed = 2f; // Velocidade de movimento do inimigo
    public float detectionRange = 5f; // Alcance de detecção do jogador
    private bool movingRight = true; // Indica se o inimigo está se movendo para a direita
    public Transform groundDetection; // Ponto de detecção de chão
    private Transform player; // Referência ao jogador
    protected Animator anim;
    //ATENÇÃO: LEIA O COMENTÁRIO ABAIXO!!
//Os dois objetos filhos do inimigo são usados para detectar colisões com o chão e o jogador
//Objeto fica um pouco a frente do inimigo e um pouco pra baixo. Se o inimigo não detectar chão, ele muda de direção
    void Start()
    {
        // Encontra o jogador na cena usando a tag "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Patrol(); // Chama o método de patrulha
        DetectPlayer(); // Chama o método de detecção do jogador
    }

    void Patrol()
    {
        // Move o inimigo para a direita ou esquerda com base na direção atual
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Lança um Raycast para baixo a partir do ponto de detecção de chão
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 2f);
        // Se o Raycast não detectar chão, o inimigo muda de direção
        if (groundInfo.collider == false)
        {
            if (movingRight)
            {
                // Muda a direção para a esquerda
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                // Muda a direção para a direita
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

    void DetectPlayer()
    {
        // Calcula a distância entre o inimigo e o jogador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        // Se o jogador estiver dentro do alcance de detecção, o inimigo começa a segui-lo
        if (distanceToPlayer < detectionRange)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        // Se o jogador estiver à direita do inimigo, move o inimigo para a direita
        if (transform.position.x < player.position.x)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            // Se o jogador estiver à esquerda do inimigo, move o inimigo para a esquerda
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
    }
}
