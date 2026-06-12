using System.Linq;
using NUnit.Framework;
using Zero53.GameplayTags;

namespace Zero53.Tests
{
    public class TagTests
    {
        [Test]
        public void Tag_Empty_ShouldBeInvalid()
        {
            var tag = new Tag("");
            Assert.IsTrue(tag.isEmpty);
            Assert.IsFalse(tag.isValid);
        }

        [Test]
        public void Tag_ValidFormat_ShouldBeValid()
        {
            Tag tag = "Skill.Fire.Damage";
            Assert.IsTrue(tag.isValid);
            Assert.AreEqual(3, tag.layer);
        }

        [Test]
        public void Tag_InvalidFormat_ShouldBeInvalid()
        {
            Assert.IsFalse(new Tag("Skill..Fire").isValid);
            Assert.IsFalse(new Tag(".Skill.Fire").isValid);
            Assert.IsFalse(new Tag("Skill.Fire.").isValid);
            Assert.IsFalse(new Tag("Skill Fire").isValid);
            Assert.IsFalse(new Tag("Skill@Fire").isValid);
        }

        [Test]
        public void Equals_SameTag_ShouldBeEqual()
        {
            Tag a = "Skill.Fire";
            Tag b = "Skill.Fire";
            Assert.AreEqual(a, b);
            Assert.IsTrue(a == b);
        }

        [Test]
        public void Equals_DifferentTag_ShouldNotBeEqual()
        {
            Tag a = "Skill.Fire";
            Tag b = "Skill.Ice";
            Assert.AreNotEqual(a, b);
            Assert.IsTrue(a != b);
        }

        [Test]
        public void Matches_ChildTagMatchesParent_ShouldReturnTrue()
        {
            Tag parent = "Skill.Fire";
            Tag child = "Skill.Fire.Damage";

            Assert.IsTrue(child.Matches(parent));
            Assert.IsFalse(parent.Matches(child));
        }

        [Test]
        public void Matches_SameTag_ShouldReturnTrue()
        {
            Tag a = "Skill.Fire";
            Tag b = "Skill.Fire";
            Assert.IsTrue(a.Matches(b));
        }

        [Test]
        public void MatchesExact_OnlyTrueWhenExactlySame()
        {
            Tag parent = "Skill.Fire";
            Tag child = "Skill.Fire.Damage";

            Assert.IsFalse(child.MatchesExact(parent));
            Assert.IsTrue(parent.MatchesExact(parent));
        }

        [Test]
        public void IsChildOf_DirectChild_ShouldReturnTrue()
        {
            Tag parent = "Skill.Fire";
            Tag child = "Skill.Fire.Damage";
            Tag unrelated = "Skill.Ice";

            Assert.IsTrue(child.IsChildOf(parent));
            Assert.IsFalse(child.IsChildOf(unrelated));
        }

        [Test]
        public void IsParentOf_DirectParent_ShouldReturnTrue()
        {
            Tag parent = "Skill.Fire";
            Tag child = "Skill.Fire.Damage";

            Assert.IsTrue(parent.IsParentOf(child));
        }

        [Test]
        public void GetParents_ReturnsFullHierarchy()
        {
            Tag tag = "A.B.C.D";
            var parents = tag.GetParents().ToList();

            Assert.AreEqual(3, parents.Count);
            Assert.AreEqual((Tag)"A", parents[0]);
            Assert.AreEqual((Tag)"A.B", parents[1]);
            Assert.AreEqual((Tag)"A.B.C", parents[2]);
        }

        [Test]
        public void GetParents_SingleLayer_ReturnsEmpty()
        {
            Tag tag = "Skill";
            var parents = tag.GetParents().ToList();
            Assert.AreEqual(0, parents.Count);
        }

        [Test]
        public void CompareTo_SortByLayerAndName()
        {
            Tag a = "A";
            Tag b = "A.B";
            Tag c = "B";

            Assert.Less(a.CompareTo(b), 0);
            Assert.Less(a.CompareTo(c), 0);
            Assert.Less(b.CompareTo(c), 0);
        }

        [Test]
        public void ImplicitStringConversion_WorksCorrectly()
        {
            Tag tag = "Skill.Fire";
            Assert.AreEqual("Skill.Fire", tag.ToString());
        }
    }
}