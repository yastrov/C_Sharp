using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MissAndCan;

namespace UnitTestProject
{
    /// <summary>
    /// Test for State class.
    /// It's not full cover test, but it's Ok for this solution.
    /// </summary>
    [TestClass]
    public class UnitTestForState
    {
        [TestMethod]
        public void TestStateValid()
        {
            var state = new State(-1, 4,BoatState.Left, 1);
            Assert.AreEqual<bool>(false, state.IsValidState());
            state = new State(2, -1, BoatState.Right, 1);
            Assert.AreEqual<bool>(false, state.IsValidState());
            state = new State(-2, -1, BoatState.Left, 1);
            Assert.AreEqual<bool>(false, state.IsValidState());
            state = new State(2, 2, BoatState.Left, 1);
            Assert.AreEqual<bool>(true, state.IsValidState());
            // nMis < (3-1) = 2
            state = new State(1, 2, BoatState.Left, 1);
            Assert.AreEqual<bool>(false, state.IsValidState());
        }

        [TestMethod]
        public void TestStateEquals()
        {
            var state1 = new MissAndCan.State(2, 2, BoatState.Left, 1);
            var state2 = new MissAndCan.State(2, 2, BoatState.Left, 1);
            Assert.AreEqual<bool>(true, state1.Equals(state2));
            var state3 = new MissAndCan.State(1, 2, BoatState.Left, 1);
            Assert.AreEqual<bool>(false, state1.Equals(state3));
            var state4 = new MissAndCan.State(2, 2, BoatState.Right, 1);
            Assert.AreEqual<bool>(false, state1.Equals(state4));
        }

        [TestMethod]
        public void TestStateContained()
        {
            var state1 = new MissAndCan.State(2, 2, BoatState.Left, 1);
            var state2 = new MissAndCan.State(2, 2, BoatState.Left, 1);
            var set = new System.Collections.Generic.HashSet<State>(new StateEqualityComparer());
            set.Add(state1);
            Assert.AreEqual<bool>(true, set.Contains(state2));
        }
        [TestMethod]
        public void TestStateContainedA()
        {
            var state1 = new MissAndCan.State(2, 2, BoatState.Left, 1);
            var state2 = new MissAndCan.State(2, 2, BoatState.Left, 1);
            var set = new System.Collections.Generic.HashSet<State>();
            set.Add(state1);
            Assert.AreEqual<bool>(false, set.Contains(state2));
        }

        [TestMethod]
        public void TestStateContainedB()
        {
            var state1 = new MissAndCan.State(2, 2, BoatState.Left, 1);
            var set = new System.Collections.Generic.HashSet<State>(new StateEqualityComparer());
            set.Add(state1);
            Assert.AreEqual<bool>(true, set.Contains(state1));
        }
    }
}
