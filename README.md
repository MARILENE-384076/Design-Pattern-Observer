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

<p style="text-align: center;">
  <img src="./Imagens/observer.png" alt="Design Pattern Observer" style="width: 50%; display: inline-block;">
</p>

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

A implementação do padrão **Observer** neste projeto visa resolver três gargalos críticos no desenvolvimento de sistemas orientados a objetos: o **Acoplamento Rígido**, a **Ineficiência de Processamento (Polling)** e a **Inconsistência de Dados**.

### 3.1. Acoplamento Rígido (Dependência Direta)
Sem o padrão Observer, para que o motor financeiro atualizasse a interface, ele precisaria ter uma referência direta à classe da View (`MainWindow`).
* **O Problema:** Isso cria uma dependência onde o "coração" do sistema (lógica de mercado) precisa conhecer detalhes da "casca" (interface). Se desejarmos adicionar um novo serviço de Log ou um alerta via SMS, seríamos obrigados a alterar o código interno do motor.
* **A Solução:** O Observer permite que o motor interaja apenas com uma **Interface (`IObservadorAcoes`)**. O motor não sabe *quem* está ouvindo, apenas que eles sabem processar a atualização.

### 3.2. Polling vs. Push (Desperdício de Recursos)

<p style="text-align: center;">
  <img src="./Imagens/polling.png" alt="Ineficiencia de Processamento Polling" style="width: 50%; display: inline-block;">
</p>

* **Abordagem Ineficiente (Polling):** A interface gráfica precisaria de um loop constante perguntando ao motor: *"O preço mudou?"*. Na maioria das verificações, a resposta seria negativa, resultando em desperdício de CPU e memória.
* **Abordagem Reativa (Push):** Com o Observer, a interface fica em estado de espera (dormente). O motor assume a responsabilidade de "empurrar" (*push*) a informação apenas quando ocorre uma mudança real. Isso economiza recursos computacionais e simplifica o fluxo de execução.

### 3.3. Violação do Princípio de Responsabilidade Única (SRP)
* **O Problema:** Sem o padrão, o `MotorMercado` acabaria acumulando responsabilidades que não são dele, como gerenciar instâncias de UI, formatar strings para exibição ou gerenciar conexões de log.
* **A Solução:** O motor foca exclusivamente em sua regra de negócio: **gerar e processar variações de mercado**. A responsabilidade de como essa informação será exibida ou armazenada fica a cargo de cada observador concreto, mantendo o código limpo e modular.

### 3.4. Sincronização de Múltiplos Interessados
Em sistemas de monitoramento, é comum ter vários componentes (Gráficos, Grids de Preço, Alertas de Limite) dependendo do mesmo dado.
* **O Problema:** Garantir que todos esses componentes mostrem o mesmo valor simultaneamente é complexo sem um despacho centralizado.
* **A Solução:** Como todos os componentes se inscrevem no mesmo "Sujeito", o padrão garante a **integridade e sincronia da informação** em toda a aplicação no exato momento da notificação.
---
## 4. Estrutura e Diagrama de Classes

A arquitetura deste projeto foi desenhada para separar completamente a lógica de geração de dados da lógica de exibição. Abaixo, detalhamos como o padrão **Observer** organiza as classes.

<p style="text-align: center;">
  <img src="./Imagens/Arquitetura.png" alt="Arquitetura Observer" style="width: 50%; display: inline-block;">
</p>

### 4.1. O Diagrama de Classes
O diagrama abaixo representa a relação de dependência entre as classes do sistema. Note que o "Motor" (Subject) não conhece as "ViewModels" (Observers) diretamente, mas sim a interface que elas implementam.
#INSERIR DIAGRAMA AQUI!!!

### 4.2. Fluxo de Relacionamento
1.  **Associação de Composição:** O `MotorMercado` possui uma `List<IObservadorAcoes>`. Isso permite que ele armazene múltiplos interessados sem saber de que tipo eles são (se são telas, logs ou serviços de e-mail).
2.  **Realização de Interface:** A classe `MonitorAcoesViewModel` realiza (implementa) a interface `IObservadorAcoes`. Isso garante que ela possua o método `Atualizar()`, que será chamado pelo motor.
3.  **Dependência de Dados:** Tanto o Sujeito quanto o Observador dependem da classe de modelo `Acao`, que serve como o "pacote" de dados trafegado durante a notificação.

### 4.3. Dinâmica de Execução (Diagrama de Sequência)
O funcionamento ocorre em três etapas principais:
* **Inscrição:** Ao iniciar a aplicação, a `ViewModel` chama o método `Motor.Inscrever(this)`.
* **Mudança de Estado:** O `MotorMercado` altera o preço de uma ação (via Timer).
* **Notificação:** O Motor percorre sua lista interna e executa o método `Atualizar(acao)` de cada observador inscrito.
  # INSERIR DIAGRAMA AQUI!
