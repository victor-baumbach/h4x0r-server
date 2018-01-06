using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace h4x0r
{
    [TestClass]
    public class FileTests
    {
        [TestMethod]
        public void TestFileFactory()
        {
            File file = FileFactory.Create(File.Type.Generic);
            Assert.IsNotNull(file);
        }

        [TestMethod]
        public void TestFileFactoryAll()
        {
            foreach (File.Type fileType in Enum.GetValues(typeof(File.Type)))
            {
                File file = FileFactory.Create(fileType);
                Assert.IsNotNull(file);
            }
        }
    }
}
