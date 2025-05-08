namespace IntegrationTests
{
    public class IntegreationTests
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