/*using System.Collections.Immutable;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using System.Linq;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ListModificationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "UNITY_COLL_MOD";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        "Collection modified during enumeration", // Title
        "Cannot modify collection '{0}' inside its own foreach loop", // Message
        "Logic",
        DiagnosticSeverity.Error, // Shows as Red Error
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        // Target "foreach" loops
        context.RegisterSyntaxNodeAction(AnalyzeForEach, SyntaxKind.ForEachStatement);
    }

    private void AnalyzeForEach(SyntaxNodeAnalysisContext context)
    {
        var forEachLoop = (ForEachStatementSyntax)context.Node;

        // 1. Identify the collection being iterated
        // syntax: foreach (var item in CollectionName)
        var collectionNode = forEachLoop.Expression as IdentifierNameSyntax;
        if (collectionNode == null) return; // Complex expressions (e.g. Linq) are skipped for simplicity

        string collectionName = collectionNode.Identifier.Text;

        // 2. Scan the loop body for method calls
        var methodCalls = forEachLoop.Statement
            .DescendantNodes()
            .OfType<InvocationExpressionSyntax>();

        // 3. Define methods that modify the list
        string[] forbiddenMethods = { "Add", "Remove", "RemoveAt", "Clear", "Insert" };

        foreach (var call in methodCalls)
        {
            // Check if this call is accessing a member (e.g. list.Add)
            if (call.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                // Check if it's calling a method on OUR collection
                if (memberAccess.Expression is IdentifierNameSyntax callerId &&
                    callerId.Identifier.Text == collectionName)
                {
                    string methodName = memberAccess.Name.Identifier.Text;

                    if (forbiddenMethods.Contains(methodName))
                    {
                        var diagnostic = Diagnostic.Create(
                            Rule, 
                            call.GetLocation(), 
                            collectionName);
                        
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}*/