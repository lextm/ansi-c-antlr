using System;
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

        class TranslationUnitVisitor : CBaseVisitor<TranslationUnit>
        {
            public override TranslationUnit VisitTranslationUnit([NotNull] TranslationUnitContext translationUnit)
            {
                throw new NotImplementedException();
            }
        }
    }
}
