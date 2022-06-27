using System;
using NUnit.Framework;

namespace AList.Test
{
    [TestFixture]
    public class AddAt
    {
        [Test]
        public void AddAtInEmptyList()
        {
            //arrange - act - assert
            Assert.Throws<IndexOutOfRangeException>(delegate { new InterestingList<int>().AddAt(0, 10); });
        }
        
        [Test]
        public void AddAtWhereIndexGreaterThanLengthList()
        {
            //arrange - act - assert
            Assert.Throws<IndexOutOfRangeException>(delegate { new InterestingList<int>(){-3, 100, 0, 3}.AddAt(4, 10); });
        }
        
        [Test]
        public void AddAtInListWithNegativeIndex()
        {
            //arrange - act - assert
            Assert.Throws<IndexOutOfRangeException>(delegate { new InterestingList<int>(){2, 5, 10}.AddAt(-3, -30); });
        }
        
        [Test]
        public void AddAtInList()
        {
            //arrange
            int numberForAdd = -30;
            int position = 0;
            var list = new InterestingList<int>() {2, 5, 10};
            //act
            list.AddAt(position, numberForAdd);
            //assert
            Assert.AreEqual(numberForAdd, list[position]);
        }
    }
}