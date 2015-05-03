using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissAndCan
{
    /// <summary>
    /// State of Left beach.
    /// </summary>
    public class State: IEquatable<State>
    {
        private int nMiss, nCan;
        private BoatState _boatOnTheSide;
        private static int NumOfEachAtStart = 3;
        private String _name;
        private State _prevState;
        private int stateLevel = 0;
        private int _cost;

        #region public fields
        public State PrevState
        {
            get { return _prevState; }
            private set { ;}
        }
        public int StateLevel
        {
            get { return stateLevel; }
            private set { ;}
        }

        public string Name
        {
            get { return _name; }
            private set { ;}
        }

        public BoatState BoatOnTheSide
        {
            get { return _boatOnTheSide; }
            private set { ;}
        }

        public int Missionaries
        {
            get { return nMiss; }
            private set { ;}
        }

        public int Cannibals
        {
            get { return nCan; }
            private set { ;}
        }
        public int Cost
        {
            get { return _cost; }
            private set { ; }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="nMiss">Number of Missionaries within the current state</param>
        /// <param name="nCan">Number of Cannibals within the current state</param>
        /// <param name="BoatOnTheSide">An indication of the banks at which the boat is</param>
        /// <param name="stateLevel"></param>
        /// <param name="Cost"></param>
        public State(int nMiss, int nCan, BoatState BoatOnTheSide,
                    int stateLevel = 0, int NumOfEachAtStart = 3, 
                    int Cost = 0):
            this(nMiss, nCan, BoatOnTheSide, null, stateLevel, NumOfEachAtStart, Cost)
    {
        ;
    }
        public State(int nMiss, int nCan, BoatState BoatOnTheSide,
                    State PrevState, int stateLevel = 0,
                    int NumOfEachAtStart = 3, int Cost = 0)
        {
            this._name = Name;
            this.nMiss = nMiss;
            this.nCan = nCan;
            this._boatOnTheSide = BoatOnTheSide;
            this._prevState = PrevState;
            this.stateLevel = stateLevel;
            this._cost = Cost + 1;
            if(_prevState != null)
                this._cost += this._prevState.Cost;
            State.NumOfEachAtStart = NumOfEachAtStart;
        }

        public override string ToString()
        {
            return String.Format("{0}M/{1}C {2:7} {3}M/{4}C",
                Missionaries, Cannibals,
                BoatOnTheSide.PositionAsString(),
                GetMissionariesOnOtherSide(),
                GetCannibalsOnOtherSide());
        }

        public bool Equals(State OtherState)
        {
            if (OtherState == null)
                return false;
            return (nMiss == OtherState.Missionaries &&
                nCan == OtherState.Cannibals &&
                BoatOnTheSide == OtherState.BoatOnTheSide);
        }

        public bool IsValidState()
        {
            if (nMiss < 0 || nCan < 0 ||
                nMiss > NumOfEachAtStart ||
                nCan > NumOfEachAtStart)
                return false;
            if (nMiss < nCan && nMiss > 0)
                return false;
            var otherMiss = GetMissionariesOnOtherSide();
            var otherCan = GetCannibalsOnOtherSide();
            if ((otherMiss < otherCan) && (otherMiss > 0))
                return false;
            return true;
        }

        public int GetMissionariesOnOtherSide()
        {
            return NumOfEachAtStart - nMiss;
        }
        public int GetCannibalsOnOtherSide()
        {
            return NumOfEachAtStart - nCan;
        }

        public int GetHashCode(State state)
        {
            int hCode = state.Cannibals ^ state.Missionaries * 100 
                ^ state.BoatOnTheSide.GetHashCode() * 10000;
            return hCode.GetHashCode();
        }

        public static void SetNumberOfEach(int number)
        {
            NumOfEachAtStart = number;
        }
    }

    public class StateEqualityComparer : IEqualityComparer<State>
    {

        public bool Equals(State a, State b)
        {
            return a.Equals(b);
        }

        public int GetHashCode(State state)
        {
            int hCode = state.Cannibals ^ state.Missionaries*100 ^ state.BoatOnTheSide.GetHashCode()*10000;
            return hCode.GetHashCode();
        }
    }

    /// <summary>
    /// It's education and we can use static Fabric.
    /// Do not use this approach in enterprise
    /// </summary>
    public class StateFactory
    {
        private static int numberOfEach = 3;
        public static int NumberOfEach
        {
            get { return numberOfEach; }
            set { if (value > 0) numberOfEach = value; }
        }
        public static void SetNumberOfEach(int number)
        {
            if(number > 0) numberOfEach = number;
        }

        public static State Make(int nMiss, int nCan, BoatState BoatOnTheSide,
                    int stateLevel=0, int Cost=0)
        {
            return new State(nMiss, nCan, BoatOnTheSide, stateLevel, numberOfEach, Cost);
        }

        public static State Make(int nMiss, int nCan, BoatState BoatOnTheSide,
                    State PrevState, int stateLevel = 0, int Cost = 0)
        {
            return new State(nMiss, nCan, BoatOnTheSide, PrevState, stateLevel, numberOfEach, Cost);
        }
    }
}
