using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MissAndCan
{
    public class StateView
    {
        [DisplayName("Number Of Step")]
        public int NumberOfStep { get; set; }
    [DisplayName("Missionaries")]
        public int MissionariesLeft {get; set;}
        [DisplayName("Cannibals")]
        public int CannibalsLeft {get; set;}
        [DisplayName("Boat Status")]
        public string BoatStatus {get; set;}
        [DisplayName("Missionaries")]
        public int MissionariesRight {get; set;}
        [DisplayName("Cannibals")]
        public int CannibalsRight {get; set;}

        public StateView(State state, int number=0)
        {
            MissionariesLeft = state.Missionaries;
            MissionariesRight = state.GetMissionariesOnOtherSide();
            BoatStatus = state.BoatOnTheSide.StatusAsString();
            CannibalsLeft = state.Cannibals;
            CannibalsRight = state.GetCannibalsOnOtherSide();
            NumberOfStep = number;
        }
    }
}
