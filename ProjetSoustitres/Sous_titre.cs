using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetSoustitres
{
    class Sous_titre
    {

        public int start;
        public int end;
        public string st1;
        public string st2;
        public Sous_titre head; // stock le tout premier sous titre pour pouvoir reveni au début quand tout sera stocker
        public Sous_titre next; //stock le sous-titre prochain

        public Sous_titre(string[] Start, string[] End, string St1, string St2)
        {
            //On convertit les durées en millisecondes
            start = Convert.ToInt32(Start[0]) * 3600000 + Convert.ToInt32(Start[1]) * 60000 + Convert.ToInt32(Start[2]);
            end = Convert.ToInt32(End[0]) * 3600000 + Convert.ToInt32(End[1]) * 60000 + Convert.ToInt32(End[2]);
            st1 = St1;
            st2 = St2;
            next = null;
        }
    }
}
