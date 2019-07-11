using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part3
{
    interface IGameBase
    {

        event EventHandler GameOver;
        int Score { get; set; }
        bool Pause { get; set; }
        bool[,] FieldToDisplay { get; set; }
        bool[,] FieldAdditional { get; set; }
        bool EndGame { get; set; }
        void DownKey();
        void LeftKey();
        void RightKey();
        void UpKey();
        void FuncKeyPressed();

    }

}
