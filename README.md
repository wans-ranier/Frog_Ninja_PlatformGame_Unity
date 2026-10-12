# 🐸 Frog Ninja

Jogo de **plataforma 2D** desenvolvido na **Unity 6** (6000.0.35f1) com **Universal Render Pipeline (URP)**, tiles 16x16 e sprites do pacote *Pixel Adventure 1*.

O jogador percorre fases cheias de obstáculos, inimigos e coletáveis para alcançar o ponto de saída de cada nível. Algumas mecânicas clássicas de platformer estão presentes: pulo duplo, trampolim, queda de verdade, plataformas que desmoronam e inimigos que precisam ser derrotados pisando na cabeça.


---

# Assets Visuais (Pixel Adventure 1)

Os arquivos binários desta pasta não são versionados no repositório.

1. Baixe o pacote gratuito em: https://pixelfrog-assets.itch.io/pixel-adventure-1
2. Extraia o conteúdo dentro desta pasta, mantendo a estrutura
   (Animations, Background, Items, Main Characters, Terrain, Traps...)
3. Reabra o projeto na Unity — as referências dos prefabs e cenas
   serão restauradas automaticamente.

---

# Créditos dos Assets Visuais

Este projeto utiliza assets visuais de uso livre fornecidos pelo criador Pixel Frog.

🎨 Informações da Arte

