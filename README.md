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

## 🚀 Como executar

1. Clone o repositório:
   ```bash
   git clone [https://github.com/fredericonascimentof/ShoppingCartAPI.git]
   cd ShoppingCartAPI
