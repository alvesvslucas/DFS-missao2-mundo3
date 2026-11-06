# Diagnóstico de compatibilidade sintática T-SQL

Esta ferramenta lê `SQL/loja.sql` e analisa sua gramática com o parser oficial Microsoft ScriptDOM `161.8901.0`, usando `TSql160Parser`. O pacote tem licença MIT; não instala ou inicia SQL Server e não exige aceitar uma EULA de banco de dados.

O script contém SQL em texto, incluindo `CREATE SEQUENCE`, mas o repositório não declara inequivocamente seu SGBD/versão. Portanto, este diagnóstico mede compatibilidade com a gramática T-SQL escolhida como ferramenta. Um resultado verde não identifica o SGBD original, não verifica comportamento de dados e não homologa execução em produção. Um erro também pode indicar SQL válido em outro dialeto.

O workflow roda em push na branch `main` e em pull requests que alterem o SQL, esta ferramenta ou o próprio workflow; também mantém execução manual. Nenhum SQL é alterado ou executado. As mensagens omitem valores de registros, inclusive em erros do parser. Se o diagnóstico falhar, confira o dialeto pretendido antes de modificar o SQL; o script não deve ser adaptado apenas para satisfazer este parser.

O .NET 10 é somente o runtime desta ferramenta de CI, não um requisito atribuído ao projeto SQL original.

```sh
dotnet restore tests/SqlSyntaxCheck/SqlSyntaxCheck.csproj --source https://api.nuget.org/v3/index.json
dotnet run --project tests/SqlSyntaxCheck/SqlSyntaxCheck.csproj --configuration Release --no-restore -- SQL/loja.sql
```
