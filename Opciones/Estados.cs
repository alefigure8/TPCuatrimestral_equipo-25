using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opciones
{
    public class Estados
    {

        //public const int Cancelado = 0;
        public const int Solicitado = 1;
        public const int EnPreparacion = 2;
        public const int DemoradoEnCocina = 3;
        public const int ListoParaEntregar = 4;
        public const int Entregado = 5;


        public string getColorEstado(int id)
        {
            if (id == Solicitado) return "#D4FFD5";
            if (id == EnPreparacion) return "#34B437";
            if (id == DemoradoEnCocina) return "#FFA200";
            if (id == ListoParaEntregar) return "#FFBEE8";
            if (id == Entregado) return "#FF4C75";
            return "";

        }

    }
}
