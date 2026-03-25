# DESING PATTERN - OBSERVER (Padrão Comportamental)
## Monitor de Mercado Financeiro 

**Nome:** Marilene Araujo  
**Disciplina:** Desenvolvimento de Sistemas  
**Instrutor:** Frederico Martins Aguiar  
**Unidade:** SENAI - Nova Lima

---
## 1. Introdução

No cenário atual da engenharia de software, a **sincronização de estados** entre componentes distintos representa um desafio crítico, especialmente em sistemas que exigem alta disponibilidade e processamento de dados em tempo real. O desenvolvimento de arquiteturas baseadas em **acoplamento rígido** frequentemente resulta em sistemas inflexíveis, onde alterações em um módulo central desencadeiam efeitos colaterais em toda a aplicação, dificultando a manutenção e a escalabilidade.

Este trabalho propõe a implementação de um **Monitor de Mercado Financeiro**, desenvolvido na linguagem **C#** com o framework **WPF** (*Windows Presentation Foundation*). A solução foca na aplicação prática de padrões de projeto para gerenciar fluxos de dados dinâmicos de maneira eficiente e organizada.

### Objetivos do Projeto

* **Implementação do Padrão Observer:** Demonstrar como a utilização deste padrão comportamental permite que um objeto central (**Sujeito**) notifique automaticamente múltiplos dependentes (**Observadores**) sobre mudanças de estado, eliminando a necessidade de verificações manuais constantes (*polling*).
* **Adoção do Padrão MVVM:** Integrar o padrão *Model-View-ViewModel* para estruturar a aplicação, garantindo que a lógica de negócio e o motor de dados permaneçam isolados da camada de apresentação.
* **Desacoplamento e Escalabilidade:** Estabelecer uma arquitetura onde novos serviços ou interfaces possam ser acoplados ao sistema sem a necessidade de modificar o código-fonte do núcleo de dados, promovendo a independência entre as camadas.
---
## 2. Definição do Padrão

O **Observer** (ou Observador) é um padrão de projeto **comportamental** que estabelece uma relação de dependência do tipo **um-para-muitos** entre objetos. O propósito central é garantir que, sempre que um objeto principal sofrer uma alteração de estado, todos os seus dependentes sejam notificados e atualizados automaticamente de forma desacoplada.

### Funcionamento do Mecanismo

O padrão é estruturado através de dois componentes fundamentais que interagem por meio de abstrações (interfaces):

<p align="center">
  <img src="./Imagens/observer.png" alt="Design Pattern Observer" width="500">
</p>

1. **O Sujeito (Subject / Publisher):** Atua como o detentor do estado de interesse. Ele gerencia uma lista interna de observadores e disponibiliza métodos para que novos componentes possam se "inscrever" (`Subscribe`) ou "cancelar a assinatura" (`Unsubscribe`) dinamicamente em tempo de execução.
2. **O Observador (Observer / Subscriber):** Representa o componente que consome as atualizações. Em vez de monitorar o Sujeito ativamente (eliminando o consumo de recursos por *polling*), ele permanece em estado passivo até ser invocado pelo Sujeito através de um método de atualização.

### Pilares Teóricos do Padrão

* **Acoplamento Fraco (*Loose Coupling*):** O Sujeito não possui conhecimento sobre as implementações concretas dos observadores (seja uma View WPF, um serviço de log ou uma API externa). A interação ocorre estritamente via **Interface**, permitindo que o sistema evolua sem dependências rígidas.
* **Inversão de Controle:** A lógica de comunicação é invertida; em vez de a interface requisitar dados, o motor de dados "empurra" (*push*) a informação para a interface no exato momento da mudança.
* **Comunicação em *Broadcast*:** A notificação é propagada para todos os assinantes simultaneamente. O Sujeito foca exclusivamente na entrega do dado, sem se responsabilizar pelo processamento individual de cada observador.

### Representação Técnica

Para a viabilização deste padrão, define-se um contrato (Interface) que padroniza a comunicação. O método central deste contrato é:

> **`Atualizar(dados)`**: Este método atua como o ponto de entrada da notificação. Ao detectar uma mudança, o Sujeito percorre sua lista de inscritos e dispara este método para cada um deles, transmitindo o novo estado como parâmetro.
---
## 3. Problema que Resolve

A implementação do padrão **Observer** neste projeto visa mitigar três gargalos críticos no desenvolvimento de sistemas orientados a objetos: o **Acoplamento Rígido**, a **Ineficiência de Processamento (Polling)** e a **Violabilidade de Princípios de Design**.

