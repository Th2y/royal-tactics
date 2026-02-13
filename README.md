# ♟️ Royal Tactics

**Royal Tactics** é um jogo de tabuleiro tático por turnos inspirado no xadrez, mas com foco em **controle de território, economia de peças e decisões estratégicas**, em vez de regras tradicionais.

O jogo propõe partidas curtas, alta rejogabilidade e um sistema de vitória original baseado em **Domínio**, não em xeque-mate.

---

## 🎯 Conceito Central

* Tabuleiro **8x8**
* Dois jogadores (Jogador vs IA)
* Não existe Rei
* Cada peça possui um **custo em pontos**
* Pontos são usados tanto para **posicionar peças** quanto para **vencer a partida**

O jogo é estruturado para que **cada decisão importe**: posicionamento inicial, troca de peças e controle da vantagem ao longo dos turnos.

---

## ♜ Peças e Custos

| Peça   | Custo |
| ------ | ----- |
| Peão   | 1     |
| Cavalo | 3     |
| Bispo  | 3     |
| Torre  | 5     |
| Dama   | 9     |

---

## 🧩 Fase Inicial — Posicionamento

1. Ao iniciar a partida, **cada lado recebe x pontos** (rodada 1 = 10 pontos, as outras vai aumentando de 2 em 2 pontos até chegar a 20 pontos)
2. O **oponente posiciona suas peças primeiro**, livremente no tabuleiro
3. Em seguida, o **jogador posiciona suas peças**, também livremente
4. O jogador **não vê as peças do oponente**, apenas casas bloqueadas

Essa fase cria um cenário de **informação incompleta**, incentivando leitura de jogo e antecipação de movimentos.

---

## 🔄 Turnos de Jogo

A partir do segundo turno, o jogo começa de fato.

Em cada turno:

* Apenas **uma peça** pode agir
* A ação pode ser:

  * **Atacar**, se houver uma peça inimiga ao alcance
  * **Mover**, caso nenhum ataque seja possível

### Prioridade de Ação

O sistema sempre tenta:

1. Atacar
2. Caso não seja possível, mover

---

## ⚔️ Combate e Pontuação

* Ao atacar:

  * A peça se move para a casa da peça inimiga
  * A peça inimiga é removida
* O jogador ganha:

  * **(Valor da peça capturada − 1) pontos**

Esses pontos podem ser usados em **turnos futuros** para posicionar novas peças no tabuleiro.
Posicionar uma peça **consome o turno inteiro**.

---

## 🏆 Condição de Vitória — Domínio

O jogo **não é vencido por eliminar todas as peças**.

### Vitória por Domínio

Um jogador vence ao manter:

* Uma vantagem mínima de **x pontos** (valor recebido inicialmente pela rodada dividido por 2)
* Considerando o **valor total das peças ativas no tabuleiro**
* Por **3 turnos consecutivos**

O valor considerado é sempre o **custo original da peça**, independentemente de como ela foi obtida.

Essa condição:

* Evita partidas longas
* Incentiva trocas inteligentes
* Gera tensão constante
* Permite reviravoltas

---

## 🎨 Sistema de Cores

O jogador pode escolher:

* A cor das **suas peças**
* A cor das **peças do oponente**

Regras:

* As cores **não podem ser iguais**
* A escolha é puramente visual
* As cores são persistidas entre sessões

O sistema foi projetado para evitar duplicação de materiais, com aplicação de cor desacoplada da lógica de gameplay.

---

## 🧪 Estado Atual do Projeto

### Implementado

* Menu inicial
* Seleção de cores do jogador e do oponente
* Validação para impedir cores iguais
* Persistência da escolha de cores
* Estrutura base de UI e fluxo inicial

### Em Desenvolvimento

* Lógica do tabuleiro
* Sistema de turnos
* Posicionamento inicial das peças
* Sistema de ataque e movimento
* Condição de vitória por Domínio
* IA do oponente

---

## 🛠️ Tecnologias

* **Unity**
* **C#**
* Arquitetura focada em:

  * separação de responsabilidades
  * baixo acoplamento
  * escalabilidade

---

## 📌 Objetivo do Projeto

**Royal Tactics** é um projeto autoral com foco em:

* Design de sistemas
* Lógica de gameplay
* Arquitetura limpa
* Tomada de decisões técnicas conscientes
