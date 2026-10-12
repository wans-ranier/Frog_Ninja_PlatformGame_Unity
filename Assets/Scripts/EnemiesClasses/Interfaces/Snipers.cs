using UnityEngine;

public interface ISniper
{
    void ExecuteSpecialAttack(int damage);
    void DispararProjetil(GameObject projectilePrefab, Transform spawnPoint, Vector2 direction);
    void AreaOfEffectAttack(float radius, int damage); // Ataque em área
}