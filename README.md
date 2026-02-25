# ♟️ Royal Tactics

**Royal Tactics** é um jogo inspirado no xadrez, focado em **desafios táticos rápidos**, aprendizado progressivo e alta rejogabilidade.

O projeto foi desenvolvido com uma **arquitetura modular e escalável**, permitindo a criação de múltiplos modos de jogo e fases reutilizando os mesmos sistemas centrais.

Cada partida é curta, objetiva e baseada em **análise de posição**, não em partidas completas de xadrez tradicional.

---

## 🎯 Objetivo do Projeto

Royal Tactics é um projeto autoral com foco em:

* Design de sistemas de jogo
* Arquitetura escalável
* Separação clara entre lógica, dados e UI
* Geração dinâmica de conteúdo
* Criação de experiências educativas e táticas

O jogo foi pensado tanto para **aprendizado de xadrez** quanto como **exercício de engenharia de software aplicada a games**.

---

## 🧩 Estrutura Geral do Jogo

* Tabuleiro **8x8**
* Jogador vs IA
* Partidas baseadas em **puzzles táticos**
* Progressão por **modos → fases**
* Nenhuma fase é fixa: posições são **geradas dinamicamente**

Cada modo possui:

* Regras próprias
* Conjunto específico de peças
* Critérios de validação independentes
* Fases progressivas

---

## 🎮 Modos de Jogo

### ♜ Vantagem de Pontos

Capture peças mais valiosas antes do oponente.

* Cada peça possui um valor
* O jogador deve escolher a melhor captura disponível
* Foco em avaliação material e priorização de jogadas

---

### ❓ Peça Misteriosa

Identifique qual peça está sendo representada apenas pela sua movimentação.

* O jogador observa as casas alcançáveis
* Deve escolher qual peça corresponde àquele padrão
* Foco em reconhecimento de padrões e movimentação

---

### 🔤 Nome das Casas

Treino da nomenclatura tradicional do tabuleiro de xadrez.

* O jogador deve identificar corretamente as casas (ex: A1, E4, H8)
* Progressão de dificuldade por fase
* Foco em coordenação espacial e notação algébrica

---

### 📍 Posicionamento Correto

Aprenda e treine a posição inicial das peças no tabuleiro.

* O jogador deve posicionar corretamente cada peça
* Foco em memorização e compreensão do setup inicial

---

> 🚧 **Novos modos em desenvolvimento**
> Dois novos modos já estão em estágio avançado e serão lançados em breve.

---

## 🔄 Progressão e Fases

* Cada modo possui várias fases
* A **fase 1 de cada modo inicia desbloqueada**
* As fases seguintes:

  * começam bloqueadas
  * são desbloqueadas ao vencer a fase anterior
* O progresso é salvo utilizando **PlayerPrefs**

O sistema foi projetado para permitir futura migração para banco de dados sem refatorações complexas.

---

## 🎲 Geração Dinâmica de Posições

* As posições do tabuleiro são geradas dinamicamente
* Baseadas em regras específicas de cada modo
* Evitam padrões fixos e repetição de fases
* Garantem alta rejogabilidade

Nenhuma fase é exatamente igual à outra.

---

## 🧠 IA

* A IA posiciona peças e gera cenários de acordo com o modo
* Atualmente utiliza regras determinísticas
* Próxima etapa:

  * análise de posição
  * tomada de decisão baseada em contexto

---

## 🎨 Customização Visual

O jogador pode personalizar:

* Cor das próprias peças
* Cor das peças do oponente
* Cor do tabuleiro

Regras:

* As cores não podem ser iguais
* Customização puramente visual
* Preferências persistidas entre sessões

O sistema evita duplicação de materiais e mantém a lógica de gameplay desacoplada da renderização.

---

## 🛠️ Tecnologias e Arquitetura

* **Unity**
* **C#**
* Uso extensivo de **ScriptableObjects** para:

  * definição de modos
  * fases
  * regras
* Arquitetura baseada em:

  * baixo acoplamento
  * alta reutilização
  * fácil expansão de conteúdo

O projeto foi estruturado desde o início pensando em **crescimento contínuo**.

---

## 🧪 Estado Atual do Projeto

### Implementado

* Sistema de modos de jogo
* Sistema de fases
* Geração dinâmica de posições
* Progressão e desbloqueio
* Persistência de progresso
* Suporte a PC e mobile
* Base sólida de UI e fluxo de navegação

### Em Desenvolvimento

* Novos modos de jogo
* Expansão de fases existentes
* Melhoria visual da UI
* Evolução da IA

---

## 📌 Considerações Finais

**Royal Tactics** é um projeto em constante evolução, desenvolvido como iniciativa pessoal, cobrindo desde o design até a implementação técnica.

Ele serve tanto como:

* Ferramenta de aprendizado de xadrez
* Demonstração prática de arquitetura e design de sistemas em jogos

É só dizer.