### 3.1. Acoplamento Rígido (Dependência Direta)

Sem a aplicação do padrão, o motor financeiro (lógica de negócio) exigiria uma referência direta à classe da interface gráfica (`MainWindow`) para disparar atualizações.
* **O Problema:** Esta abordagem cria uma dependência cíclica ou rígida, onde o núcleo do sistema precisa conhecer detalhes da camada de apresentação. Caso fosse necessário adicionar um novo serviço de log ou um sistema de alertas via SMS, o código interno do motor precisaria ser modificado, violando a extensibilidade.
* **A Solução:** O Observer permite que o motor interaja estritamente com uma **Interface (`IObservadorAcoes`)**. O motor desconhece a identidade dos ouvintes, sabendo apenas que eles são capazes de processar a notificação recebida.

### 3.2. Polling vs. Push (Otimização de Recursos)

<p align="center">
  <img src="./Imagens/polling.png" alt="Comparativo Polling vs Push" width="50%">
</p>

* **Abordagem Ineficiente (Polling):** A interface gráfica executaria um laço de repetição constante consultando o motor: *"O preço mudou?"*. Na maioria das iterações, a resposta seria negativa, resultando em desperdício de ciclos de CPU e memória.
* **Abordagem Reativa (Push):** Com o Observer, a interface permanece em estado de espera. O motor assume a responsabilidade de "empurrar" (*push*) a informação somente quando ocorre uma alteração real no estado. Esta inversão economiza recursos computacionais e simplifica o fluxo de execução.

### 3.3. Violação do Princípio de Responsabilidade Única (SRP)

* **O Problema:** Sem o padrão, a classe `MotorMercado` acabaria acumulando responsabilidades alheias ao seu propósito, como gerenciar instâncias de UI, formatar strings para exibição ou controlar conexões de persistência.
* **A Solução:** O motor foca exclusivamente em sua regra de negócio: **processar variações de mercado**. A responsabilidade sobre como essa informação será renderizada ou armazenada é delegada aos observadores concretos, mantendo o código modular e limpo.

### 3.4. Sincronização de Múltiplos Interessados

Em sistemas de monitoramento financeiro, é comum que diversos componentes (Gráficos, Grids de Cotação, Alertas de Limite) dependam do mesmo fluxo de dados.
* **O Problema:** Garantir a consistência visual entre múltiplos componentes de forma manual é complexo e propenso a falhas de sincronia.
* **A Solução:** Como todos os componentes se inscrevem no mesmo **Sujeito**, o padrão assegura a **integridade e a simultaneidade da informação** em toda a aplicação no exato instante da notificação.
---
## 4. Estrutura e Arquitetura do Sistema

<p align="center">
  <img src="./Imagens/Arquitetura.png" alt="Diagrama de Classes Observer" width="80%">
</p>

A arquitetura deste projeto foi projetada para garantir a separação total entre a lógica de geração de dados e a camada de apresentação. A estrutura utiliza o padrão **Observer** como o mecanismo de comunicação central, integrando-se ao padrão **MVVM** (*Model-View-ViewModel*) para o ambiente WPF.

### 4.1. Diagrama de Classes

O diagrama abaixo ilustra a hierarquia e as relações de dependência do sistema. Observa-se que o núcleo da aplicação (Motor) interage apenas com abstrações, desconhecendo as implementações concretas das ViewModels.

<p align="center">
  <img src="./Imagens/diagrama%20de%20classe.png" alt="Diagrama de Classes" width="500">
</p>

### 4.2. Descrição dos Componentes e Relacionamentos

Com base na modelagem apresentada, a estrutura divide-se em:

1.  **Sujeito (MotorMercado):** Atua como o provedor de informações. Ele mantém uma `List<IObservadorAcoes>` e utiliza um `Timer` para simular variações de mercado. O método `Inscrever()` permite o registro dinâmico de novos interessados.
2.  **Abstração (IObservadorAcoes):** Interface que define o contrato de comunicação. Qualquer classe que deseje reagir a mudanças no mercado deve implementar o método `Atualizar(Acao acao)`.
3.  **Observador Concreto (MonitorAcoesViewModel):** Implementa a interface de observação. Ao receber a notificação, ela atualiza suas propriedades (`PrecoAtual`, `Historico`) e dispara o evento `OnPropertyChanged`, notificando a **MainWindow** (View) via *Data Binding*.
4.  **Modelo de Dados (Acao):** Objeto de transporte que encapsula as informações de símbolo e preço, servindo como o contrato de dados entre o Sujeito e o Observador.

