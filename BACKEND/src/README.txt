# Mini-Projeto - Sistema de Gestão de Livros

## Tecnologias Utilizadas

- **Backend**:  
  - **SQLServer**: Banco de dados para armazenar as informações dos livros e autores.
  - **.NET + Entity Framework (EF)**: Para o desenvolvimento da API que fornecerá os dados e funcionalidades ao Frontend.
  - Arquitetura baseada em **Controllers**, **Services** e **Repositories**. Evitar a utilização de um único arquivo para todo o código. Explore o tema em vídeos, tutoriais e documentação.

- **Frontend**:  
  - **React** + **TypeScript**: Para exibir os dados em tabelas dinâmicas.
  
- **Repositório GIT**:  
  - Utilização de GIT para versionamento do código. Ao criar uma nova funcionalidade, crie uma branch específica para ela.
  - Consulte a documentação do GIT e explore vídeos sobre boas práticas de commits e branch management.

- **Documentação e Apresentação**:  
  - Criar vídeos apresentando o trabalho desenvolvido. A documentação de cada etapa deve ser clara e bem estruturada.

---

## Tópicos a Explorar / Recapitular Memória

1. **Modelo de Arquitetura**:  
   - **Controller – Service – Repository**: Exploração e implementação de uma arquitetura bem definida para separar as responsabilidades do código.

2. **Métodos/Funções**:  
   - Desenvolver funções utilizando **HTTP Codes** e **MessagingHelper** para a comunicação com a API.

3. **DTOs (Data Transfer Objects)**:  
   - O que são e como ajudam no desenvolvimento de software, facilitando a troca de dados entre camadas.

4. **Migrations**:  
   - O que são **Migrations** no Entity Framework e quais os problemas mais comuns que podem surgir durante o processo de migração de banco de dados.

5. **Debugger**:  
   - Ferramenta fundamental para entender e depurar o código. É importante aprender como utilizar o debugger para detectar e resolver erros, mesmo quando o código parece 100% funcional.

---

## Requisitos Funcionais

### Etapa 1: Gestão de Livros

1. **Livro**:  
   - **Campos**: ISBN, Nome, Autor, Preço.
   
2. A aplicação deve permitir as seguintes funcionalidades:
   - **Consultar/Pesquisar um livro**:
     - Pesquisa obrigatória por **ISBN**.
     - Outros campos adicionais sugeridos pelo desenvolvedor (valor agregado ao projeto).
     
   - **Listar livros**:
     - Ordenação e paginação dos livros na interface.
     
   - **Inserir um novo livro**:
     - Validações:
       - Preço não pode ser negativo (obrigatório).
       - ISBN não pode ser repetido (obrigatório).
       - Outras validações sugeridas pelo desenvolvedor (valor agregado ao projeto).
     
   - **Alterar os dados de um livro**:
     - Validações:
       - Preço não pode ser negativo (obrigatório).
       - ISBN não pode ser repetido (obrigatório).
       - Outras validações sugeridas pelo desenvolvedor (valor agregado ao projeto).
     
   - **Eliminar um livro** (utilizando Soft Delete).

### Etapa 2: Relacionamento entre Livros e Autores

1. **Autor**:
   - Campos: id, nome.
   
2. **Livro**:
   - Campos: isbn, nome, idautor, preço.
   
3. Funcionalidades:
   - **Inserir um novo livro**:  
     - Ao inserir um livro, será necessário selecionar o autor a partir de uma lista de autores já cadastrados.
   
   - **Alterar a tabela "livro"**:  
     - Mudar o campo "autor" para "idautor" e relacionar a tabela "livro" com a tabela "autor".
   
   - **Alterar a API**:  
     - Modificar a API para enviar o "idautor" em vez do nome do autor.
   
   - **Alterar o Frontend**:  
     - Alterar o campo de texto para uma **dropdown list** que permita selecionar o autor.

---

## Responsáveis

- **Responsável pelo projeto**:  
  **Vitor Rodrigues**  
  [vitor.rodrigues@samsys.pt](mailto:vitor.rodrigues@samsys.pt)

- **Apoio e Acompanhamento**:  
  **Bruno Rodrigues**  
  [bruno.rodrigues@samsys.pt](mailto:bruno.rodrigues@samsys.pt)

  **Israel Silva**  
  [israel.silva@samsys.pt](mailto:israel.silva@samsys.pt)

---

## Considerações Finais

Este projeto tem como objetivo criar uma aplicação de gestão de livros para a livraria **BookSamsys**, utilizando tecnologias como **SQL Server**, **.NET**, **React** e **TypeScript**. É fundamental seguir as boas práticas de desenvolvimento, como a utilização de GIT para versionamento de código, arquitetura bem estruturada e validações adequadas durante o processo de criação, alteração e eliminação de livros.


