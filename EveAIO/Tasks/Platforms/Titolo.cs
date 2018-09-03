System.InvalidOperationException: Стек пуст.
   в System.ThrowHelper.ThrowInvalidOperationException(ExceptionResource resource)
   в System.Collections.Generic.Stack`1.Peek()
   в Reflector.Disassembler.OldTranslator.WhereFromStack.get_IsClonedExpression()
   в Reflector.Disassembler.OldTranslator.DecodeStatement(Int32 end, IEnumerable`1& AdditionalExpressions)
   в Reflector.Disassembler.OldTranslator.DecodeBlockStatement(Int32 offset, Int32 end)
   в Reflector.Disassembler.OldTranslator.DecodeTryCatchFinallyStatement(IExceptionHandler current)
   в Reflector.Disassembler.OldTranslator.DecodeBlockStatement(Int32 offset, Int32 end)
   в Reflector.Disassembler.OldTranslator.TranslateMethodDeclaration(IMethodDeclaration mD, IMethodBody mB, Boolean handleExpressionStack)
   в Reflector.Disassembler.OldTranslator.TranslateMethodDeclaration(IMethodDeclaration mD, IMethodBody mB)
   в Reflector.Disassembler.Disassembler.TransformMethodDeclaration(IMethodDeclaration value)
   в Reflector.CodeModel.Visitor.Transformer.TransformMethodDeclarationCollection(IMethodDeclarationCollection methods)
   в Reflector.Disassembler.Disassembler.TransformTypeDeclaration(ITypeDeclaration value)
   в Reflector.Application.Translator.TranslateTypeDeclaration(ITypeDeclaration value, Boolean memberDeclarationList, Boolean methodDeclarationBody)
   в Reflector.Application.FileDisassembler.WriteTypeDeclaration(ITypeDeclaration typeDeclaration, String sourceFile, ILanguageWriterConfiguration languageWriterConfiguration)
namespace EveAIO.Tasks.Platforms
{
}

