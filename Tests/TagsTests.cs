using NUnit.Framework;
using Zero53.GameplayTags;

namespace Zero53.Tests
{
    public class TagsTests
    {
        [Test]
        public void TestTagContainer()
        {
            var tagContainer = new TagContainer();
            Assert.True(tagContainer.isEmpty);

            tagContainer.Add("T1.T2.T3");

            Assert.True(tagContainer.Has("T1"));
            Assert.True(tagContainer.Has("T1.T2"));
            Assert.True(tagContainer.Has("T1.T2.T3"));
            Assert.True(tagContainer.HasExact("T1.T2.T3"));
            Assert.True(!tagContainer.HasExact("T1.T2"));

            tagContainer.Remove("T1");

            Assert.True(tagContainer.isEmpty);

            tagContainer.Add("T1.T2.T3");
            tagContainer.RemoveExact("T1");

            Assert.True(!tagContainer.isEmpty);

            tagContainer.Clear();

            Assert.True(tagContainer.isEmpty);
        }
    }

}