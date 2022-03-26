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
            if (list == null && list2 == null)
            {
                Size = 0;
                InitZero = true;
            }
            this.ValidateSemantic();
        }
        public IdExpression Id { get; }

        public List<int> list { get; }

        public List<string> list2 { get; }

        public int Size { get; set; }

        public bool InitZero{ get; set; }

        public override string GenerateCode()
        {
            var code = string.Empty;
            if (list != null)
            {
                int contador = 0;
                code += $"{this.Id.GenerateCode()} = new List<int>()";
                if (Size > 0)
                {
                    code += "{ ";
                    for (int i = 0; i < Size; i++)
                    {
                        code += list[i].ToString();
                        contador++;
                        if (Size != contador)
                        {
                            code += ", ";
                        }
                    }
                    code += $" }};{System.Environment.NewLine}";
                }
                else
                {
                    code += $";{System.Environment.NewLine}";
                }
                
            }
            else if(list2 != null)
            {
                int contador = 0;
                code += $"{this.Id.GenerateCode()} = new List<string>(){{ ";
                for (int i = 0; i < Size; i++)
                {
                    code += list2[i].ToString();
                    contador++;
                    if (Size != contador)
                    {
                        code += ", ";
                    }
                }
                code += $" }};{System.Environment.NewLine}";
            }
            else
            {
                code += $"{this.Id.GenerateCode()} = new List<int>();{System.Environment.NewLine}";
            }
            
            return code;
        }

        public override void ValidateSemantic()
        {
            
            if (list != null && Size == 0)
            {
                Size = list.Count();
            }
            else if(list2 != null && Size == 0)
            {
                Size = list2.Count();
            }
        }
    }
}
