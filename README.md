# ♟ Royal Tactics

**Royal Tactics** é um protótipo tático por turnos desenvolvido na Unity, inspirado em regras de movimentação do xadrez, mas concebido como um sistema original de combate em grid.

O projeto tem foco em **arquitetura de gameplay, sistemas determinísticos e design orientado a regras**, priorizando clareza técnica e extensibilidade em vez de gráficos ou animações.

---

## 🎯 Objetivos do Projeto

* Projetar um **sistema tático em grid** independente do tamanho do tabuleiro
* Implementar **movimentação baseada em regras** inspiradas em peças de xadrez
* Construir um **sistema de turnos determinístico**, fácil de depurar
* Explorar **combate por linha de visão** utilizando lasers direcionais
* Demonstrar **design sistêmico** com uso de recursos criados pelo jogador (peões)

---

## 🕹 Visão Geral do Gameplay

* Tabuleiro: **10×10**
* O jogador controla uma **Dama**
* Inimigos:

  * **Torres**
  * **Bispos**
* O combate é resolvido por **lasers direcionais**
* O jogador ganha **peões** ao derrotar inimigos e pode posicioná-los como bloqueios estratégicos
* A vitória ocorre ao eliminar todas as peças inimigas

---

## 🔁 Loop Principal de Jogo

1. O jogador executa **uma ação**:

   * Mover a Dama
     **ou**
   * Posicionar um Peão
2. As peças inimigas se movimentam
3. As peças inimigas disparam seus lasers
4. O sistema resolve colisões e danos
5. O jogo verifica vitória ou derrota
6. O turno avança

---

## 🧩 Sistemas Principais

### Sistema de Grid

* Implementação genérica **NxM**
* Controle de ocupação das células
* Consulta rápida de bloqueios
* Conversão entre coordenadas de grid e mundo

---

### Sistema de Turnos

* Fluxo determinístico baseado em estados:

  * `PlayerTurn`
  * `EnemyMove`
  * `EnemyAttack`
  * `Resolve`
* Suporte a execução passo a passo para debug

---

### Sistema de Unidades

* Classe base (`BaseUnit`)
* Movimentação definida por **padrões reutilizáveis**
* Comportamentos inspirados em peças de xadrez, sem seguir as regras tradicionais

---

### Sistema de Combate (Laser)

* Ataques em linha reta sobre o grid
* Lasers percorrem célula por célula
* O laser é interrompido ao colidir com:

  * Uma unidade
  * Um peão
  * O limite do tabuleiro

---

### Sistema de Recursos e Posicionamento

* Peões são obtidos ao derrotar inimigos
* Quantidade máxima armazenada é limitada
* Posicionamento validado por:

  * Adjacência à Dama
  * Ocupação da célula

---

### IA Inimiga

* Movimentação baseada em regras da peça
* Seleção de direção do laser com pesos simples
* Comportamento totalmente determinístico por turno

---

## 🧠 Foco Técnico

Este projeto foi desenvolvido para enfatizar:

* Arquitetura de gameplay
* Separação clara de responsabilidades
* Design orientado a dados
* Sistemas determinísticos
* Performance em lógica de grid
* Ferramentas de debug para sistemas de jogo

Não utiliza física, animações complexas ou sistemas gráficos avançados.

---

## 🚀 Extensibilidade

A arquitetura permite facilmente:

* Adição de novas peças
* Criação de novos padrões de movimento
* Novos tipos de ataque
* Diferentes tamanhos de tabuleiro
* Modos de jogo adicionais

---

## 🛠 Tecnologias Utilizadas

* Unity
* C#
* New Input System da Unity
* Sem frameworks externos de gameplay

---

## 📌 Propósito do Projeto

**Royal Tactics** não é um jogo comercial completo.
Ele existe como **vitrine técnica**, demonstrando como estruturar e implementar sistemas de gameplay escaláveis em Unity.

---
