using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestovi
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Kupovina k = new Kupovina("idKupca",DateTime.Now,DateTime.Now.AddDays(10));
        }
    }
}