### 4.3. Dinâmica de Execução e Fluxo de Dados

O funcionamento operacional do sistema segue um ciclo reativo dividido em três etapas fundamentais, conforme ilustrado no diagrama de sequência abaixo:

<p align="center">
  <img src="./Imagens/diagrama%20de%20sequencia.png" alt="Diagrama de Sequência Observer" width="800">
</p>

1. **Registro (Inscrição):** Durante a inicialização, a `MainWindow` instancia a `ViewModel` e o `MotorMercado`. A `MonitorAcoesViewModel` solicita sua inclusão na lista de observadores do motor através do método `Inscrever(vm)`.
2. **Processamento de Estado:** O `MotorMercado`, operando de forma assíncrona via `Timer`, dispara o método `GerarVariacao()`. Este processo cria uma nova instância da classe `Acao` com os dados atualizados de símbolo e preço.
3. **Notificação (Push):** Ao detectar a variação, o Motor percorre sua lista interna de observadores e invoca o método `Atualizar(acao)` da interface `IObservadorAcoes`. A `ViewModel`, ao ser acionada, executa sua lógica interna (atualiza preço, define cor de tendência e adiciona ao histórico) e dispara o `OnPropertyChanged()` para atualizar a View via Data Binding.
---
## 5. Participantes do Padrão

O padrão **Observer** define quatro participantes principais que colaboram para viabilizar o desacoplamento do sistema. Abaixo, detalhamos cada um desses papéis e como eles foram implementados no projeto de **Monitoramento de Mercado Financeiro**:

### 1. Sujeito (Subject / Publisher)
* **Papel:** É o objeto central que detém o estado de interesse (os preços das ações) e gerencia a lista de dependentes.
* **Responsabilidades:** Disponibilizar métodos para anexar (`Subscribe`) e desanexar (`Unsubscribe`) observadores, além de iterar sobre a lista para disparar as notificações de mudança.
* **No Projeto:** Representado pela classe **`MotorMercado`**, que centraliza a lógica de variação dos ativos.

### 2. Interface do Observador (Observer / Subscriber)
* **Papel:** Define o contrato de comunicação técnica entre o Sujeito e seus dependentes.
* **Responsabilidades:** Declarar o método de notificação padronizado (neste caso, `Atualizar`) que o Sujeito invocará para transmitir os dados.
* **No Projeto:** Representada pela interface **`IObservadorAcoes`**, garantindo que qualquer componente (seja UI ou Log) possa ouvir o motor.

### 3. Observador Concreto (Concrete Observer)
* **Papel:** Componente que implementa a interface do observador para reagir aos estímulos do Sujeito.
* **Responsabilidades:** Implementar a lógica de negócio específica ao receber um novo dado, mantendo o estado da interface ou do serviço sincronizado com o motor.
* **No Projeto:** Representado pela classe **`MonitorAcoesViewModel`**, que processa a atualização e notifica a View via *Data Binding*.

### 4. Objeto de Dados / Estado (Concrete State)
* **Papel:** Encapsula a informação que está sendo trafegada entre o Sujeito e os Observadores.
* **Responsabilidades:** Carregar os valores alterados (Símbolo, Preço e Tendência) de forma íntegra e imutável durante o transporte.
* **No Projeto:** Representado pela classe **`Acao`**, que atua como o *Data Transfer Object* (DTO) da notificação.
---
## 6. Justificativa da Escolha do Contexto (Mercado Financeiro)

A seleção do **Mercado Financeiro** como cenário para este projeto fundamenta-se na natureza intrínseca dos dados econômicos, que exigem alta reatividade, integridade e consistência. Abaixo, detalhamos os motivos técnicos que tornam este contexto o "caso de uso ideal" para a aplicação do padrão **Observer**:

### 6.1. Reatividade em Tempo Real (Low Latency)

No mercado de capitais, a informação possui uma janela de utilidade extremamente curta. Um atraso de milissegundos na atualização de uma cotação pode comprometer a tomada de decisão. 
* **O Diferencial:** O padrão Observer viabiliza um sistema **reativo**. Em vez de a interface realizar consultas periódicas ao banco de dados (gerando latência), o motor de dados "empurra" (*push*) a informação no exato instante da volatilidade, garantindo que o usuário visualize o estado mais atual do ativo.

### 6.2. Orquestração de Múltiplas Visualizações

