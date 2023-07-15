# WellToDo

WellToDo é um aplicativo de gerenciamento de tarefas que permite aos usuários criar, visualizar, atualizar e excluir tarefas em grupos específicos.


## Definição

Sua descrição foi retirada de um [desafio](desafio.md), porém repensada para ser mais complexa, para mostrar minhas habilidades, ao mesmo tempo que fiel a definição base.

A aplicação continua centrada em Tarefas e Grupos, porém agora com mais atributos além da adição de suporte a diferentes Usuários. A seguir informarei mais detalhes sobre cada entidade e seus atributos.

- **Grupo (Group)**
    - Possui um identificador (Id).
    - Possui título (Title).
    - Possui uma cor (Color) para auxílio visual.
    - Possui um relacionamento one to many com a entidade Tarefas.
    - Possui um relacionamento many to one com um usuário.
    - Se excluído, todas as suas tarefas serão excluidas.
- **Tarefa (Task)**
    - Possui um identificador (Id).
    - Possui título (Title) e descrição (Description) como strings longas.
    - Possui data (Date) obrigatoriamente para informar quando a tarefa deve ser feita.
    - Todas as tarefas são ordenadas por data.
    - Possui prioridade (Priority) que pode ser nulo ou ter os niveis baixo (Low), médio (Medium) e alto (High).
    - Possui URL (Url), retirando seu possível uso dentro da descrição e possibilitanto um botão de redirecionamento.
    - Possui status (isChecked) para representar sua completude.
    - Possui sinalizador (IsFlagged) para representar que a tarefa precisa de atenção.
    - Possui um relacionamento many to one com um usuário.
    - Possui um relacionamento many to one com a entidade grupo.
    - Toda tarefa possui um grupo.
    - É possível mover tarefas entre grupos.
- **Usuário (User)**
    - Possui um identificador (Id).
    - Possui nome (Firstname) e sobrenome (Lastname).
    - Possui um apelido (Username) único usado no SignIn.
    - Não possui senha pois sua gestão aumentaria consideravelmente a complexidade do projeto por um retorno mínimo.

Com os modelos definidos, defini também algumas metas de funcionamento geral:
- A ordem de exibição **SEMPRE** deve ser por data
- Deve ter um sistema de filtros para encontrar as tarefas utilizando diversas atributos em conjunto:
	- Frase de Pesquisa (será usada para procurar a tarefa com o título e descrição que combinam)
	- Data
	- Prioridade
	- Completa/Pendente
	- Sinalizador
	- Grupo
- Uma tarefa concluída não deve ser exibida na lista.
- As tarefas concluídas podem ser visualizadas através de alguma opção na tela.
- Se a tarefa tiver uma URL, deve apresentar um botão de fácil acesso a URL informada.
- Deve ter um feedback visual na tarefa se o horário dela tiver passado estiver incompleta.
- Deve ser possível editar todos os atributos de tarefas e grupos.
- Deve ser possível marcar uma tarefa como concluída/pendente.
- A tela inicial deve ser de SignIn/SignUp.
- O usuário só precisa informar seu apelido para entrar.
- No primeiro acesso, o usuário deve visualizar um tutorial.
- Esse tutorial deve poder ser acessado em um menu.


## Backend

Nesse tópico vou informar detalhes técnicos sobre a construção do Banco de Dados, da API e demais configurações.

### Tecnologias

- **Linguagem / Framework**: C# com ASP.NET Core
- **Bando de dados**: SQL Server da Microsoft
- **IDE**: Visual Studio Code
- **Hospedagem**: https://freeasphosting.net/
- **Design Pattern**: MVC
- **Testes Unitários**: xUnit e Moq
- **Documentação**: Swagger

### Passos para Execução

Você pode executar da forma padrão que é aplicando os comentos `dotnet restore` e `dotnet run` para carregar as frameworks necessárias e executar o projeto, ou você também pode acessar o site com o código da branch main hospedado.

A documentação da API pode ser encontrada tanto executando o projeto localmente quanto acessando o Swagger no site. Apesar de não ideal, decidi deixá-lo disponível devido ao contexto de teste da aplicação.

### Testes

Foram definidos diversos testes para cada uma das APIs e todos eles estão em um projeto separado que referencia o projeto principal. Sendo assim, para realizar os testes você precisa acessar o projeto [WellToDoTests](WellToDoTests/WellToDoTests.csproj) e executar o comando `dotnet test`.


## Frontend

*Em desenvolvimento*