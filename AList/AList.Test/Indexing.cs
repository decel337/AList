using System;
using NUnit.Framework;

namespace AList.Test
{
    [TestFixture]
    public class Indexing
    {
        [Test]
        public void NegativeIndexInList()
        {
            //arrange
            var list = new InterestingList<int>() {1, 2, 3, 4, 6};
            
            //act - assert
            Assert.Throws<IndexOutOfRangeException>(delegate {
                 Console.WriteLine(list[-1]);
            });
        }
        
        [Test]
        public void IndexThatGreaterThanLengthList()
        {
            //arrange
            var list = new InterestingList<int>() {1, 2, 3, 4, 6};
            
            //act - assert
            Assert.Throws<IndexOutOfRangeException>(delegate {
                Console.WriteLine(list[5]);
            });
        }
        
        [Test]
        public void IndexInList()
        {
            //arrange
            var list = new InterestingList<int>() {1, 2, 3, 4, 6};
            
            //act - assert
            Assert.AreEqual(3, list[2]);
        }
    }
}