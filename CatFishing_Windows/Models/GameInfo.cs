using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFishing_Windows.Models
{

    public enum GameStates
    {
        Starting = 0,
        Running = 1,
        Ended = 2,
        Loading = 3
    }
    
    class GameInfo
    {
        public GameStates NowGameState { get; set; }
        
        public double Counter { get; set; }
    }
}
