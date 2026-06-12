using Zero53.GameplayTags;
using NUnit.Framework;
using System.Linq;

namespace Zero53.Tests
{
    public class TagContainerTests
    {
        private TagContainer _container;
        private Tag _tagA;
        private Tag _tagB;
        private Tag _tagParentA;

        [SetUp]
        public void SetUp()
        {
            // 每次测试前创建全新容器
            _container = new TagContainer();

            _tagA = new Tag("ParentA.TagA");
            _tagB = new Tag("TagB");
            _tagParentA = new Tag("ParentA");
        }

        [Test]
        public void AddTag_ShouldIncreaseCount_AndTriggerEvent()
        {
            // 准备
            var added = false;
            _container.OnTagAdded += t => added = true;

            // 执行
            var result = _container.Add(_tagA);

            // 断言
            Assert.IsTrue(result);
            Assert.AreEqual(1, _container.count);
            Assert.IsTrue(added);
        }

        [Test]
        public void AddDuplicateTag_ShouldNotAdd()
        {
            // 执行
            _container.Add(_tagA);
            var result = _container.Add(_tagA);

            // 断言
            Assert.IsFalse(result);
            Assert.AreEqual(1, _container.count);
        }

        [Test]
        public void HasExact_OnlyReturnTrue_WhenTagIsExactlyPresent()
        {
            _container.Add(_tagA);

            Assert.IsTrue(_container.HasExact(_tagA));
            Assert.IsFalse(_container.HasExact(_tagParentA));
        }

        [Test]
        public void Has_InheritedTag_ShouldReturnTrue()
        {
            _container.Add(_tagA);

            // 如果 TagA 继承 ParentA，则 Has(ParentA) 应该为 true
            Assert.IsTrue(_container.Has(_tagParentA));
        }

        [Test]
        public void Remove_ShouldRemoveAndTriggerEvent()
        {
            var removed = false;
            _container.OnTagRemoved += t => removed = true;

            _container.Add(_tagA);
            var count = _container.Remove(_tagA);

            Assert.AreEqual(1, count);
            Assert.AreEqual(0, _container.count);
            Assert.IsTrue(removed);
        }

        [Test]
        public void RemoveExact_OnlyRemoveExactMatch()
        {
            _container.Add(_tagA);
            var success = _container.RemoveExact(_tagA);

            Assert.IsTrue(success);
            Assert.AreEqual(0, _container.count);
        }

        [Test]
        public void Clear_ShouldEmptyAllTags()
        {
            _container.Add(_tagA);
            _container.Add(_tagB);
            _container.Clear();

            Assert.IsTrue(_container.isEmpty);
            Assert.AreEqual(0, _container.count);
        }

        [Test]
        public void Append_BatchAddTags()
        {
            var added = _container.Append(new[] { _tagA, _tagB, _tagA });

            Assert.AreEqual(2, added);
            Assert.AreEqual(2, _container.count);
        }

        [Test]
        public void HasAll_CheckAllTagsExist()
        {
            _container.Add(_tagA);
            _container.Add(_tagB);

            Assert.IsTrue(_container.HasAll(new[] { _tagA, _tagB }));
            Assert.IsFalse(_container.HasAll(new[] { _tagA, new Tag("Missing") }));
        }

        [Test]
        public void HasAny_CheckAnyTagExist()
        {
            _container.Add(_tagA);

            Assert.IsTrue(_container.HasAny(new[] { _tagB, _tagA }));
        }

        [Test]
        public void Intersect_ReturnsCommonTags()
        {
            var c1 = new TagContainer(_tagA, _tagB);
            var c2 = new TagContainer(_tagA, new Tag("C"));

            var result = new TagContainer(c1.Intersect(c2).ToArray());

            Assert.AreEqual(1, result.count);
            Assert.IsTrue(result.HasExact(_tagA));
        }

        [Test]
        public void Except_ReturnsOnlyInFirstContainer()
        {
            var c1 = new TagContainer(_tagA, _tagB);
            var c2 = new TagContainer(_tagA);

            var diff = new TagContainer(c1.Except(c2).ToArray());

            Assert.AreEqual(1, diff.count);
            Assert.IsTrue(diff.HasExact(_tagB));
        }

        [Test]
        public void GetChildren_ReturnsTagsMatchingParent()
        {
            _container.Add(_tagA); // 继承 ParentA
            var children = _container.GetChildren(_tagParentA).ToList();

            Assert.AreEqual(1, children.Count);
        }
    }
}