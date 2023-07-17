# WellToDo

WellToDo é uma aplicação de gerenciamento de tarefas que permite aos usuários criar, visualizar, atualizar e excluir tarefas em grupos específicos.


## Definição

Sua descrição foi retirada de um [desafio](desafio.md), porém repensada para ser mais complexa, para mostrar minhas habilidades, ao mesmo tempo que fiel a definição base.

A aplicação continua centrada em Tarefas e Grupos, porém agora com mais atributos. A seguir informarei mais detalhes sobre cada entidade e seus atributos.

- **Grupo (Group)**
    - Possui um identificador (Id).
    - Possui título (Title).
    - Possui uma cor (Color) para auxílio visual.
    - Possui um relacionamento one to many com a entidade Tarefas.
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
    - Possui um relacionamento many to one com a entidade grupo.

Com os modelos definidos, defini também algumas metas de funcionamento geral:
- A ordem de exibição **SEMPRE** deve ser por data
- Deve ter um sistema de filtros para encontrar as tarefas utilizando diversas atributos em conjunto:
	- Frase de Pesquisa (será usada para procurar a tarefa com o título e descrição que combinam)
	- Data
	- Prioridade
	- Completa/Pendente
	- Sinalizador
	- Grupo
- Se a tarefa tiver uma URL, deve apresentar um botão de fácil acesso a URL informada.
- Deve ter um feedback visual na tarefa se o horário dela tiver passado estiver incompleta.
- Deve ser possível editar todos os atributos de tarefas e grupos.
- Deve ser possível marcar uma tarefa como concluída/pendente.

## Backend

Nesse tópico vou informar detalhes técnicos sobre a construção do Banco de Dados, da API e demais configurações.

### Tecnologias

- **Linguagem / Framework**: C# com ASP.NET Core
- **Bando de dados**: SQL Server da Microsoft
- **IDE**: Visual Studio Code
- **Hospedagem**: https://welltodo.bsite.net/ (hoster: https://freeasphosting.net/)
- **Design Pattern**: MVC
- **Testes Unitários**: xUnit e Moq
- **Documentação**: Swagger

### Passos para Execução

Você pode executar da forma padrão que é aplicando os comandos `dotnet restore` e `dotnet run` para carregar as frameworks necessárias e executar o projeto, ou você também pode [acessar o site](https://welltodo.bsite.net/) com o código da branch main hospedado.

A documentação da API pode ser encontrada tanto executando o projeto localmente quanto acessando o Swagger no site. Apesar de não ideal, decidi deixá-lo disponível devido ao contexto de teste da aplicação.

### Testes

Foram definidos diversos testes para cada uma das APIs e todos eles estão em um projeto separado que referencia o projeto principal. Sendo assim, para realizar os testes você precisa acessar o projeto [WellToDoTests](WellToDoTests/WellToDoTests.csproj) e executar o comando `dotnet test`.


## Frontend

Nesse tópico vou informar detalhes técnicos sobre a construção dos componentes do Angular, comunicação com a API, etc.

### Tecnologias

- **Linguagem / Framework**: TypeScript, HTML e CSS com Angular
- **IDE**: Visual Studio Code
- **Hospedagem**: https://welltodo.bsite.net/ (hoster: https://freeasphosting.net/)
- **Design Pattern**: MVC
- **Frameworks**:
    -ngx-translate: Biblioteca para localização e internacionalização da aplicação em Português (Brasil) e Inglês

### Passos para Execução

Você pode executar da forma padrão que é aplicando o comando `npm run start`, ou você também pode [acessar o site](https://welltodo.bsite.net/) com o código da branch main hospedado.

### Organização

Toda a aplicação se baseia em 2 Modelos (Task e Group), os seus respectivos serviços de comunicação com a API e outros 3 Componentes para a visualização das informações:
- `task-list`: Componente responsável por exibir a lista de tarefas e suas informações. Permite a criação, atualização e exclusão de tarefas. Possui recursos de paginação para lidar com grandes volumes de dados.
- `menu`: Componente responsável as opções de filtragem das tarefas, como filtro textual, por prioridade, por sinalizador, por status, por data e por grupo. Além disso, permite a visualização da lista de grupos, sendo ele o seu sub-componente.
- `group-list`: Componente responsável por exibir a lista de grupos e suas informações. Permite a criação, atualização e exclusão de grupos e auxilia na filtragem de tarefas a partir da seleção dos grupos.

## Decisões e Detalhes Adicionais:

- Ele foi testado e pensado ao redor do browser Google Chrome. Pode funcionar em outros, porém devido a diferenças em métodos base do CSS e Javascript é possível que tenha diferenças de comportamento. Qualquer problema encontrado peço que entre em contato.
- O design da aplicação é responsivo, permitindo uma experiência de usuário consistente em diferentes dispositivos.
- As requisições para as APIs do backend são realizadas assincronamente para melhorar o desempenho e a responsividade da aplicaçãop.
- Foram utilizados padrões de projeto como o MVC (Model-View-Controller) para separação de responsabilidades e o padrão de projeto Singleton para os serviços.
- Foi implementada a funcionalidade de paginação na listagem de tarefas para melhorar a performance e a usabilidade da aplicação.
- A aplicação possui recursos de validação de dados para garantir a integridade dos dados inseridos pelos usuários.
- A internacionalização foi incorporada para permitir a tradução da aplicação para diferentes idiomas.
