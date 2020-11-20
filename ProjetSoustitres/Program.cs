using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProjetSoustitres
{
    class Program
    {
        static string ChoixPath(Fichier.Langue langue)
        {
            string path = "";
            if (langue == Fichier.Langue.Français)
            {
                path = @"C:\Users\jerem\Documents\CoursYnov\2020_2021\C\LSDA_sous-titres_debut.txt";
            }
            else if (langue == Fichier.Langue.Anglais)
            {
                path = @"Chemin_anglais";
            }
            else if (langue == Fichier.Langue.Allemand)
            {
                path = @"Chemin_allemand";
            }
            else if (langue == Fichier.Langue.Espagnol)
            {
                path = @"Chemin_espagnol";
            }

            return path;
        }
        static void Main(string[] args)
        {
            string path_opt = @"C:\Users\jerem\Documents\CoursYnov\2020_2021\C\Options.txt";
            Fichier options = new Fichier(path_opt);
            Task.WaitAll(options.LectureOptions());
            string path = ChoixPath(options.langue);

            
            Fichier film = new Fichier(path);
            film.langue = options.langue;
            film.couleur = options.couleur;
            Sous_titre st = film.Initialiser_St();
            Console.Clear();
            Task.WaitAll(film.Lire(st.head, st.end));
        }
    }
}
