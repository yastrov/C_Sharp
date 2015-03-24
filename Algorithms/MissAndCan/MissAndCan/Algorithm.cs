using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissAndCan
{
    public static class Algorithm
    {
        public static State UniformCostSearch(Problem problem)
        {
            State result = null;
            var comparer = problem.GetComparer();
            var visited = new HashSet<State>(comparer);
            // It's need other collection as it possible! Red Black Tree for example.
            List<State> fringe = new List<State>();
            fringe.Add(problem.GetInitialState());
            while(fringe.Count != 0)
            {
                // If we use heuristic function with problem.GetCostOfActions(q), we can take A* algorythm:).
                // It's need other collection as it possible!
                var currNode = fringe.OrderBy(q => problem.GetCostOfActions(q)).First();
                fringe.Remove(currNode);
                if (problem.IsGoal(currNode))
                {
                    result = currNode;
                    break;
                }
                if (!visited.Contains(currNode))
                {
                    visited.Add(currNode);
                    foreach(var node in problem.GetSuccessors(currNode))
                    {
                        // It's not good idea, but I have no more best solution on this time!
                        fringe.Add(node);
                    }
                }
            }
            return result;
        }
    }
}
