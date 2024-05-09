namespace Tests.Integration.Internal
{
    public class AuthServiceTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [TearDown]
        public void Teardown()
        {

        }

#region LoginAsync Tests:

        [Test]
        public async Task LoginAsync_Fails_WhenDTONull()
        {
            Assert.Pass();
        }

        #endregion
    }
}