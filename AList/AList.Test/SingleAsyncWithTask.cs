using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AList.Test
{
    [TestFixture]
    public class SingleAsyncWithTask
    {
        [Test]
        public void SingleAsyncInEmptyList()
        {
            //arrange - act - assert
            Assert.Throws<InvalidOperationException>(delegate { new InterestingList<int>().SingleAsyncWithTask(4); });
        }
        
        [Test]
        public void SingleAsyncWithRepeatValuesInList()
        {
            //arrange - act - assert
            Assert.Throws<AggregateException>(delegate { new InterestingList<int>(){1, 2, 2, 3}.SingleAsyncWithTask(2); });
        }
        
        [Test]
        public void SingleAsyncValueDontContainInList()
        {
            //arrange - act - assert
            Assert.Throws<InvalidOperationException>(delegate { new InterestingList<int>(){1, 2, 2, 3}.SingleAsyncWithTask(4); });
        }
        
        [Test]
        public void SingleAsyncInList()
        {
            //arrange
            var lst = new InterestingList<double>();
            for (int i = -1000; i < 10000; i+=10)
            {
                lst.Add(i);
            }
            double elementForSearch = 0;
            //- act
            int position = lst.SingleAsyncWithTask(elementForSearch);
            //- assert
            Assert.AreEqual(lst[position], elementForSearch);
        }
    }
}