using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.Equivalents.Globalization;

namespace NetWebScript.Test.Material.Tests.Globalization
{
    /// <summary>
    /// <see cref="DateTimeFormat"/> and <see cref="DateTimeParseFormat"/> unit tests.
    /// </summary>
    [TestClass]
    [ScriptAvailable]
    public class DateTimeFormatTest
    {
        [TestMethod]
        public void CreateDateParseRegExp()
        {
            // Default format
            var format = DateTimeFormat.CreateDateParseRegExp(DateTimeFormatInfo.InvariantInfo, null);
            Assert.AreEqual(@"^(\d\d?)(\/)(\d\d?)(\/)(\d{4})\s+(\d\d?):(\d\d?):(\d\d?)$", format.regexp);
            Assert.AreEqual("MM|/|dd|/|yyyy|HH|mm|ss", string.Join("|", format.groups));

            // Standards formats
            format = DateTimeFormat.CreateDateParseRegExp(DateTimeFormatInfo.InvariantInfo, "f");
            Assert.AreEqual(@"^(\D+),\s+(\d\d?)\s+(\D+)\s+(\d{4})\s+(\d\d?):(\d\d?)$", format.regexp);
            Assert.AreEqual("dddd|dd|MMMM|yyyy|HH|mm", string.Join("|", format.groups));

            format = DateTimeFormat.CreateDateParseRegExp(DateTimeFormatInfo.InvariantInfo, "F");
            Assert.AreEqual(@"^(\D+),\s+(\d\d?)\s+(\D+)\s+(\d{4})\s+(\d\d?):(\d\d?):(\d\d?)$", format.regexp);
            Assert.AreEqual("dddd|dd|MMMM|yyyy|HH|mm|ss", string.Join("|", format.groups));

            format = DateTimeFormat.CreateDateParseRegExp(DateTimeFormatInfo.InvariantInfo, "d");
            Assert.AreEqual(@"^(\d\d?)(\/)(\d\d?)(\/)(\d{4})$", format.regexp);
            Assert.AreEqual("MM|/|dd|/|yyyy", string.Join("|", format.groups));

            format = DateTimeFormat.CreateDateParseRegExp(DateTimeFormatInfo.InvariantInfo, "D");
            Assert.AreEqual(@"^(\D+),\s+(\d\d?)\s+(\D+)\s+(\d{4})$", format.regexp);
            Assert.AreEqual("dddd|dd|MMMM|yyyy", string.Join("|", format.groups));

            format = DateTimeFormat.CreateDateParseRegExp(DateTimeFormatInfo.InvariantInfo, "t");
            Assert.AreEqual(@"^(\d\d?):(\d\d?)$", format.regexp);
            Assert.AreEqual("HH|mm", string.Join("|", format.groups));

            format = DateTimeFormat.CreateDateParseRegExp(DateTimeFormatInfo.InvariantInfo, "T");
            Assert.AreEqual(@"^(\d\d?):(\d\d?):(\d\d?)$", format.regexp);
            Assert.AreEqual("HH|mm|ss", string.Join("|", format.groups));

            format = DateTimeFormat.CreateDateParseRegExp(DateTimeFormatInfo.InvariantInfo, "g");
            Assert.AreEqual(@"^(\d\d?)(\/)(\d\d?)(\/)(\d{4})\s+(\d\d?):(\d\d?)$", format.regexp);
            Assert.AreEqual("MM|/|dd|/|yyyy|HH|mm", string.Join("|", format.groups));

            format = DateTimeFormat.CreateDateParseRegExp(DateTimeFormatInfo.InvariantInfo, "G");
            Assert.AreEqual(@"^(\d\d?)(\/)(\d\d?)(\/)(\d{4})\s+(\d\d?):(\d\d?):(\d\d?)$", format.regexp);
            Assert.AreEqual("MM|/|dd|/|yyyy|HH|mm|ss", string.Join("|", format.groups));

            format = DateTimeFormat.CreateDateParseRegExp(DateTimeFormatInfo.InvariantInfo, "M");
            Assert.AreEqual(@"^(\D+)\s+(\d\d?)$", format.regexp);
            Assert.AreEqual("MMMM|dd", string.Join("|", format.groups));

            format = DateTimeFormat.CreateDateParseRegExp(DateTimeFormatInfo.InvariantInfo, "Y");
            Assert.AreEqual(@"^(\d{4})\s+(\D+)$", format.regexp);
            Assert.AreEqual("yyyy|MMMM", string.Join("|", format.groups));

            format = DateTimeFormat.CreateDateParseRegExp(DateTimeFormatInfo.InvariantInfo, "s");
            Assert.AreEqual(@"^(\d{4})\-(\d\d?)\-(\d\d?)T(\d\d?):(\d\d?):(\d\d?)$", format.regexp);
            Assert.AreEqual("yyyy|MM|dd|HH|mm|ss", string.Join("|", format.groups));
        }

        [TestMethod]
        public void DateTimeParseFormat_Parse()
        {
            var format = DateTimeFormat.CreateDateParseRegExp(DateTimeFormatInfo.InvariantInfo, null);

            var date = format.Parse("01/01/1970 00:00:00", DateTimeFormatInfo.InvariantInfo, true);
            Assert.AreEqual(0, date.GetTime());

            date = format.Parse("01/13/2012 09:11:00", DateTimeFormatInfo.InvariantInfo, true);
            Assert.AreEqual(1326445860000, date.GetTime());

            // Incorrect format
            date = format.Parse("01/13/2012 09:11", DateTimeFormatInfo.InvariantInfo, true);
            Assert.AreEqual(null, date);

            // Month overflow
            date = format.Parse("13/01/2012 09:11:00", DateTimeFormatInfo.InvariantInfo, true);
            Assert.AreEqual(null, date);
        }
    }
}
