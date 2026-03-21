# DESING PATTERN - OBSERVER (Padrão Comportamental)
## Monitor de Mercado Financeiro 

**Nome:** Marilene Araujo  
**Disciplina:** Desenvolvimento de Sistemas  
**Instrutor:** Frederico Martins Aguiar  
**Unidade:** SENAI - Nova Lima

---
## 1. Introdução

A constante necessidade de **sincronização de dados** em sistemas complexos é um dos maiores desafios da engenharia de software moderna. Em aplicações onde múltiplos componentes precisam reagir a mudanças de estado de um único objeto central, o **acoplamento rígido** entre essas partes pode tornar o código frágil, difícil de testar e quase impossível de expandir.

Este projeto apresenta a implementação de um **Monitor de Mercado Financeiro** desenvolvido em **C#** com interface **WPF** (*Windows Presentation Foundation*). 

### Objetivos Principais:
* **Demonstração Prática:** Aplicar o padrão de projeto **Observer** para resolver a distribuição de informações em tempo real.
* **Arquitetura Desacoplada:** Utilizar o padrão **MVVM** (*Model-View-ViewModel*) para garantir que o motor de dados (**Sujeito**) notifique diversas interfaces e serviços (**Observadores**) de forma assíncrona.
* **Independência de Camadas:** Manter a lógica de negócio totalmente independente da interface gráfica, facilitando a manutenção e a escalabilidade do sistema.
---
## 2. Definição do Padrão

O **Observer** (ou *Observador*) é um padrão de projeto **comportamental** que estabelece uma relação de dependência do tipo **um-para-muitos** entre objetos. O objetivo principal é garantir que, quando um objeto muda de estado, todos os seus dependentes sejam notificados e atualizados automaticamente de forma desacoplada.

### Funcionamento do Mecanismo
O padrão funciona através de dois componentes principais que interagem por meio de abstrações (interfaces):

1.  **O Sujeito (Subject / Publisher):** É o detentor da informação principal ou do estado de interesse. Ele possui uma lista interna de observadores e fornece métodos para que novos interessados possam se "inscrever" (`Subscribe`) ou "cancelar a assinatura" (`Unsubscribe`) em tempo de execução.
2.  **O Observador (Observer / Subscriber):** É o componente que deseja ser informado sobre as mudanças no Sujeito. Ele não monitora o Sujeito constantemente (evitando o gasto de processamento por *polling*); em vez disso, ele aguarda passivamente ser "chamado" pelo Sujeito através de um método de atualização.

### Pilares Teóricos do Padrão

* **Acoplamento Fraco (*Loose Coupling*):** O Sujeito não precisa conhecer as classes concretas dos observadores (se é uma tela WPF, um log de texto ou um serviço de e-mail). Ele interage apenas com uma **Interface**, o que permite que o sistema cresça sem que as partes dependam intimamente umas das outras.
* **Inversão de Controle:** Em vez de a interface gráfica perguntar ao motor de dados "o preço mudou?", o motor de dados é quem empurra a informação para a interface no momento exato da alteração.
* **Comunicação *Broadcast*:** A notificação é enviada para todos os assinantes simultaneamente. O Sujeito não se preocupa com o que cada observador fará com a informação recebida; sua única responsabilidade é entregar o dado.

### Representação Técnica
Para implementar esse padrão, define-se um contrato (Interface) que todos os observadores devem seguir. Geralmente, essa interface possui um método central:

> **`Atualizar(dados)`**: Este método é o ponto de entrada da notificação. Quando o Sujeito detecta uma mudança, ele percorre sua lista de inscritos e dispara este método para cada um deles, passando o novo estado como parâmetro.
---

## 3. Problema que Resolve
## 4. Estrutura e Diagrama de Classes
## 5. Participantes do Padrão
## 6. Justificativa da Escolha do Contexto (Mercado Financeiro)
## 7. Explicação da Implementação no Projeto
### 7.1. Camada Model
### 7.2. O Sujeito (Motor do Mercado)
### 7.3. A Interface do Observador
### 7.4. O Observador Concreto (ViewModel)
## 8. Integração MVVM e Boas Práticas
## 9. Análise Crítica
### 9.1. Comparação: Com Padrão vs. Sem Padrão
### 9.2. Vantagens Observadas
### 9.3. Desvantagens e Limitações
## 10. Exemplos Reais de Uso no Mercado
## 11. Conclusão
## 12. Referências Bibliográficas
