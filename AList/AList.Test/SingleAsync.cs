using System;
using NUnit.Framework;

namespace AList.Test
{
    [TestFixture]
    public class SingleAsync
    {
        [Test]
        public void SingleAsyncInEmptyList()
        {
            //arrange - act - assert
            Assert.Throws<InvalidOperationException>(delegate { new InterestingList<int>().SingleAsync(4); });
        }
        
        [Test]
        public void SingleAsyncWithRepeatValuesInList()
        {
            //arrange - act - assert
            Assert.Throws<AggregateException>(delegate { new InterestingList<int>(){1, 2, 2, 3}.SingleAsync(2); });
        }
        
        [Test]
        public void SingleAsyncValueDontContainInList()
        {
            //arrange - act - assert
            Assert.Throws<InvalidOperationException>(delegate { new InterestingList<int>(){1, 2, 2, 3}.SingleAsync(4); });
        }
        
        [Test]
        public void SingleAsyncInList()
        {
            //arrange
            var list = new InterestingList<double>() {10.3, 2.5, -2.8};
            double elementForSearch = 2.5;
            //- act
            int position = list.SingleAsync(2.5);
            //- assert
            Assert.AreEqual(list[position], elementForSearch);
        }
    }
}