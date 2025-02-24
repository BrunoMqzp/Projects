using System;
using System.Collections;
using System.Collections.Generic;

namespace KDDC
{
    public static class Utilidades
    {

        public static void Shuffle<T>(List<T> list)
        {
            System.Random random = new System.Random();
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = random.Next(i, list.Count);
                T temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }
    }
}
