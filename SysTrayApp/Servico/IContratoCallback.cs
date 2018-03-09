using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Servico
{
    public interface IContratoCallback
    {
        [OperationContract(IsOneWay = true)]
        void Receber(string valor);

        [OperationContract(IsOneWay = true)]
        void ReceberArquivo(Arquivo file);
    }
}
