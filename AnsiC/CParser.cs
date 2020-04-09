using System;
using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;

namespace Lextm.AnsiC
{
    public partial class CParser
    {
        public ParserRuleContext Parse()
        {
            ErrorHandler = new BailErrorStrategy();
            Interpreter.PredictionMode = PredictionMode.Sll;
            var document = compilationUnit();
            return document;
        }

        public static CompilationUnit ParseDocument(string fileName)
        {
            return ParseContent(File.ReadAllText(fileName));
        }

        public static CompilationUnit ParseContent(string text)
        {
            try
            {
                var lexer = new CLexer(new AntlrInputStream('\n' + text));
                var tokens = new CommonTokenStream(lexer);
                var parser = new CParser(tokens);
                CompilationUnitVisitor visitor = new CompilationUnitVisitor();
                return visitor.Visit(parser.compilationUnit());
            }
            catch (RecognitionException ex)
            {
                return null;
            }
            catch (ParseCanceledException ex)
            {
                return null;
            }
        }

        class CompilationUnitVisitor : CBaseVisitor<CompilationUnit>
        {
            public override CompilationUnit VisitCompilationUnit([NotNull] CompilationUnitContext context)
            {
                var result = new CompilationUnit();
                var translationUnit = context.translationUnit();
                if (translationUnit == null)
                {
                    return result;
                }

                var translationUnitVisitor = new TranslationUnitVisitor();
                var unit = translationUnitVisitor.VisitTranslationUnit(translationUnit);

                result.Handle(unit);
                return result;
            }
        }

        class TranslationUnitVisitor : CBaseVisitor<List<ExternalDelaration>>
        {
            public override List<ExternalDelaration> VisitTranslationUnit([NotNull] TranslationUnitContext context)
            {
                var nestUnit = context.translationUnit();
                var visitor = new ExternalDeclarationVisitor();
                var externalDeclaration = visitor.VisitExternalDeclaration(context.externalDeclaration());
                if (nestUnit == null)
                {
                    return new List<ExternalDelaration> { externalDeclaration };
                }

                var result = VisitTranslationUnit(nestUnit);
                result.Add(externalDeclaration);
                return result;
            }
        }

        class ExternalDeclarationVisitor : CBaseVisitor<ExternalDelaration>
        {
            public override ExternalDelaration VisitExternalDeclaration([NotNull] ExternalDeclarationContext context)
            {
                var function = context.functionDefinition();
                if (function != null)
                {
                    var functionVisitor = new FunctionDefinitionVisitor();
                    return new ExternalDelaration(functionVisitor.VisitFunctionDefinition(function));
                }

                var declaration = context.declaration();
                if (declaration == null)
                {
                    return null;
                }

                var declarationVisitor = new DeclarationVisitor();
                return new ExternalDelaration(declarationVisitor.VisitDeclaration(declaration));
            }
        }

        class FunctionDefinitionVisitor : CBaseVisitor<FunctionDefinition>
        {
            public override FunctionDefinition VisitFunctionDefinition([NotNull] FunctionDefinitionContext context)
            {
                var declarator = new DeclaratorVisitor();
                return new FunctionDefinition(declarator.VisitDeclarator(context.declarator()));
            }
        }

        class DeclarationVisitor : CBaseVisitor<Declaration>
        {
            public override Declaration VisitDeclaration([NotNull] DeclarationContext context)
            {
                return base.VisitDeclaration(context);
            }
        }

        internal class DeclaratorVisitor : CBaseVisitor<Declarator>
        {
            public override Declarator VisitDeclarator([NotNull] DeclaratorContext context)
            {
                var visitor = new DirectDeclaratorVisitor();
                return new Declarator(visitor.VisitDirectDeclarator(context.directDeclarator()));
            }
        }

        internal class DirectDeclaratorVisitor : CBaseVisitor<DirectDeclarator>
        {
            public override DirectDeclarator VisitDirectDeclarator([NotNull] DirectDeclaratorContext context)
            {
                var identifier = context.Identifier();
                if (identifier != null)
                {
                    return new DirectDeclarator(identifier.ToString());
                }

                var nest = context.directDeclarator();
                if (nest != null)
                {
                    return VisitDirectDeclarator(nest);
                }

                return base.VisitDirectDeclarator(context);
            }
        }
    }
}
