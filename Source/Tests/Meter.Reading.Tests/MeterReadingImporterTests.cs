using Meter.Reading.Application.Business;
using NUnit.Framework;

namespace Meter.Reading.Tests
{
    [TestFixture]
    [Description("Test class to validate the HL7 message parsing assumptions")]
    public class Tests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            //Mock
            //readingImporter = new MeterReadingImporter();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}