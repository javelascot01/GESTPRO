using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GESTPRO.domain
{
    internal class Helpers
    {
        // Atributos para el metodo random
        private static Random rand = new Random();
        private static readonly object syncLock = new object();
        // Metodo random
        public static int random(int min, int max)
        {
            lock (syncLock)
            {
                return rand.Next(min, max + 1);
            }
        }
    }
}
