using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ReForge.Union;

[Generator]
public class UnionSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var unionClasses = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => IsUnion(node),
                transform: static (ctx, _) => GetUnionInfo(ctx))
            .Where(static info => info != null);
            
            
        context.RegisterSourceOutput(unionClasses, GenerateUnion);
    }
        
    private static bool IsUnion(SyntaxNode node)
    {
        return node is BaseTypeDeclarationSyntax classDeclaration &&
               classDeclaration.AttributeLists
                   .Any(al => al.Attributes.Any(a => a.Name.ToString() == "Union"));
    }

    private static BaseTypeDeclarationSyntax? GetUnionInfo(GeneratorSyntaxContext context)
    {
        var classDeclaration = (BaseTypeDeclarationSyntax)context.Node;
        return classDeclaration;
    }
        
    private static void Log(SourceProductionContext context, string message)
    {
        context.ReportDiagnostic(Diagnostic.Create(
            new DiagnosticDescriptor(
                id: "UnionSourceGenerator",
                title: "UnionSourceGenerator",
                messageFormat: message,
                category: "UnionSourceGenerator",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true),
            Location.None));
    }
        
    private static void GenerateUnion(SourceProductionContext context, BaseTypeDeclarationSyntax? typeDeclaration)
    {
        if (typeDeclaration is null)
            return;

        if (typeDeclaration.Modifiers.All(m => m.Text != "partial"))
        {
            Log(context, "Union classes must be declared as partial");
            return;
        }
            
        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine($"#nullable enable");
        
        sourceBuilder.AppendLine("using System;");
        sourceBuilder.AppendLine("using System.Diagnostics.CodeAnalysis;");
        
        var namespaceName = GetNamespace(typeDeclaration);
        if (namespaceName is not null)
        {
            Log(context, "Detected Namespace: " + namespaceName);
            sourceBuilder.AppendLine($"namespace {namespaceName}");
        }
        else
        {
            Log(context, "No namespace detected");
            return;
        }
            
        var unionName = typeDeclaration.Identifier.Text;
        var unionType = typeDeclaration switch
        {
            ClassDeclarationSyntax => "class",
            StructDeclarationSyntax => "struct",
            RecordDeclarationSyntax => "record",
            _ => throw new InvalidOperationException("Invalid type declaration")
        };
        var unionAccessModifierString = typeDeclaration.Modifiers.ToString();
        //Log(context, "Access Modifier: " + unionAccessModifierString);

        var nestedVariantTypes = typeDeclaration
            .DescendantNodes()
            .OfType<BaseTypeDeclarationSyntax>() // BaseTypeDeclarationSyntax is the base class for ClassDeclarationSyntax, StructDeclarationSyntax, and RecordDeclarationSyntax
            .Where(node => node is ClassDeclarationSyntax || node is StructDeclarationSyntax || node is RecordDeclarationSyntax)
            .Where(node => node.AttributeLists.Any(al => al.Attributes.Any(a => a.Name.ToString() == "Variant")))
            .ToList();
            

        sourceBuilder.AppendLine("{");
        sourceBuilder.Append(Indent(1)).AppendLine($"{unionAccessModifierString} {unionType} {unionName}");
        sourceBuilder.Append(Indent(1)).AppendLine("{");

        foreach (var variant in nestedVariantTypes)
        {
            if (variant is null) 
                continue;
                
            var variantName = variant.Identifier.Text;
            var variantType = variant switch
            {
                ClassDeclarationSyntax => "class",
                StructDeclarationSyntax => "struct",
                RecordDeclarationSyntax => "record",
                _ => throw new InvalidOperationException("Invalid type declaration")
            };
            var variantAccessModifierString = variant.Modifiers.ToString();
            //var parameters = string.Join(", ", variant.ParameterList?.Parameters);
            sourceBuilder.Append(Indent(2)).AppendLine($"{variantAccessModifierString} {variantType} {variantName} : {unionName};");
        }

        sourceBuilder.AppendLine();
        sourceBuilder.Append(Indent(2)).AppendLine("public bool Is<T>() => this is T;");
        sourceBuilder.AppendLine();
        sourceBuilder.Append(Indent(2)).AppendLine($"public T? As<T>() where T : {unionName} => this as T;");
        sourceBuilder.AppendLine();
        sourceBuilder.Append(Indent(2)).AppendLine($"public bool TryAs<T>([NotNullWhen(true)] out T? value) where T : {unionName}");
        sourceBuilder.Append(Indent(2)).AppendLine("{");
        sourceBuilder.Append(Indent(3)).AppendLine("value = this as T;");
        sourceBuilder.Append(Indent(3)).AppendLine("return value is not null;");
        sourceBuilder.Append(Indent(2)).AppendLine("}");
        sourceBuilder.AppendLine();
        sourceBuilder.Append(Indent(2)).AppendLine("public T Match<T>(");
            
        var funcLines = nestedVariantTypes
            .Select(variant =>
            {
                var variantName = variant.Identifier.Text;
                return $"{Indent(3)}Func<{variantName}, T>? {variantName.ToLower()}Func = null";
            });
        sourceBuilder.AppendLine(string.Join(",\n", funcLines));
            
        sourceBuilder.Append(Indent(2)).AppendLine(")");
        sourceBuilder.Append(Indent(2)).AppendLine("{");
        sourceBuilder.Append(Indent(3)).AppendLine("return this switch");
        sourceBuilder.Append(Indent(3)).AppendLine("{");
        foreach (var variant in nestedVariantTypes)
        {
            var variantName = variant.Identifier.Text;
            var variantParam = variantName.ToLower() + "Func";
            sourceBuilder.Append(Indent(4)).AppendLine($"{variantName} {variantName.ToLower()} => {variantParam} is not null ? {variantParam}({variantName.ToLower()}) : throw new InvalidOperationException(\"{variantName} function not provided\"),");
        }
        sourceBuilder.Append(Indent(4)).AppendLine($"_ => throw new InvalidOperationException(\"Unknown variant of {unionName}\")");
        sourceBuilder.Append(Indent(3)).AppendLine("};");
        sourceBuilder.Append(Indent(2)).AppendLine("}");
        sourceBuilder.Append(Indent(1)).AppendLine("}");
        sourceBuilder.AppendLine("}");

        context.AddSource($"{unionName}_Union.g.cs", sourceBuilder.ToString());
    }

    private static string Indent(int indentLevel)
    {
        return new string('\t', indentLevel);
    }
    
    private static string? GetNamespace(SyntaxNode node)
    {
        while (node != null)
        {
            if (node is NamespaceDeclarationSyntax namespaceDeclaration)
            {
                return namespaceDeclaration.Name.ToString();
            }
            else if (node is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclaration)
            {
                return fileScopedNamespaceDeclaration.Name.ToString();
            }

            node = node.Parent;
        }

        return null; // or throw an exception if a namespace is mandatory
    }
}