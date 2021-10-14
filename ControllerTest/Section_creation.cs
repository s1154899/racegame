using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace ControllerTest
{
    [TestFixture]
    class Section_creation
    {


        

        [Test]
        [TestCase(SectionTypes.Finish)]
        public void Section_Correct_section_type(SectionTypes sec)
        {

            Assert.AreEqual(new Section(sec).SectionType, sec);

        }

    }
}
