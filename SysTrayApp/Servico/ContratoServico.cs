using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Servico
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,ConcurrencyMode = ConcurrencyMode.Multiple,
       UseSynchronizationContext = false)]
    public class ContratoServico :IContratoServico
    {
        object syncObj = new object();

        Dictionary<Dados, IContratoCallback> clientes = new Dictionary<Dados, IContratoCallback>();
        List<Dados> listaClientes = new List<Dados>();

        public IContratoCallback CurrentCallback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IContratoCallback>();
            }
        }

        private bool SearchClientsByName(string ip)
        {
            foreach (Dados c in clientes.Keys)
            {
                if (c.IpOrigem == ip)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Connect(Dados dados)
        {
            if (!clientes.ContainsValue(CurrentCallback) && !SearchClientsByName(dados.IpOrigem))
            {
                lock (syncObj)
                {
                    clientes.Add(dados, CurrentCallback);
                    listaClientes.Add(dados);
                }
                return true;
            }
            return false;
        }

        public void EnviarDados(string ipOrigem, string usuario, string valor)
        {
            lock (syncObj)
            {
                foreach (var item in clientes.Keys)
                {
                    if (item.Usuario == usuario && ipOrigem != item.IpOrigem)//original
                    //if (item.Usuario == usuario)
                    {
                        clientes[item].Receber(valor);
                    }
                }
            }
        }

        public void Disconnect(string ip)
        {
            foreach (Dados c in clientes.Keys)
            {
                if (c.IpOrigem == ip)
                {
                    lock (syncObj)
                    {
                        this.clientes.Remove(c);
                        this.listaClientes.Remove(c);
                    }
                    return;
                }
            }
        }
        
        public bool SendFile(Arquivo fileMsg, Dados destino)
        {
            foreach (Dados rcvr in clientes.Keys)
            {
                if (rcvr.Usuario == destino.Usuario)
                {
                    Dados msg = new Dados();
                    msg.IpOrigem = fileMsg.Sender;

                    IContratoCallback rcvrCallback = clientes[rcvr];
                    rcvrCallback.ReceberArquivo(fileMsg);
                }
            }
            return false;
        }

        public void MudarUsuario(string ip, string usuario, string usuarioAntigo)
        {
            lock (syncObj)
            {
                foreach (var item in clientes.Keys)
                {
                    if (item.Usuario == usuarioAntigo)
                    {
                        item.Usuario = usuario;
                    }
                }
            }
        }
    }
}
