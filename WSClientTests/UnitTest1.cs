using NUnit.Framework;
using UNDataWSClient.Services;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            UNDataSOAPService service = new UNDataSOAPService();
            service.SendQueryStructurRequest();
            Assert.Pass();
        }
    }
}