---
## 5. Participantes do Padrão

O padrão Observer define quatro participantes principais que colaboram para realizar o desacoplamento do sistema. Abaixo, detalhamos cada um deles e como eles se traduzem no projeto de **Monitoramento de Ações**:

### 1. Sujeito (Subject / Publisher)
* **Papel:** É o objeto que detém o estado de interesse (os preços das ações) e mantém a lista de observadores.
* **Responsabilidades:** Fornecer uma interface para anexar (`Subscribe`) e desanexar (`Unsubscribe`) observadores, além de percorrer a lista para enviar notificações.
* **No Projeto:** É a classe `MotorMercado`.

### 2. Interface do Observador (Observer / Subscriber)
* **Papel:** Define o contrato de atualização para os objetos que devem ser notificados pelo Sujeito.
* **Responsabilidades:** Declarar o método de notificação (geralmente chamado de `Update` ou `Atualizar`) que o Sujeito utilizará para passar as informações.
* **No Projeto:** É a interface `IObservadorAcoes`.

### 3. Observador Concreto (Concrete Observer)
* **Papel:** Mantém uma referência ao Sujeito e implementa a interface do Observador para manter seu estado sincronizado com o do Sujeito.
* **Responsabilidades:** Implementar a lógica de reação ao receber um dado (ex: atualizar um gráfico ou uma propriedade na tela).
* **No Projeto:** É a classe `MonitorAcoesViewModel`.

### 4. Objeto de Dados / Estado (Concrete State)
* **Papel:** Representa a informação que está sendo transmitida do Sujeito para os Observadores.
* **Responsabilidades:** Carregar os valores alterados (Símbolo da ação, Preço, Variação) de forma íntegra.
* **No Projeto:** É a classe (ou record) `Acao`.
  
---
## 6. Justificativa da Escolha do Contexto (Mercado Financeiro)

A escolha do **Mercado Financeiro** como cenário para este projeto não foi arbitrária; ela baseia-se na natureza intrínseca dos dados financeiros, que exigem alta reatividade e integridade. Abaixo, detalhamos os motivos técnicos que tornam este contexto o "caso de uso perfeito" para o padrão Observer:

### 6.1. Natureza Crítica do Tempo (Real-Time)
No mercado de ações, a informação perde valor rapidamente. Um atraso de poucos segundos na atualização de um preço pode resultar em decisões erradas. O padrão Observer permite que o sistema seja **reativo**: em vez de a interface "perguntar" ao banco de dados se o preço mudou (o que seria lento e custoso), o motor de dados "empurra" a informação no exato milissegundo em que ela é alterada.

### 6.2. Necessidade de Múltiplas Visualizações
Em um terminal financeiro real, um único dado (ex: o preço da PETR4) precisa atualizar simultaneamente:
1. Um **Gráfico de Candlestick** (visual).
2. Uma **Grade de Cotações** (tabela).
3. Um **Sistema de Alertas** (notificação de preço alto/baixo).
4. Um **Log de Transações** (histórico).

O Observer resolve isso perfeitamente, pois o motor (Sujeito) apenas emite o dado uma vez, e todos esses componentes (Observadores) reagem de forma independente.

### 6.3. Escalabilidade e Desacoplamento
Sistemas financeiros são modulares. Hoje o sistema tem uma grade de preços; amanhã pode precisar de um robô de investimentos (*Trading Bot*). Com o Observer, podemos adicionar esse robô como um novo "Assinante" sem precisar alterar uma única linha de código do Motor de Mercado. Isso respeita o **Princípio Aberto/Fechado (OCD)** do SOLID.

### 6.4. Eficiência de Recursos (Push vs. Pull)
Se tivéssemos 10 janelas abertas e cada uma fizesse uma requisição por segundo ao motor (modelo *Pull*), teríamos um tráfego desnecessário. No modelo do Observer (*Push*), o tráfego só acontece quando há uma mudança real no dado, economizando processamento e memória, o que é vital para aplicações desktop em C#/WPF.

---

## 7. Explicação da Implementação no Projeto

Nesta seção, detalhamos como o código desenvolvido no **Rider** traduz os conceitos teóricos do padrão Observer para uma aplicação funcional em **C#** e **WPF**.

### 7.1. Camada Model
A classe `Acao` atua como o **Objeto de Dados (DTO)**. Sua função é puramente carregar a informação que será transportada do motor para os observadores.
* **Implementação:** Utilizamos o tipo `record` do C# para garantir a **imutabilidade**. Isso evita que um observador altere acidentalmente o dado antes que outros observadores o recebam.
* **Propriedades:** Contém `Simbolo` (identificador da empresa) e `Preco` (valor atualizado).

