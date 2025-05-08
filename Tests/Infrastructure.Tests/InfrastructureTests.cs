namespace Infrastructure.Tests
{
    public class InfrastructureTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.That(1 + 1, Is.EqualTo(2));
        }
    }
}