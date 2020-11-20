using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProjetSoustitres
{
    class Fichier
    {
        public string path;
        public ConsoleColor couleur;
        public Langue langue;
        public enum Langue
        {
            Français,
            Anglais,
            Allemand,
            Espagnol
        }

        public Fichier(string Path)
        {
            path = Path;
        }

        public void Choix(int count)
        {
            int choix = 0;
            int erreur = 0;
            do
            {
                erreur = 0;
                try
                {
                    choix = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Veuillez rentrer une valeur exacte");
                    erreur = 1;
                }
            } while (erreur == 1);
            if (count == 6)
                this.langue = (Langue)choix;
            else if (count == 15)
                this.couleur = (ConsoleColor)choix;
        }
        public async Task LectureOptions()
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string l = "";
                int count = 0;
                int nbEspaces;
                while ((l = sr.ReadLine()) != null)
                {
                    nbEspaces = (Console.WindowWidth - l.Length) / 2;
                    foreach (char i in l)
                    {   
                        Console.SetCursorPosition(nbEspaces, Console.CursorTop);
                        Console.Write(i);
                        nbEspaces++;
                        await Task.Delay(50);
                    }
                    Console.WriteLine();
                    if (count == 6 || count == 15)
                    {
                        Choix(count); // on stock le choix de la langue et de la couleur;
                    }
                    if (count == 1 || count == 6 || count == 15)
                    {
                        await Task.Delay(2000);
                        Console.Clear();
                    }
                    count = count + 1;
                }
            }
        }

        public Sous_titre Coupage(StreamReader sr, string l)
        {
            int erreur;
            do // on boucle et essaye de convertir la ligne tant qu'on est pas à la première ligne de chaque bloque c'est a dire le numéro du st
            {
                erreur = 1;
                try
                {
                    int nbr_st = Convert.ToInt32(l);
                }
                catch (FormatException)
                {
                    l = sr.ReadLine();
                    erreur = 0;
                }
            } while (erreur == 0);
            l = sr.ReadLine();
            string[] separator = new string[] { " --> " };
            string[] time_st = l.Split(separator, StringSplitOptions.RemoveEmptyEntries); //la deuxième option sert à virer les champs vide
            string time_st_deb = time_st[0].Remove(8,1); // on enlève la virgule pour assembler les secondes et millisecondes
            string time_st_fin = time_st[1].Remove(8,1);

            Sous_titre st_next = new Sous_titre(time_st_deb.Split(':'), time_st_fin.Split(':'), sr.ReadLine(), sr.ReadLine());
            return st_next;
        }

        public Sous_titre Initialiser_St()
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string l = "";
                Sous_titre st = Coupage(sr, sr.ReadLine());
                st.head = st;
                while ((l = sr.ReadLine()) != null)
                {
                    st.next = Coupage(sr, l);
                    st.next.head = st.head;
                    st = st.next;
                }
                return st;
            }
        }
        public void CentrerLeTexte(string texte)
        {
            Console.SetCursorPosition((Console.WindowWidth - texte.Length) / 2, Console.WindowHeight / 2);
            Console.WriteLine(texte);
        }
        public void AfficherChrono(Stopwatch chrono, TimeSpan ts, String temps)
        {
            ts = chrono.Elapsed;
            temps = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            Console.SetCursorPosition((Console.WindowWidth - temps.Length) / 2, 3);
            Console.WriteLine(temps);
        }
       /* public async Task<string> Pause() //Essaie de la fonction pour faire pause, blocage pour récupérer une saisie d'utilisateur en asynchrone
        {
            string t = Task.Run(() => Console.ReadLine());
            return t;
        }*/
        /*public async Task Lire(Sous_titre st, int timer_end) //Fonction Lire sans le timer, avec des await
        {
            int ancien = 0;
            Console.ForegroundColor = this.couleur;
            Console.Clear();
            CentrerLeTexte("Le film va pouvoir commencer, asseyez-vous au fond de votre chaise et prenez votre bol de pop corn");
            await Task.Delay(3000);
            Console.Clear();
            do
            {
                await Task.Delay(st.start - ancien);
                CentrerLeTexte(st.st1);
                CentrerLeTexte(st.st2);
                await Task.Delay(st.end - st.start);
                Console.Clear();
                ancien = st.end;
                st = st.next;
            } while (st != null);
        }*/
        public async Task Lire(Sous_titre st, int timer_end) //Fonction Lire avec le timer
        {
            int ancien = 0;
            Console.ForegroundColor = this.couleur;
            Console.Clear();
            CentrerLeTexte("Le film va pouvoir commencer, asseyez-vous au fond de votre chaise et prenez votre bol de pop corn");
            await Task.Delay(3000);
            Console.Clear();
            Stopwatch chrono = new Stopwatch();
            chrono.Start();
            TimeSpan ts = chrono.Elapsed;
            string temps = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            do
            {
                //string pause = Pause();
                while (chrono.ElapsedMilliseconds != st.start)
                {
                    AfficherChrono(chrono, ts, temps);
                }
                CentrerLeTexte(st.st1);
                CentrerLeTexte(st.st2);
                while (chrono.ElapsedMilliseconds != st.end)
                {
                    AfficherChrono(chrono, ts, temps);
                }
                Console.Clear();
                ancien = st.end;
                st = st.next;
            } while (chrono.IsRunning && st != null);
        }
    }
}
