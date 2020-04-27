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
                CompilationUnitVisitor visitor = new CompilationUnitVisitor(tokens);
                return visitor.Visit(parser.compilationUnit());
            }
            catch (RecognitionException)
            {
                return null;
            }
            catch (ParseCanceledException)
            {
                return null;
            }
        }

        class CompilationUnitVisitor : CParserBaseVisitor<CompilationUnit>
        {
            private CommonTokenStream tokens;

            public CompilationUnitVisitor(CommonTokenStream tokens)
            {
                this.tokens = tokens;
            }

            public override CompilationUnit VisitCompilationUnit([NotNull] CompilationUnitContext context)
            {
                var result = new CompilationUnit();

                var start = context.Start;
                var index = start.TokenIndex;
                var channel = tokens.GetHiddenTokensToLeft(index, CLexer.INCLUDE);
                if (channel != null)
                {
                    result.Process(channel);
                }

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

        class TranslationUnitVisitor : CParserBaseVisitor<List<ExternalDelaration>>
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

        class ExternalDeclarationVisitor : CParserBaseVisitor<ExternalDelaration>
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

        class FunctionDefinitionVisitor : CParserBaseVisitor<FunctionDefinition>
        {
            public override FunctionDefinition VisitFunctionDefinition([NotNull] FunctionDefinitionContext context)
            {
                var declarator = new DeclaratorVisitor();
                var statement = new CompoundStatementVisitor();
                return new FunctionDefinition(declarator.VisitDeclarator(context.declarator()),
                    statement.VisitCompoundStatement(context.compoundStatement()));
            }
        }

        class CompoundStatementVisitor : CParserBaseVisitor<CompoundStatement>
        {
            public override CompoundStatement VisitCompoundStatement([NotNull] CompoundStatementContext context)
            {
                var list = new BlockItemListVisitor();
                var scope = context.ToScope();
                if (context.blockItemList() != null)
                {
                    return new CompoundStatement(list.VisitBlockItemList(context.blockItemList()), scope);
                }

                return new CompoundStatement(scope);
            }
        }

        class BlockItemListVisitor : CParserBaseVisitor<List<IBlockItem>>
        {
            public override List<IBlockItem> VisitBlockItemList([NotNull] BlockItemListContext context)
            {
                var result = new List<IBlockItem>();
                var list = context.blockItemList();
                if (list != null)
                {
                    result.AddRange(VisitBlockItemList(list));
                }

                result.Add(new BlockItemVisitor().VisitBlockItem(context.blockItem()));
                return result;
            }
        }

        class BlockItemVisitor : CParserBaseVisitor<IBlockItem>
        {
            public override IBlockItem VisitBlockItem([NotNull] BlockItemContext context)
            {
                var declaration = context.declaration();
                if (declaration != null)
                {
                    return new DeclarationVisitor().VisitDeclaration(declaration);
                }

                var statement = context.statement();
                if (statement != null)
                {
                    return new StatementVisitor().VisitStatement(statement);
                }

                return new BrokenBlock(new Scope());
            }
        }

        class StatementVisitor : CParserBaseVisitor<Statement>
        {
            public override Statement VisitStatement([NotNull] StatementContext context)
            {
                return new Statement(context.ToScope());
            }
        }

        class DeclarationVisitor : CParserBaseVisitor<Declaration>
        {
            public override Declaration VisitDeclaration([NotNull] DeclarationContext context)
            {
                var staticAssertDeclaration = context.staticAssertDeclaration();
                if (staticAssertDeclaration != null)
                {
                    return base.VisitStaticAssertDeclaration(staticAssertDeclaration);
                }

                var specifiersContext = context.declarationSpecifiers();
                if (specifiersContext != null)
                {
                    var specifiers = new DeclarationSpecifiersVisitor().VisitDeclarationSpecifiers(specifiersContext);
                    var initDeclaratorList = context.initDeclaratorList();
                    if (initDeclaratorList != null)
                    {
                        var visitor = new InitDeclarationListVisitor();
                        return new Declaration(specifiers,
                            visitor.VisitInitDeclaratorList(initDeclaratorList), new Scope());
                    }

                    return new Declaration(specifiers, new Scope());
                }

                return new Declaration(new Scope());
            }
        }

        internal class InitDeclarationListVisitor : CParserBaseVisitor<List<InitDeclarator>>
        {
            public override List<InitDeclarator> VisitInitDeclaratorList([NotNull] InitDeclaratorListContext context)
            {
                var result = new List<InitDeclarator>();
                var list = context.initDeclaratorList();
                if (list != null)
                {
                    result.AddRange(VisitInitDeclaratorList(list));
                }

                result.Add(new InitDeclaratorVisitor().VisitInitDeclarator(context.initDeclarator()));
                var shared = context.ToScope();
                foreach (var variable in result)
                {
                    variable.OverrideScope(shared);
                }

                return result;
            }
        }

        internal class InitDeclaratorVisitor : CParserBaseVisitor<InitDeclarator>
        {
            public override InitDeclarator VisitInitDeclarator([NotNull] InitDeclaratorContext context)
            {
                return new InitDeclarator(new DeclaratorVisitor().VisitDeclarator(context.declarator()),
                    context.ToScope());
            }
        }

        internal class DeclarationSpecifiersVisitor : CParserBaseVisitor<List<DeclarationSpecifier>>
        {
            public override List<DeclarationSpecifier> VisitDeclarationSpecifiers([NotNull] DeclarationSpecifiersContext context)
            {
                var result = new List<DeclarationSpecifier>();
                var visitor = new DeclarationSpecifierVisitor();
                foreach (var specifier in context.declarationSpecifier())
                {
                    result.Add(visitor.VisitDeclarationSpecifier(specifier));
                }

                return result;
            }
        }

        internal class DeclarationSpecifierVisitor : CParserBaseVisitor<DeclarationSpecifier>
        {
            public override DeclarationSpecifier VisitDeclarationSpecifier([NotNull] DeclarationSpecifierContext context)
            {
                return new DeclarationSpecifier(context.ToString());
            }
        }

        internal class DeclaratorVisitor : CParserBaseVisitor<Declarator>
        {
            public override Declarator VisitDeclarator([NotNull] DeclaratorContext context)
            {
                var visitor = new DirectDeclaratorVisitor();
                return new Declarator(visitor.VisitDirectDeclarator(context.directDeclarator()));
            }
        }

        internal class DirectDeclaratorVisitor : CParserBaseVisitor<DirectDeclarator>
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

    internal static class ContextExtension
    {
        public static Scope ToScope(this ParserRuleContext context)
        {
            return new Scope
            {
                Start = new Position { Row = context.Start.Line - 1, Column = context.Start.Column },
                End = new Position { Row = context.Stop.Line - 1, Column = context.Stop.Column }
            };
        }
    }
}
