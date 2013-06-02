module Experiments

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

open Roslyn.Compilers
open Roslyn.Compilers.CSharp
open Roslyn.Compilers.Common
open Roslyn.Services
open Roslyn.Services.CSharp

open SemanticDiff.Core.AST.Comparison

let sampleCode = "void foo()\
                  {
                  int i = 1 + 1;
                  }"

let sampleCode2 = "void foo()\
                   { /* Same Code
                   But with comments... */
                   int i=1+1;
                   // ... and whitespace changes
                   }"

let codeWithMethods = "void foo()
                    {
                    return;
                    }
                    int bar()
                    {
                    return 1;
                    }
                    int foobar()
                    {
                    return bar() + bar ();
                    }"

[<TestClass>]
type RoslynBasicExperiments() = 

    [<TestMethod>]
    member this.GenerateSimpleAST () = 
        Assert.AreNotEqual(String.Empty, sampleCode)

        let ast1 = SyntaxTree.ParseText sampleCode
        let identical_ast = SyntaxTree.ParseText sampleCode
        let ast2 = SyntaxTree.ParseText sampleCode2

        Assert.AreNotEqual(ast1, identical_ast)

        (* The "IsEquivalentTo" disregards SyntaxTrivia *)
        Assert.IsTrue(ast1.IsEquivalentTo ast2)

    [<TestMethod>]
    member this.FindMethodDeclarationInAST() = 
        let ast = SyntaxTree.ParseText codeWithMethods
        let visitor = new MethodCollector()
        
        visitor.Visit(ast.GetRoot())

        Assert.AreEqual(3, List.length visitor.VisitedMethods)

    [<TestMethod>]
    member this.CompareSameMethodOnSuccessiveParsing() =
        let ast1 = SyntaxTree.ParseText codeWithMethods
        let visitor1 = new MethodCollector()
        visitor1.Visit <| ast1.GetRoot()

        let ast2 = SyntaxTree.ParseText codeWithMethods
        let visitor2 = new MethodCollector()
        visitor2.Visit <| ast2.GetRoot()

        Assert.IsTrue(visitor1.VisitedMethods.Head.IsEquivalentTo visitor2.VisitedMethods.Head)