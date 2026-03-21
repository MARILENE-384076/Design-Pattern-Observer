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
