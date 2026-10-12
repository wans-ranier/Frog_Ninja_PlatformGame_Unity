using UnityEngine;

public interface IDamageable
{
    float Health { get; }  // Propriedade para obter a saúde do objeto (em inglês)
    float damage { get; }  // Propriedade para obter o dano do objeto (em inglês)
    void TakeDamage(int damage);  // Método para receber dano (em inglês)
    void Heal(float amount); // Novo método para curar
}