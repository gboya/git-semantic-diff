module Experiments

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

open Roslyn.Compilers
open Roslyn.Compilers.CSharp
open Roslyn.Compilers.Common
open Roslyn.Services
open Roslyn.Services.CSharp

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
