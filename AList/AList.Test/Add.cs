using System.Collections;
using NUnit.Framework;

namespace AList.Test
{
    [TestFixture]
    public class Add
    {
        [Test]
        public void AddInEmptyList()
        {
            //arrange
            var list = new InterestingList<decimal>();
            //act
            list.Add(3);
            //asert
            Assert.IsNotEmpty(list);
        }
        
        [Test]
        public void AddInList()
        {
            //arrange
            var list = new InterestingList<decimal>(){2, 3, 5};
            var elementForAdd = 10;
            //act
            list.Add(elementForAdd);
            //asert
            StringAssert.Contains(elementForAdd.ToString(), list.ToString());
        }
    }
}