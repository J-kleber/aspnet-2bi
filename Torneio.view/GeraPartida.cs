using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Torneio.view
{
    public class GeraPartida
    {
        public void montaTabela()
        {
            List<string> times = new List<string>();
            times.Insert(0, "flamengo");
            times.Insert(1, "botafogo");
            times.Insert(2, "vasco");
            times.Insert(3, "bangu");
            times.Insert(4, "fluminense");
            times.Insert(5, "resende");
            times.Insert(6, "corinthians");

            if (times.Count % 2 == 1)
            {
                times.Insert(0, "");
            }
            int t = times.Count;
            int m = times.Count / 2;

            string rodada = "";

            for (int i = 0; i < t - 1; i++)
            {
                rodada = ((i + 1) + "a rodada: ");
                for (int j = 0; j < m; j++)
                {
                    //Clube está de fora nessa rodada?
                    if (times[j] == "")
                    {
                        continue;
                    }


                    //Teste para ajustar o mando de campo
                    if (j % 2 == 1 || i % 2 == 1 && j == 0)
                    {
                        rodada += times[t - j - 1] + " x " + times[j] + " ";
                    }
                    else
                    {
                        rodada += times[j] + " x " + times[t - j - 1] + " ";
                    }
                }
                rodada += ' ';
                //Gira os clubes no sentido horário, mantendo o primeiro no lugar
                int asa = times.Count - 1;
                string k = times[asa];
                times.Remove(times[asa]);
                times.Insert(1, k);

                Console.WriteLine(rodada);
            }
        }
    }
}