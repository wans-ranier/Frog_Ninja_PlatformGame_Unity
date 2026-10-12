using UnityEngine;

public class Enemy : EnemySimple
{
    // Sobrescreve o método Start
    protected override void Start()
    {
        // Chama o Start da classe pai para manter o comportamento padrão
        base.Start();

        // Adiciona novos comportamentos específicos para a classe filha
    }

    // Sobrescreve o método Update
    protected override void Update()
    {
        // Chama o Update da classe pai para manter o comportamento padrão
        base.Update();
        Moviment();
    }


}
