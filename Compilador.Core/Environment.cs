using System;
using System.Collections.Generic;
using Compilador.Core;
using Compilador.Core.Expressions;

namespace Compilador.Core
{
    public static class EnvironmentManager
    {
        public static List<Environment> contexts = new List<Environment>();
        private static List<Environment> interpretationContexts = new List<Environment>();

        public static Environment PushContext()
        {
            var env = new Environment();
            contexts.Add(env);
            interpretationContexts.Add(env);
            return env;
        }

        public static Environment PopContext()
        {
            var lastContext = contexts.Last();
            contexts.Remove(lastContext);
            return lastContext;
        }

        public static Environment getContextBlock()
        {
            var lastContext = contexts.First();
            return lastContext;
        }

        public static IEnumerable<Symbol> GetSymbolsForCurrentContext()
        {
            if (!contexts.Any())
            {
                return Enumerable.Empty<Symbol>();
            }
            return contexts.Last().GetSymbolsForCurrentContext();
        }

        public static IEnumerable<Symbol> GetSymbolsForBlockContext()
        {
            if (!contexts.Any())
            {
                return Enumerable.Empty<Symbol>();
            }
            return contexts.First().GetSymbolsForBlockContext();
        }

        public static IEnumerable<Symbol> GetSymbolsUnassignedSymbolsInterpretation()
        {
            if (!interpretationContexts.Any())
            {
                return Enumerable.Empty<Symbol>();
            }
            return interpretationContexts.SelectMany(x => x.GetSymbolsForCurrentContext()).Where(x => x.Value == null);
        }

        public static void UpdateSymbol(string symbolName, dynamic value)
        {
            for (int i = interpretationContexts.Count - 1; i >= 0; i--)
            {
                var symbol = interpretationContexts[i].Get(symbolName);
                if (symbol != null)
                {
                    interpretationContexts[i].UpdateSymbolValue(symbolName, value);
                }
            }
        }

        public static void Put(string lexeme, IdExpression id, dynamic startValue)
        {
            int conteo = 0;
            foreach (var ctx in contexts)
            {
                var symbol = ctx.Get(lexeme);
                if (symbol != null)
                {
                    return;
                }
                if (conteo == contexts.Count-1)
                {
                    break;
                }
                conteo++;
                
            }
            contexts.Last().Put(lexeme, id, startValue);
        }

        public static Symbol Get(string lexeme)
        {
            for (int i = contexts.Count - 1; i >= 0; i--)
            {
                var symbol = contexts[i].Get(lexeme);
                if (symbol != null)
                {
                    return symbol;
                }
            }

            throw new ApplicationException($"Symbol {lexeme} doesn't exist in current context");
            //return null;
        }
        public static Symbol PutCheckPrevious(string lexeme)
        {
            for (int i = contexts.Count - 1; i >= 0; i--)
            {
                var symbol = contexts[i].Get(lexeme);
                if (symbol != null)
                {
                    return symbol;
                }
            }

            throw new ApplicationException($"Symbol {lexeme} doesn't exist in current context");
            //return null;
        }

        public static Symbol GetSymbolForInterpretation(string lexeme)
        {
            for (int i = interpretationContexts.Count - 1; i >= 0; i--)
            {
                var symbol = interpretationContexts[i].Get(lexeme);
                if (symbol != null)
                {
                    return symbol;
                }
            }

            throw new ApplicationException($"Symbol {lexeme} doesn't exist in current context");
        }
    }
    public class Environment
    {
        private readonly Dictionary<string, Symbol> _symbolTable;
        //protected Environment Previous;

        public Environment()
        {
            _symbolTable = new Dictionary<string, Symbol>();
        }

        public void Put(string lexeme, IdExpression id, Symbol startValue)
        {
            if (_symbolTable.ContainsKey(lexeme))
            {
                //throw new ApplicationException($"Symbol {lexeme} was previously defined in this scope");
            }else
                _symbolTable.Add(lexeme, new Symbol(id, startValue));
        }

        public void UpdateSymbolValue(string symbolName, dynamic value)
        {
            var variable = Get(symbolName);
            variable.Value = value;
            _symbolTable[symbolName] = variable;
        }
        public Symbol Get(string lexeme)
        {
            if (_symbolTable.TryGetValue(lexeme, out var symbol))
            {
                return symbol;
            }

            return null;
        }

        public IEnumerable<Symbol> GetSymbolsForCurrentContext() => this._symbolTable.Select(x => x.Value);

        public IEnumerable<Symbol> GetSymbolsForBlockContext() => this._symbolTable.Select(x => x.Value);

    }
}
