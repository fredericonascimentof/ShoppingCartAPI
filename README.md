# 🛒 ShoppingCart API (.NET 8)

Projeto de API RESTful construída em ASP.NET Core 8 para simular um e-commerce com operações de Categorias, Produtos e Carrinho de Compras.

Este projeto foi desenvolvido como parte de um desafio técnico.

---

## ✅ Funcionalidades

- **Categorias**
  - Criar, listar, atualizar e inativar categorias (soft delete).
- **Produtos**
  - Cadastrar produtos vinculados a categorias.
  - Controle de estoque, preço normal e preço promocional (opcional).
- **Carrinho de Compras**
  - Criar carrinho com `Guid`.
  - Adicionar itens com validação de estoque.
  - Aplicar preço promocional se existir.
  - Listar itens do carrinho com total.
- **Regras**
  - Estoque insuficiente retorna 400.
  - Soft-delete impede uso de itens inativos.
  - Carrinho inexistente retorna 404.

---

## 🧪 Testes Automatizados

Durante o desenvolvimento do projeto, foram implementados testes unitários utilizando **xUnit** e **Moq** para validar a lógica da API.
Porém, enfrentei dificuldades técnicas para concluir todos os testes, principalmente pela minha falta de experiência prévia com testes automatizados em .NET.
Alguns testes não foram concluídos ou apresentaram falhas, e por isso optei por focar na implementação funcional da API para garantir as funcionalidades principais.
Continuarei estudando e aprimorando a parte de testes para projetos futuros.

---

## 📂 Estrutura do Projeto

- **ShoppingCart.Domain**: Entidades e regras de negócio.
- **ShoppingCart.Application**: Serviços e lógica da aplicação.
- **ShoppingCart.Infrastructure**: Implementação dos repositórios.
- **ShoppingCart.API**: Camada API RESTful.
- **ShoppingCart.Tests**: Projetos de testes unitários.

## 🚀 Como executar

1. Clone o repositório:
   ```bash
   git clone [https://github.com/fredericonascimentof/ShoppingCartAPI.git]

