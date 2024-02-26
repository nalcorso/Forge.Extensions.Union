using Microsoft.CodeAnalysis;

namespace ReForge.Union;

internal static class SyntaxNodeHelper
{
    public static bool TryGetParentSyntax<T>(SyntaxNode? syntaxNode, out T? result) where T : SyntaxNode
    {
        result = null;

        if (syntaxNode is null)
            return false;
        
        try
        {
            syntaxNode = syntaxNode.Parent;

            if (syntaxNode is null)
                return false;

            if (syntaxNode.GetType() != typeof(T)) 
                return TryGetParentSyntax<T>(syntaxNode, out result);
            
            result = syntaxNode as T;
            return true;

        }
        catch
        {
            return false;
        }
    }
}