using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rellotge
{
    [Serializable()]
    public class DiferenciaHoraria
    {
        private String Pais;
        private float DifHoraria;

        public DiferenciaHoraria() {
            this.DifHora = 0;
        }

        public DiferenciaHoraria(String pais,float difHora) {
            this.Pais = pais;
            this.DifHora = difHora;
        }

       


        public String NomPais
        {
            get { return Pais; }
            set { Pais = value; }
        }

        public float DifHora
        {
            get { return DifHoraria; }
            set { DifHoraria = value; }
        }







    }
}
