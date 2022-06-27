using System;
using NUnit.Framework;

namespace AList.Test
{
    [TestFixture]
    public class RemoveAt
    {
        [Test]
        public void RemoveAtFromEmptyList()
        {
            //arrange - act - assert
            Assert.Throws<IndexOutOfRangeException>(delegate { new InterestingList<int>().RemoveAt(0); });
        }
        
        [Test]
        public void RemoveAtWhereIndexGreaterThanLengthList()
        {
            //arrange - act - assert
            Assert.Throws<IndexOutOfRangeException>(delegate { new InterestingList<int>(){-3, 100, 0, 3}.RemoveAt(4); });
        }
        
        [Test]
        public void RemoveAtFromListWithNegativeIndex()
        {
            //arrange - act - assert
            Assert.Throws<IndexOutOfRangeException>(delegate { new InterestingList<int>(){2, 5, 10}.RemoveAt(-3); });
        }
        
        [Test]
        public void RemoveAtFromList()
        {
            //arrange
            int position = 1;
            var list = new InterestingList<int>() {2, 5, 10};
            int numberBeforeRemove = list[position];
            //act
            list.RemoveAt(position);
            //assert
            Assert.AreNotEqual(numberBeforeRemove, list[position]);
        }
    }
}