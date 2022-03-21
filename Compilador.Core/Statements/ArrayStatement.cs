using Compilador.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Core.Statements
{
    public class ArrayStatement : Statement
    {
        public ArrayStatement(IdExpression id, List<int> lista, List<string> listaS)
        {
            Id = id;
            list = lista;
            list2 = listaS;
            this.ValidateSemantic();
        }
        public IdExpression Id { get; }

        public List<int> list { get; }

        public List<string> list2 { get; }

        public override string GenerateCode()
        {
            var code = string.Empty;
            if (list != null)
            {
                int contador = 0;
                code += $"{this.Id.GenerateCode()} = new List<int>(){{ ";
                foreach (var item in list)
                {
                    code += item.ToString();
                    contador++;
                    if (list.Count() != contador)
                    {
                        code += ", ";
                    }
                }
                code += $" }}{System.Environment.NewLine}";
            }
            else
            {
                int contador = 0;
                code += $"{this.Id.GenerateCode()} = new List<string>(){{ ";
                foreach (var item in list2)
                {
                    code += item.ToString();
                    contador++;
                    if (list2.Count() != contador)
                    {
                        code += ", ";
                    }
                }
                code += $" }}{System.Environment.NewLine}";
            }
            
            return code;
        }

        public override void ValidateSemantic()
        {
            
        }
    }
}