Em um terminal financeiro profissional, um único evento (ex: a variação de preço da PETR4) deve disparar atualizações simultâneas em diversos componentes:
1. **Gráficos de Tendência:** Para análise técnica visual.
2. **Grades de Cotação:** Para monitoramento tabular de preços.
3. **Motores de Alerta:** Para notificações de limites de preço (Stop Loss/Gain).
4. **Logs de Auditoria:** Para registro histórico de transações.

O Observer soluciona essa complexidade de forma elegante: o **Sujeito** emite a notificação uma única vez, e todos os **Observadores** reagem de maneira independente e paralela.

### 6.3. Escalabilidade e o Princípio Aberto/Fechado (OCP)

Sistemas financeiros são modulares e evolutivos. Uma aplicação que hoje monitora preços pode, futuramente, precisar de um módulo de *Machine Learning* para predição ou um robô de investimentos (*Trading Bot*). 
* **A Vantagem:** Com o Observer, novos módulos podem ser acoplados como novos "Assinantes" sem a necessidade de alterar ou recompilar o núcleo do **Motor de Mercado**. Isso respeita o **Princípio Aberto/Fechado** do SOLID, permitindo que o sistema cresça sem comprometer a estabilidade do código existente.

### 6.4. Eficiência de Recursos Computacionais

A economia de processamento é vital em aplicações Desktop desenvolvidas em **C#/WPF**.
* **Modelo Pull (Ineficiente):** Se 10 janelas distintas fizessem requisições constantes ao motor, haveria um tráfego de dados redundante e alto consumo de CPU.
* **Modelo Push (Eficiente):** No fluxo do Observer, o processamento só é disparado quando há uma mudança real no estado do dado. Isso otimiza o uso de memória e threads, permitindo que a aplicação permaneça fluida mesmo com múltiplos ativos sendo monitorados.
---
### 7.1. Camada de Modelo (Model)

A classe **`Acao`** atua como o Objeto de Transferência de Dados (**DTO**). Sua função é transportar a informação do motor para os observadores de forma íntegra e simplificada.

* **Implementação Técnica:** Diferente de estruturas imutáveis, a classe utiliza propriedades públicas com *getters* e *setters* automáticos e um construtor padrão. Esta abordagem facilita a manipulação dos dados e a integração direta com frameworks de serialização ou persistência.
* **Atributos:**
    * `Simbolo`: Um identificador do tipo `string` (ex: "PETR4") que diferencia o ativo.
    * `Preco`: Um valor do tipo `double` que representa a cotação atualizada.
* **Inicialização:** A classe garante a integridade inicial ao definir o `Simbolo` como uma string vazia (`string.Empty`), evitando exceções de referência nula (*NullReferenceException*) durante o ciclo de vida do objeto.

### 7.2. O Sujeito Concreto (Motor do Mercado)

A classe **`MotorMercado`** detém o estado do sistema e atua como a fonte central de notificações.
* **Gerenciamento de Assinantes:** Mantém uma `List<IObservadorAcoes>` privada. O motor interage apenas com a abstração, desconhecendo as implementações concretas (ViewModels ou Loggers) presentes na lista.
* **Lógica de Notificação:** O método `Notificar()` percorre a coleção de inscritos e invoca o método `Atualizar()` de cada um, propagando a instância de `Acao`.
* **Gatilho de Eventos:** Utiliza um `System.Timers.Timer` para simular a volatilidade financeira. A cada intervalo (*tick*), o motor gera uma nova variação e dispara automaticamente o fluxo de notificação.

### 7.3. A Interface do Observador (Abstração)

A interface **`IObservadorAcoes`** é o componente que viabiliza o **Baixo Acoplamento** e a **Inversão de Dependência**.
* **Definição do Contrato:** Estabelece o método padrão `void Atualizar(Acao acao)`, obrigatório para qualquer componente que deseje monitorar o motor.
* **Extensibilidade:** Permite que novos observadores sejam acoplados ao sistema sem a necessidade de modificar o código interno do `MotorMercado`.

### 7.4. O Observador Concreto (ViewModel)

A classe **`MonitorAcoesViewModel`** é responsável por processar os estímulos do motor e preparar os dados para a camada de apresentação (**View**).
* **Auto-Inscrição:** No construtor, a ViewModel recebe a instância do motor e realiza o registro de interesse via `_motor.Inscrever(this)`.
* **Sincronização de Threads (Dispatcher):** Como as notificações do `Timer` ocorrem em threads secundárias, a ViewModel utiliza o `Dispatcher` para garantir que a atualização da interface gráfica ocorra na *UI Thread*.
* **Reatividade via Data Binding:** Após o processamento, a classe dispara o evento `OnPropertyChanged`, notificando o motor de vínculo do WPF para renderizar o novo valor na tela.
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
## 9. Análise Crítica e Resultados

