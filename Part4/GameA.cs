using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part4
{
    class GameA : IGame
    {
        //Override
        public bool[] GameField { get; set; } 


        public GameA(int width, int height)
        {
            GameField = new bool[width * height];
            GameField.Initialize();
            for (int j = 0; j < height; j+=2)
            {
                for (int i = 0; i < width; i++)
                {
                    GameField[j * width + i] = true;
                }

            }
        }



    }
}
