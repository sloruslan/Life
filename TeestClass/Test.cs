using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextureGenerationMethods;

namespace TeestClass
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestArray()
        {
            byte[] array = new byte[] { 1, 0, 0, 1 };

            var res = TextureGeneration.GetTextureData(array, 2, 2, 2, 3);
        }
    }
}