A aplicação do padrão **Observer** no Monitor de Mercado Financeiro permite uma avaliação profunda sobre o impacto da arquitetura no ciclo de vida do software. Abaixo, detalhamos os pontos observados durante o desenvolvimento e os resultados obtidos:

### 9.1. Matriz Comparativa: Arquitetura Tradicional vs. Observer

A tabela abaixo resume as principais diferenças observadas entre uma implementação de acoplamento direto e a solução proposta com o padrão:

| Característica | Sem o Padrão Observer | Com o Padrão Observer |
| :--- | :--- | :--- |
| **Acoplamento** | **Rígido:** O Motor de Mercado exige referência direta para cada View ou ViewModel. | **Fraco (Loose):** O Motor interage apenas com a abstração `IObservadorAcoes`. |
| **Escalabilidade** | **Complexa:** Adicionar novos componentes exige alteração e recompilação do núcleo. | **Simples:** Basta criar uma classe que implemente a interface e realizar a inscrição. |
| **Manutenção** | **Arriscada:** Mudanças na UI podem impactar a estabilidade da lógica de negócio. | **Segura:** As camadas são independentes; o motor é isolado das alterações de design. |
| **Eficiência** | **Pull (Busca):** A UI consome recursos consultando o motor constantemente. | **Push (Envio):** O motor despacha o dado somente no instante da alteração real. |

### 9.2. Vantagens Observadas na Prática

* **Respeito ao OCP (Open-Closed Principle):** Durante a codificação, confirmou-se a facilidade de acoplar novos "assinantes" (como um serviço de log ou alertas sonoros) sem violar ou modificar o código-fonte estável do `MotorMercado`.
* **Reatividade e Performance:** A percepção de atualização na interface **WPF** é instantânea. O modelo de notificação via *Push* provou-se ideal para o contexto financeiro, onde a latência mínima é um requisito de negócio.
* **Separation of Concerns (SoC):** A clara distinção entre a regra de negócio (geração de preços) e a lógica de apresentação (formatação de cores e histórico) resultou em um código mais limpo e legível.

### 9.3. Desafios e Limitações Identificadas

Embora robusto, o padrão introduz considerações importantes que devem ser gerenciadas:

* **Curva de Aprendizado e Sobrecarga Inicial:** Para sistemas triviais, o uso de interfaces e gerenciamento de listas pode parecer excessivo (*over-engineering*), exigindo maior esforço de design comparado a uma solução monolítica.
* **Gestão de Memória (*Memory Leaks*):** No ambiente .NET, se um observador for instanciado e não realizar o `Unsubscribe` ao ser descartado, o Sujeito manterá uma referência viva na memória, impedindo a atuação do *Garbage Collector*.
* **Imprevisibilidade da Ordem de Notificação:** O padrão, por definição, não estabelece uma sequência garantida para o despacho das mensagens. Se a lógica da aplicação exigir que o "Observador A" processe a informação antes do "Observador B", mecanismos adicionais de orquestração seriam necessários.

---
  
## 10. Exemplos Reais de Uso no Mercado

O padrão **Observer** é um dos mais utilizados na indústria de software, servindo de base para arquiteturas reativas e sistemas distribuídos. Abaixo, listamos exemplos clássicos de sua aplicação no mercado real:

### 10.1. Notificações Push (Mobile)
Este é o exemplo mais comum no cotidiano. Quando um servidor de notícias (Sujeito) publica uma nova matéria, ele não sabe quais usuários estão com o celular ligado. Ele simplesmente dispara uma notificação para todos os aparelhos inscritos (Observadores). Cada aplicativo reage à sua maneira: exibindo um banner, vibrando o celular ou emitindo um som.

### 10.2. Dashboards Financeiros e de BI
Plataformas como **Bloomberg Terminal**, **ProfitChart** ou painéis de **Business Intelligence (Power BI)** utilizam o Observer para atualizar gráficos em tempo real. Quando o banco de dados recebe uma nova transação, todos os widgets (velas de gráfico, tabelas de volume, indicadores de média móvel) são notificados simultaneamente para redesenhar a interface.

