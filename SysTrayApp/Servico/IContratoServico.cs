using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Servico
{
    [ServiceContract(CallbackContract = typeof(IContratoCallback), SessionMode = SessionMode.Required)]
    public interface IContratoServico
    {
        [OperationContract(IsInitiating = true)]
        bool Connect(Dados dados);

        [OperationContract(IsOneWay = true)]
        void EnviarDados(string ipOrigem, string usuario, string valor);
        
        [OperationContract(IsOneWay = true)]
        void MudarUsuario(string ip, string usuario, string usuarioAntigo);

        [OperationContract(IsOneWay = true, IsTerminating = true)]
        void Disconnect(string ip);

        [OperationContract(IsOneWay = false)]
        bool SendFile(Arquivo fileMsg, Dados destino);
    }
}
