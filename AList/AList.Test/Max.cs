using System;
using NUnit.Framework;

namespace AList.Test
{
    [TestFixture]
    public class Max
    {
        [Test]
        public void MaxOnEmptyList()
        {
            //arrange - act - assert
            Assert.That(() => new InterestingList<int>().Max, Throws.InvalidOperationException);
        }
        
        [Test]
        public void MaxOnListWithOneElement()
        {
            //arrange - act - assert
            Assert.AreEqual(100, new InterestingList<double>() {100}.Max);
        }
        
        [Test]
        public void MaxOnList()
        {
            //arrange - act - assert
            Assert.AreEqual(5.33, new InterestingList<double>() {5.3, 2.3, -0.777, 5.33, 0, -30}.Max);
        }
    }
}