Criador: [Pixel Frog](https://pixelfrog-assets.itch.io/)

Pacote de Assets: [Pixel Adventure 1](https://pixelfrog-assets.itch.io/pixel-adventure-1)

Personagem Utilizado: **Ninja Frog**

Os assets visuais deste repositório permanecem sob os direitos de uso e termos originais definidos pelo criador no itch.io.

---

## 🎮 Sobre o jogo

Você controla um ninja-sapo em fases horizontais repletas de perigos. O objetivo de cada fase é simples: **chegar até o ponto final** (NextLevel) vivo e coletando o máximo de maçãs possível. No caminho há armadilhas mortais — serras, espinhos e abismos — e inimigos que patrulham ou perseguem o jogador.

**Conteúdo:**

| Elemento | Descrição |
| --- | --- |
| **Cenários em Tilemap** | Terrenos montados com tiles 16x16 (`Assets/Tiles`), incluindo áreas de chão falso que desmoronam |
| **Fases** | `level_1` até `level_4` (mais `SampleScene` e `SampleScene 1` de prototipagem) |
| **Inimigos** | Inimigos terrestres de patrulha e um perseguidor que detecta o jogador por distância |
| **Armadilhas** | Espinhos, serras animadas, plataformas que caem e chão que desmorona |
| **Coletáveis** | Maçãs que aumentam a pontuação (com VFX de coleta) |
| **HUD** | Texto de pontuação, painel de Game Over e painel de próximo nível |

---

## 🕹️ Controles

Os controles usam o **Input Manager clássico** da Unity (eixos horizontais e botão *Jump*):

| Ação | Teclado |
| --- | --- |
| Andar para os lados | `A` / `D` ou ← / → |
| Pular (com pulo duplo) | `Espaço` |

Ao andar, o sprite se vira para a direção do movimento (rotação em 180° no eixo Y).

---

## ⚙️ Funcionalidades

### 1. Jogador (`Player`)

`Assets/Scripts/Player.cs`

- **Movimentação horizontal** com `Speed` e animações de *walk* via `Animator`.
- **Pulo com impulso** (`JumpForce`) usando `AddForce` no `Rigidbody2D` no modo `Impulse`.
- **Pulo duplo**: após o primeiro pulo, um segundo pode ser executado com força maior (`JumpForce * 1.5`), desde que o jogador não esteja sobre um trampolim (`onTrampoline`).
- **Estados de animação**: `walk`, `jump` e `fall` (a queda é detectada quando a velocidade vertical é negativa enquanto o jogador está no ar).
- **Vida e cura**: propriedade `Health` (inicial 100) com `TakeDamage(float)` e `Heal(float)` (`Player.cs:15-33`).
- **Morte**: ao encostar nas tags `Spike` ou `Saw`, morre imediatamente, destrói o objeto e exibe o Game Over (`Player.cs:113-120`).
- **Morte por inimigo**: o `Player` implementa o sistema de dano e, ao zerar a vida, destrói-se e chama `GameController.instance.ShowGameOver()` (`Player.cs:35-40`).
- **Posição segura**: registra em `lastSafePosition` o último ponto pisado em solo válido (Layer 8), base para um anti-travamento previsto no código.
- **Detecção de colisão com paredes**: `DetectorColisaoParede` usa `Physics2D.OverlapBox` nas extremidades do jogador e desenha os pontos de checagem no editor via `Gizmos`.

### 2. Personagens e animação

- `Animator` no jogador controla os estados *walk*, *jump* e *fall*.
- Controladores dedicados em `Assets/Animations`: `Saw.controller`, `Falling Platform.controller`, `Collected.controller`, `Apple.controller`, `Image.controller`, entre outros — usados nos prefabs de coletável, serra e plataformas.

### 3. Coletáveis e pontuação (`Apple`)

`Assets/Scripts/Apple.cs`

- Maçãs com colisão **trigger**: ao tocar o jogador, desativam o sprite e o `CircleCollider2D`, ativam um efeito de coleta (`collected`) e somam `Score` ao `totalScore` do `GameController`.
- O texto da HUD é atualizado automaticamente (`UpdateScoreText`), e o objeto é destruído após 0,25 s para a animação de coleta terminar.

### 4. Inimigos

**Hierarquia de classes:**

```
EnemySimple (abstrata) ──> Enemy ──> EnemyDesconfia
```

- **`EnemySimple`** (`Assets/Scripts/EnemiesClasses/EnemySimple.cs`): base abstrata de todo inimigo terrestre. Implementa patrulha automática com `rig.linearVelocity` e um `Linecast` entre dois pontos (`Rightcol`/`Leftcol`) que inverte a direção e o sprite ao detectar parede ou borda.
- **`Enemy`** (`Assets/Scripts/EnemiesClasses/Enemy.cs`): especialização que apenas reutiliza/complementa o comportamento da classe pai.
- **`EnemyDesconfia`** (`Assets/Scripts/EnemiesClasses/EnemyDesconfia.cs`): variante que não confia no chão — usa um *raycast* para baixo (`groundDetection`, `DistanceBeam`, `whatIsGround`) e muda de direção ao detectar a ausência de solo, evitando cair.
- **`Stalker`** (`Assets/Scripts/EnemiesClasses/Stalker.cs`): perseguidor. Em modo de patrulha anda para frente e para; ao entrar em `detectionRange` do jogador, passa a segui-lo ativamente e a orientar o sprite para o lado correto.
- **Regra de combate**: o jogador pode derrotar inimigos pisando na cabeça. O sistema compara a altura do ponto de contato com `headPoint` (`EnemySimple.cs:34`): se o contato for por cima, o jogador sofre um *bounce* (`AddForce` para cima) e o inimigo morre (desativa colisores, trigger `die` e destruição); se for na lateral, o inimigo causa Game Over e destrói o jogador.

### 5. Armadilhas e obstáculos

| Funcionalidade | Script | Como funciona |
| --- | --- | --- |
| **Trampolim** | `TrompolineJump.cs` | Ao tocar no jogador, dispara a animação de pulo e aplica impulso vertical forte (`ForceMode2D.Impulse`). Enquanto o jogador está sobre ele, o pulo duplo é desativado. |
| **Plataforma que cai** | `FallingPlatform.cs` | Ao ser tocada pelo jogador, agenda `Invoke("Falling", fallingTime)`. Quando acionada, desativa o `TargetJoint2D` e transforma o colisor em *trigger*, deixando o jogador cair. |
| **Chão falso** | `FalseGround.cs` | Detecta se o jogador está sobre um tile do Tilemap e desmorona os tiles num raio (`raioDesmoronamento`) durante `tempoDesmoronamento`, removendo as células do mapa. |
| **Serras** | `VaiSaw.cs` | Serras que se movem automaticamente para a esquerda e para a direita, trocando de direção a cada `MoveTime` segundos. Toque no jogador = morte imediata. |
| **Espinhos** | tag `Spike` | Tratados diretamente na colisão do `Player.cs:113`: morte imediata. |
| **`Armadilha.cs`** | `Assets/Scripts/Armadilha.cs` | Estrutura reservada (ainda sem lógica) para novas armadilhas. |
| **`testeSaw.cs`** | `Assets/Scripts/testeSaw.cs` | Variante de teste do trampolim aplicável a outros objetos. |

### 6. Sistema de vida, dano e ataque

Localizado em `Assets/Scripts/EnemiesClasses/Interfaces/sistem-Heath/`:

- **`IDamageable`**: contrato com `Health`, `damage`, `TakeDamage(int)` e `Heal(float)`.
- **`TriggerDamage`**: componente genérico que, em `OnTriggerEnter2D`, aplica 10 de dano a qualquer `IDamageable` que tocar a área. É o padrão para criar áreas de dano reaproveitáveis.
- **`DeathOnDamage`**: implementação simples que destrói o objeto ao receber dano.
- **`IAttackBasic`**: contrato de ataque básico com `AttackRange` e `Attack(Transform target)`.
- **`Snipers.cs`**: interface `ISniper` com `ExecuteSpecialAttack(int)`, `DispararProjetil(...)` e `AreaOfEffectAttack(float, int)` — prevê inimigos atiradores com projéteis e ataque em área (ainda sem implementação).

### 7. Progressão e cenas (`GameController`)

`Assets/Scripts/GameController.cs` (singleton via `static GameController instance`):

- **Pontuação** exibida em `scoreText` (`UpdateScoreText`).
- **Game Over**: `ShowGameOver()` ativa o painel `gameOver`.
- **Reinício**: `RestartGame(lvlName)` recarrega a cena pelo nome.
- **Próximo nível**: `ShowNextLevel()` ativa o painel `panelNextLvl`. O gatilho de saída de cada fase é o prefab `NextLevel` (`NextLevelPoint.cs`), que ao ser tocado pelo jogador mostra o painel em vez de trocar a cena diretamente (a troca está comentada no código para permitir confirmação).

### 8. Ferramentas de editor

`Assets/Editor/PlayModeUtility.cs`: adiciona itens de menu `Tools > Start Play Mode` / `Tools > Stop Play Mode` para agilizar testes de gameplay sem usar o botão Play da Unity.

---

## 📁 Estrutura do projeto

```
Assets/
├── Animations/      # Controllers e animações (Saw, Falling Platform, Apple, Collected...)
├── Editor/          # Utilitários do editor da Unity
├── Pixel Adventure 1/  # Pacote de sprites
├── Prefabs/         # Player, Apple, Saw, Trampoline, NextLevel, painéis, inimigos
├── Scenes/          # level_1, level_2, level_3, level_4, SampleScene, SampleScene 1
├── Scripts/         # Todo o código de gameplay
│   └── EnemiesClasses/
│       ├── EnemySimple.cs, Enemy.cs, EnemyDesconfia.cs, Stalker.cs
│       └── Interfaces/
│           ├── Snipers.cs
│           └── sistem-Heath/ (IDamageable, IAttackBasic, TriggerDamage, DeathOnDamage)
├── Tiles/           # Tiles de terreno 16x16
├── Resources/       # Recursos carregáveis em runtime
└── Settings/        # Configurações de URP / volume
```

---

## 🚀 Como executar

1. Clone o repositório
2. Baixe o pacote em https://pixelfrog-assets.itch.io/pixel-adventure-1
3. Extraia em `Assets/Pixel Adventure 1/` (mantendo a estrutura de pastas)
4. Abra o projeto na Unity 6 e dê Play

> Após importar, os sprites dos prefabs e das animações precisam ser
> religados manualmente (Sprte Renderer > Sprite, e nos clipes em
> Window > Animation), pois os arquivos binários não são versionados.

---


## 🔧 Observações técnicas

Pontos conhecidos no estado atual do código, úteis para quem for contribuir:

- `Player` possui `TakeDamage`/`Heal`, mas **não implementa `IDamageable`**, então `TriggerDamage` não afeta o jogador — isso precisa de ajuste para o sistema de áreas de dano funcionar em conjunto.
- `FalseGround.cs` contém uma comparação incomum (`tilemap.GetTile(pos) == tilemap`) e um fluxo de desmoronamento que remove tiles continuamente; a lógica de detecção do *tile* de chão falso precisa ser revisada.
- O método `CheckIfStuck()` no `Player` existe, mas **não é chamado** no `Update` — o anti-travamento previsto com `lastSafePosition` ainda não está ativo.
- Mudança de cena em `NextLevelPoint` está comentada: atualmente o jogador apenas vê o painel de próximo nível.
- `VaiSaw.cs` e `DeathOnDamage.cs` têm apenas `Debug.Log`/destruição — a morte por serra ocorre pelo contato com a tag `Saw` no `Player`.
