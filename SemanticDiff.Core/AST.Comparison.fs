namespace SemanticDiff.Core.AST

module Comparison = 

    open Roslyn.Compilers
    open Roslyn.Compilers.CSharp
    open Roslyn.Compilers.Common
    open Roslyn.Services
    open Roslyn.Services.CSharp
    
    type MethodCollector () = 
        inherit SyntaxWalker()
        let mutable i = 0
        let mutable visitedMethods = []

        member this.VisitedMethods
            with get() = visitedMethods

        override this.VisitMethodDeclaration node = 
            i <- i + 1
            visitedMethods <- node :: visitedMethods
            // good-old printf debugging :-)
            printfn "Visiting %s : %dth method" <| node.Identifier.ValueText <| i

    let internal collectMethodsInternal (collector:MethodCollector) (node:SyntaxNode) = 
        collector.Visit(node)
        collector.VisitedMethods

    let collectMethods node = 
        let collector = new MethodCollector()
        collectMethodsInternal collector node
