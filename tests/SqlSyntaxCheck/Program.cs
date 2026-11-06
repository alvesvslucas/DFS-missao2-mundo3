using Microsoft.SqlServer.TransactSql.ScriptDom;

if (args.Length != 1 || !File.Exists(args[0]))
{
    Console.Error.WriteLine("Informe o caminho do arquivo SQL existente.");
    return 2;
}

// ScriptDOM only builds an AST. It does not connect to a database or execute SQL.
var parser = new TSql160Parser(initialQuotedIdentifiers: true);
using (var invalid = new StringReader("CREATE TABLE ("))
{
    parser.Parse(invalid, out IList<ParseError> invalidErrors);
    if (invalidErrors.Count == 0)
    {
        Console.Error.WriteLine("O parser não rejeitou o controle negativo sintético.");
        return 2;
    }
}

using var reader = File.OpenText(args[0]);
var fragment = parser.Parse(reader, out IList<ParseError> errors);
if (errors.Count > 0)
{
    // Do not print ParseError.Message: it can contain values from INSERT payloads.
    foreach (var error in errors)
        Console.Error.WriteLine($"Erro sintático T-SQL {error.Number}: linha {error.Line}, coluna {error.Column}.");
    return 1;
}

if (fragment is not TSqlScript script || script.Batches.Sum(batch => batch.Statements.Count) == 0)
{
    Console.Error.WriteLine("O arquivo não contém instruções SQL analisáveis.");
    return 1;
}

Console.WriteLine("Compatibilidade sintática T-SQL verificada pelo parser. Nenhum comando SQL foi executado.");
Console.WriteLine("O SGBD e a versão originais continuam pendentes de confirmação.");
return 0;
