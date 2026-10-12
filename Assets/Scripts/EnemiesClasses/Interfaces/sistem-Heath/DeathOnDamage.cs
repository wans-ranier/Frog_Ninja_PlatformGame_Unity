using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damage)
    {
        Destroy(gameObject);
        Debug.Log($"{gameObject.name} recebeu {damage} de dano e foi destruído.");
    }
}
