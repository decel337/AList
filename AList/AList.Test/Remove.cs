using System;
using NUnit.Framework;

namespace AList.Test
{
    [TestFixture]
    public class Remove
    {
        [Test]
        public void RemoveElementFromEmptyList()
        {
            //arrange-act-assert
            Assert.Throws<InvalidOperationException>(delegate { new InterestingList<int>().Remove(10);});
        }
        
        [Test]
        public void RemoveElementThatDoesNotExist()
        {
            //arrange
            var list1 = new InterestingList<double>() {5.5, -3.5, 0, 100};
            var list2 = new InterestingList<double>() {5.5, -3.5, 0, 100};
            //act
            list1.Remove(1000);
            //assert
            Assert.AreEqual(list1, list2);
        }
        
        [Test]
        public void RemoveElementThatDoesExist()
        {
            //arrange
            var list1 = new InterestingList<double>() {5.5, -3.5, 0, 100};
            var list2 = new InterestingList<double>() {5.5, -3.5, 0, 100};
            //act
            list1.Remove(0);
            //assert
            Assert.AreNotEqual(list1, list2);
        }
        
        [Test]
        public void RemoveElementWhichRepeat()
        {
            //arrange
            double elementForRemove = 100;
            var list1 = new InterestingList<double>() {5.5, -3.5, 0, 100, 100};
            //act
            list1.Remove(elementForRemove);
            //assert
            StringAssert.Contains(elementForRemove.ToString(), list1.ToString());
        }
    }
}