using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissAndCan
{
    public class Problem
    {
        private State _initialState;
        public State InitialState
        {
            get
            {
                return _initialState;
            }
            private set { ;}
        }
        private State _finishState;
        public State FinishState
        {
            get
            {
                return _finishState;
            }
            private set { ;}
        }
        private int _boatSize;
        public int BoatSize
        {
            get { return _boatSize; }
            private set { ;}
        }
        public Problem(State InitialState, State FinishState, int BoatSize = 2)
        {
            if (InitialState.IsValidState())
                _initialState = InitialState;
            else
                throw new Exception("Initial state is not valid!");
            if(FinishState.IsValidState())
                _finishState = FinishState;
            else
                throw new Exception("Finish state is not valid!");
            _boatSize = BoatSize;
            if(BoatSize <= 1)
                throw new Exception("Boat is very small!");
        }
        public bool IsGoal(State state)
        {
            return _finishState.Equals(state);
        }
        public State GetInitialState()
        {
            return _initialState;
        }

        public IEnumerable<State> GetSuccessors(State state)
        {
            var result = new List<State>();
            //loop through all possible combinations
            for (int nMiss = 0; nMiss <= _boatSize; nMiss++)
            {
                for (int nCan = 0; nCan <= _boatSize; nCan++)
                {
                    if (nMiss == 0 && nCan == 0)
                        continue;
                    if (nMiss + nCan > _boatSize)
                        break;
                    var BoatDirection = state.BoatOnTheSide.AsMultiplier();
                    var s = StateFactory.Make(
                        state.Missionaries + nMiss * BoatDirection,
                        state.Cannibals + nCan * BoatDirection,
                        state.BoatOnTheSide.Inverse(),
                        state,
                        state.StateLevel+1);
                    if(s.IsValidState())
                        result.Add(s);
                }
            }
            return result;
        }

        public int GetCostOfActions(State state)
        {
            return state.Cost;
            //int result = 0;
            //if (state.PrevState != null)
            //    result = GetCostOfActions(state.PrevState);
            //return result + 1;
        }

        public StateEqualityComparer GetComparer()
        {
            return new StateEqualityComparer();
        }
    }
}
