# Number Sequence Analyzer API

## Sobre o Projeto

Projeto desenvolvido para processar listas de números inteiros e suportar grandes volumes. 
A API oferece serviços para análise e ordenação dessas sequências numéricas.

---

## Como Rodar a Aplicação

Executar a aplicação configurada para rodar o projeto API.

- **(api/sequence/analyze)** Analisar sequências numéricas (verificar ordem, duplicados, alternância, primalidade)
- **(api/sequence/order)**   Ordenar sequências numéricas em ordem crescente e decrescente

---

## Principais Conceitos e Arquitetura

### Domínio Rico

Critérios para modelagem:

- As entidades principais (`NumberSequence`) e (`NumberOrderer`) são responsáveis por seus próprios comportamentos.
- Toda a lógica de negocio esta dentro do serviços de domínio.
- Melhora na manutenibilidade e testabilidade.
- Respeito ao Princípio da Responsabilidade Única (SRP).

---

## Abordagens Técnicas

- **IAsyncEnumerable**: Para processar sequências grandes em fluxo contínuo, sem carregar tudo em memória, garantindo escalabilidade e eficiência.
- **ToListAsync**: A utilização do `IAsyncEnumerable<int>`, propoe que a materialização em memória ocorre apenas no momento da ordenação, assim, com a utilização do Sort() e a reutilizando a lista. Garantindo assim a compatibilidade entre fluxos assíncronos.
- **Detecção de duplicados**: Implementada com `HashSet<int>`, para garantir alta performance na verificação de elementos únicos.
- **Verificação de alternância (picos e vales)**: Utiliza uma janela deslizante com `Queue<int>`, combinando os métodos `Enqueue`, `ElementAt` e `Dequeue` para identificar padrões na sequência.
- **Verificação de primalidade paralela**: Utiliza `Task.Run` na distribuição e checagem de cada número em threads paralelas, `Task.WhenAll` para aguardar todas as verificações.
- **Logging com Serilog**: Registro informações e erros para melhor monitoramento e debugging.
- **Testes unitários**: Desenvolvidos com **xUnit** e **NSubstitute**.

---

## Estrutura dos Testes

- Testes da camada API focados na validação dos endpoints e integração com serviços.
- Testes da camada Application validam a lógica de negócio aplicada nas requisições.
- Testes da camada Domain garantem a correta implementação das regras de negócio no modelo rico.
