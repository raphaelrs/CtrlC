using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Servico
{
    [DataContract]
    public class Dados
    {
        [DataMember]
        public string IpDestino { get; set; }
        [DataMember]
        public string IpOrigem { get; set; }
        [DataMember]
        public string ControlC { get; set; }
        [DataMember]
        public string Usuario { get; set; }
    }
}
