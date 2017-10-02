using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSocialNetworkDTO.PostalCode
{
    public class PostalCodeDTO
    {
        public long ID { get; set; }
        public int Codigo_Postal { get; set; }
        public int Extensao { get; set; }
        public string Designacao_Postal { get; set; }
        public string Localidade { get; set; }
        public int? Codigo_Arteria { get; set; }
        public string Tipo_Arteria { get; set; }
        public string Prep1 { get; set; }
        public string Titulo_Arteria { get; set; }
        public string Prep2 { get; set; }
        public string Nome_Arteria { get; set; }
        public string Local_Arteria { get; set; }
        public string Troco { get; set; }
        public int? Porta { get; set; }
    }
}