### 7.2. O Sujeito (Motor do Mercado)
A classe `MotorMercado` é a detentora do estado e a fonte das notificações.
* **Gerenciamento de Assinantes:** Possui uma `List<IObservadorAcoes>` privada. O motor não sabe quais classes estão na lista, apenas que todas seguem o contrato da interface.
* **Métodos Principais:** `Inscrever()` adiciona novos interessados à lista; `Notificar()` percorre a lista e dispara o evento.
* **O Gatilho:** Um `System.Timers.Timer` simula a volatilidade do mercado. A cada "tick" do cronômetro, um novo preço é gerado e o método `Notificar()` é acionado.

### 7.3. A Interface do Observador
A interface `IObservadorAcoes` é o componente que viabiliza o **Baixo Acoplamento**.
* **O Contrato:** Define o método `void Atualizar(Acao acao)`. 
* **Inversão de Dependência:** O motor depende desta abstração e não de classes concretas. Isso permite que amanhã possamos adicionar um "Robô de Investimento" ou um "Gerador de Logs" como observadores sem mudar uma única linha de código no motor.

### 7.4. O Observador Concreto (ViewModel)
A `MonitorAcoesViewModel` é a classe que reage aos dados e os prepara para a visão (View).
* **Ligação com o Sujeito:** No construtor, a ViewModel recebe a instância do motor e executa o registro: `_motor.Inscrever(this)`.
* **Atualização da UI:** Ao receber a notificação no método `Atualizar`, a ViewModel utiliza o `Dispatcher` do WPF. Isso é fundamental, pois as notificações vêm de uma thread secundária (Timer) e a interface gráfica só pode ser manipulada pela thread principal.
* **Data Binding:** Após processar o dado, ela dispara o `OnPropertyChanged`, fazendo com que o valor apareça instantaneamente na tela do usuário.
  
---
## 8. Integração MVVM e Boas Práticas

A implementação do padrão **Observer** ganha escala e robustez ao ser integrada à arquitetura **MVVM** (*Model-View-ViewModel*), padrão de ouro para aplicações desktop em C# e WPF. Abaixo, detalhamos como essa integração ocorre e quais boas práticas foram aplicadas:

### 8.1. Sincronização de Threads com Dispatcher
Como o `MotorMercado` (Sujeito) opera em uma thread secundária (Timer) para evitar o travamento da interface (*UI Freeze*), surge um desafio técnico: o WPF não permite que threads externas modifiquem elementos visuais.
* **Solução:** No método `Atualizar` da ViewModel, utilizamos o **`Dispatcher.Invoke`**. Ele atua como um "despachante" que envia a atualização do dado para a fila da thread principal (UI Thread), garantindo estabilidade e fluidez na atualização dos preços em tempo real.

### 8.2. Data Binding e Notificação de Propriedades
A ViewModel, ao atuar como um **Observador Concreto**, recebe o objeto `Acao` e atualiza suas propriedades locais. 
* **Boas Práticas:** Utilizamos a interface `INotifyPropertyChanged`. Assim que o Observer "avisa" a ViewModel que o preço mudou, a ViewModel "avisa" o XAML (View) via Binding, mantendo a tela sempre sincronizada sem código de manipulação direta de componentes (Code-behind limpo).

### 8.3. Princípios SOLID Aplicados
* **Single Responsibility Principle (SRP):** O Motor de Mercado apenas gera dados; ele não sabe como eles são exibidos. A ViewModel apenas formata os dados para a tela; ela não sabe como os preços são calculados.
* **Open/Closed Principle (OCP):** O sistema está aberto para extensão (podemos adicionar novos gráficos ou logs como novos observadores) e fechado para modificação (não precisamos mexer no código do `MotorMercado` para adicionar essas novas funcionalidades).
* **Dependency Inversion Principle (DIP):** O Sujeito depende da interface `IObservadorAcoes` e não de classes concretas, o que facilita a criação de testes unitários e a manutenção do código.

### 8.4. Gerenciamento de Memória (Unsubscribe)
Uma boa prática implementada é garantir o descarte correto. Quando uma janela de monitoramento é fechada, a ViewModel invoca o método de **cancelamento de assinatura** no Sujeito. Isso evita o vazamento de memória (*Memory Leak*), impedindo que o motor continue tentando notificar um objeto que não deveria mais existir na memória.

---
## 9. Análise Crítica
### 9.1. Comparação: Com Padrão vs. Sem Padrão
### 9.2. Vantagens Observadas
### 9.3. Desvantagens e Limitações
## 10. Exemplos Reais de Uso no Mercado
## 11. Conclusão
## 12. Referências Bibliográficas
