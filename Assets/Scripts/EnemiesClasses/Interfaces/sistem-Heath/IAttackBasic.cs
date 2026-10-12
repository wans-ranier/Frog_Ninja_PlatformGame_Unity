using UnityEngine;

public interface IAttackBasic
{
    float AttackRange { get; }      // define o alcance do ataque. Cada inimigo que implementar essa interface terá seu próprio alcance de ataque.
    void Attack(Transform target);   // Método para realizar o ataque

}