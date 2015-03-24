using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissAndCan;

namespace UnitTestProject
{
    /// <summary>
    /// Test for BoatState
    /// </summary>
    [TestClass]
    public class UnitTestEnum
    {
        [TestMethod]
        public void TestBoatStateInverse()
        {
            var b = BoatState.Right;
            Assert.AreEqual<BoatState>(BoatState.Left, b.Inverse());
            var c = BoatState.Left;
            Assert.AreEqual<BoatState>(BoatState.Right, c.Inverse());
        }

        [TestMethod]
        public void TestBoatStateMultiplier()
        {
            var b = BoatState.Right;
            Assert.AreEqual<int>(1, b.AsMultiplier());
            var c = BoatState.Left;
            Assert.AreEqual<int>(-1, c.AsMultiplier());
        }
    }
}
