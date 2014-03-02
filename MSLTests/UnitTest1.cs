using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MaxSDll;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSLTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1() {
            var a = new[,] {{1.7, 0.8, 0.9}, {0.8, 0.7, 0.3}, {0.9, 0.3, 1.7}};
            var b = 2.88474;

            MaxSdResult s;
            MaxSd.Method(a, out s);
            var c = s.SobstvMax;
            Assert.IsTrue(Math.Abs(c-b)<0.1);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var a = new[,] { { 1.0, 0, 0 }, { 0, 3, 0 }, { 0, 0, 2 } };
            var b = 2.9999;

            MaxSdResult s;
            MaxSd.Method(a, out s);
            var c = s.SobstvMax;
            Assert.IsTrue(Math.Abs(c - b) < 0.1);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var a = new[,] { { 1.0, 0, 0 }, { 0, 3, 0 }, { 0, 0, 2 } };
            var b = 2;

            MaxSdResult s;
            MaxSd.Method(a, out s);
            var c = s.Sobstv2;
            Assert.IsTrue(Math.Abs(c - b) < 0.1);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var a = new[,] { { 1.0, 0, 0 }, { 0, 3, 0 }, { 0, 0, 2 } };
            var b = new[] { 0, 1, 0 };

            MaxSdResult s;
            MaxSd.Method(a, out s);
            var c = s.SobstvVector;
            Assert.IsTrue(Enumerable.Range(0,b.Length).Any(i=>Math.Abs(c[i] - b[i]) < 0.1));
        }

        [TestMethod]
        public void TestMethod5()
        {
            var a = new[,] { { 5, 0, 0 }, { 0, 4.9, 0 }, { 0, 0, 3 } };
            var b = 4.9;

            MaxSdResult s;
            MaxSd.Method(a, out s);
            var c = s.SobstvMax;
            Assert.IsTrue(Math.Abs(c - b) < 0.1);
        }

        [TestMethod]
        public void TestMethod6()
        {
            var a = new[,] { { 5, 0, 0 }, { 0, 4.9, 0 }, { 0, 0, 3 } };
            var b = new[] { 0, 1, 0 };

            MaxSdResult s;
            MaxSd.Method(a, out s);
            var c = s.SobstvVector;
            Assert.IsTrue(Enumerable.Range(0, b.Length).Any(i => Math.Abs(c[i] - b[i]) < 0.1));
        }

        [TestMethod]
        public void TestMethod7()
        {
            var a = new[,] { { 5, 0, 0 }, { 0, 4.9, 0 }, { 0, 0, 3 } };
            var b = 4.9;

            MaxSdResult s;
            MaxSd.Method(a, out s);
            var c = s.SobstvMax;
            Assert.IsTrue(Math.Abs(c - b) < 0.1);
        }
    }
}