### 10.3. Frameworks de Front-end (React, Vue, Angular)
A "mágica" desses frameworks modernos baseia-se no conceito de **Reatividade**. Quando o estado de uma variável muda, todos os componentes da página que dependem dessa variável (os Observadores) são "avisados" para se renderizarem novamente. O mecanismo de *Data Binding* do próprio **WPF** (utilizado neste projeto) é uma implementação profunda do padrão Observer.

### 10.4. Redes Sociais
No **Instagram** ou **X (Twitter)**, quando você segue alguém, você está se tornando um "Assinante" (Subscriber) daquele perfil. Assim que o perfil posta um novo conteúdo (Evento), o sistema notifica todos os seguidores (Observadores) para que o feed de cada um seja atualizado com a nova publicação.

### 10.5. Sistemas de Monitoramento (DevOps)
Ferramentas como **Zabbix** ou **Grafana** monitoram a saúde de servidores. Se o uso de CPU de um servidor ultrapassa 90%, o motor de monitoramento (Sujeito) dispara alertas para diversos observadores: um canal no **Slack**, um e-mail para o administrador e um log de segurança.

---
## 11. Conclusão

A realização deste projeto permitiu uma compreensão profunda de como padrões de projeto comportamentais, especificamente o **Observer**, são fundamentais para a criação de sistemas reativos e de alta performance. Através da implementação de um **Monitor de Mercado Financeiro**, foi possível observar na prática a transição de um sistema acoplado para uma arquitetura modular e escalável.

### Principais Aprendizados:
* **Desacoplamento Efetivo:** A utilização de interfaces (`IObservadorAcoes`) provou que o motor de dados não precisa conhecer os detalhes da interface gráfica para funcionar, o que facilita a manutenção a longo prazo.
* **Poder da Reatividade:** No contexto de WPF e MVVM, o padrão Observer, aliado ao `Dispatcher`, demonstrou ser a solução ideal para lidar com atualizações em tempo real vindas de threads secundárias sem comprometer a experiência do usuário.
* **Qualidade de Software:** A aplicação dos princípios **SOLID** durante o desenvolvimento não apenas organizou o código, mas também preparou a aplicação para futuras expansões, como a adição de novos tipos de análise técnica ou bots de negociação.

Em suma, o padrão Observer não é apenas uma técnica de codificação, mas uma mentalidade de design que prioriza a flexibilidade e a eficiência. Este projeto consolida o conhecimento necessário para enfrentar desafios reais de engenharia de software, onde a distribuição de informação precisa e oportuna é o diferencial entre um sistema funcional e uma aplicação de sucesso.

---
## 12. Referências Bibliográficas

As fontes abaixo serviram de base teórica e técnica para a fundamentação e implementação deste projeto:

### Livros e Literatura Base
* **GAMMA, Erich; HELM, Richard; JOHNSON, Ralph; VLISSIDES, John.** *Design Patterns: Elements of Reusable Object-Oriented Software*. 1. ed. Boston: Addison-Wesley, 1994. (O livro clássico do "GoF" que consolidou o padrão Observer).
* **FREEMAN, Eric; ROBSON, Elisabeth.** *Use a Cabeça! Padrões de Projetos*. 2. ed. Rio de Janeiro: Alta Books, 2023. (Abordagem prática e visual sobre a comunicação entre objetos).
* **MARTIN, Robert C.** *Clean Architecture: A Craftsman's Guide to Software Structure and Design*. 1. ed. Prentice Hall, 2017. (Fundamentos sobre desacoplamento e limites arquiteturais).

### Documentação Técnica e Frameworks
* **MICROSOFT LEARN.** *Observer Design Pattern in .NET*. Disponível em: [https://learn.microsoft.com/en-us/dotnet/standard/design-patterns/observer-design-pattern](https://learn.microsoft.com/en-us/dotnet/standard/design-patterns/observer-design-pattern). Acesso em: 21 mar. 2026.
* **MICROSOFT LEARN.** *WPF Data Binding Overview*. Disponível em: [https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/data-binding-overview](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/data-binding-overview). Acesso em: 21 mar. 2026.
* **REFACTORING GURU.** *Observer Pattern*. Disponível em: [https://refactoring.guru/design-patterns/observer](https://refactoring.guru/design-patterns/observer). Acesso em: 21 mar. 2026.

### Ferramentas Utilizadas
* **IDE:** JetBrains Rider / Microsoft Visual Studio 2022.
* **Linguagem:** C# 12 (.NET 8/9).
* **Interface:** Windows Presentation Foundation (WPF).
