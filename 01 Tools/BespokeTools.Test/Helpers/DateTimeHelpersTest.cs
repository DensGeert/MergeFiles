using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Protime.Bespoke.Tools.Helpers;

namespace Protime.Bespoke.Tools.Test.Helpers
{
    [TestClass]
    public class DateTimeHelpersTest
    {
        [TestMethod]
        public void SplitDateRange_dayChunkIs0_return1DateRange()
        {
            var dateFrom = new DateTime(2020, 5, 12);
            var dateTo = new DateTime(2020, 5, 15);
            int splitByDays = 0;

            var singleRange = DateTimeHelpers.SplitDateRange(dateFrom, dateTo, dayChunkSize: splitByDays).ToList();
            Assert.IsNotNull(singleRange);
            Assert.AreEqual(1, singleRange.Count);
            Assert.AreEqual("20200512", singleRange[0].Item1.ToString("yyyyMMdd"));
            Assert.AreEqual("20200515", singleRange[0].Item2.ToString("yyyyMMdd"));
        }

        [TestMethod]
        public void SplitDateRange_dayChunkIs1_return1DateRange()
        {
            var dateFrom = new DateTime(2020, 5, 12);
            var dateTo = new DateTime(2020, 5, 15);
            int splitByDays = 1;

            var singleRange = DateTimeHelpers.SplitDateRange(dateFrom, dateTo, dayChunkSize: splitByDays).ToList();
            Assert.IsNotNull(singleRange);
            Assert.AreEqual(1, singleRange.Count);
            Assert.AreEqual("20200512", singleRange[0].Item1.ToString("yyyyMMdd"));
            Assert.AreEqual("20200515", singleRange[0].Item2.ToString("yyyyMMdd"));
        }

        [TestMethod]
        public void SplitDateRange_dayChunkIs2_returnDateRanges()
        {
            var dateFrom = new DateTime(2020, 5, 12);
            var dateTo = new DateTime(2020, 5, 15);
            int splitByDays = 2;

            var singleRange = DateTimeHelpers.SplitDateRange(dateFrom, dateTo, dayChunkSize: splitByDays).ToList();
            Assert.IsNotNull(singleRange);
            Assert.AreEqual(2, singleRange.Count);
            Assert.AreEqual("20200512", singleRange[0].Item1.ToString("yyyyMMdd"));
            Assert.AreEqual("20200513", singleRange[0].Item2.ToString("yyyyMMdd"));
            Assert.AreEqual("20200514", singleRange[1].Item1.ToString("yyyyMMdd"));
            Assert.AreEqual("20200515", singleRange[1].Item2.ToString("yyyyMMdd"));
        }

        [TestMethod]
        public void SplitDateRange_dayChunkIs3_return2DateRanges()
        {
            var dateFrom = new DateTime(2020, 5, 12);
            var dateTo = new DateTime(2020, 5, 15);
            int splitByDays = 3;

            var singleRange = DateTimeHelpers.SplitDateRange(dateFrom, dateTo, dayChunkSize: splitByDays).ToList();
            Assert.IsNotNull(singleRange);
            Assert.AreEqual(2, singleRange.Count);
            Assert.AreEqual("20200512", singleRange[0].Item1.ToString("yyyyMMdd"));
            Assert.AreEqual("20200514", singleRange[0].Item2.ToString("yyyyMMdd"));
            Assert.AreEqual("20200515", singleRange[1].Item1.ToString("yyyyMMdd"));
            Assert.AreEqual("20200515", singleRange[1].Item2.ToString("yyyyMMdd"));
        }

        [TestMethod]
        public void SplitDateRange_dayChunkIs3_return1DateRanges()
        {
            var dateFrom = new DateTime(2020, 5, 12);
            var dateTo = new DateTime(2020, 5, 14);
            int splitByDays = 3;

            var singleRange = DateTimeHelpers.SplitDateRange(dateFrom, dateTo, dayChunkSize: splitByDays).ToList();
            Assert.IsNotNull(singleRange);
            Assert.AreEqual(1, singleRange.Count);
            Assert.AreEqual("20200512", singleRange[0].Item1.ToString("yyyyMMdd"));
            Assert.AreEqual("20200514", singleRange[0].Item2.ToString("yyyyMMdd"));
        }

        [TestMethod]
        public void SplitDateRange_dayChunkIs0AndDateIsOneDay_return1DateRange()
        {
            var dateFrom = new DateTime(2020, 5, 12);
            var dateTo = new DateTime(2020, 5, 12);
            int splitByDays = 0;

            var singleRange = DateTimeHelpers.SplitDateRange(dateFrom, dateTo, dayChunkSize: splitByDays).ToList();
            Assert.IsNotNull(singleRange);
            Assert.AreEqual(1, singleRange.Count);
            Assert.AreEqual("20200512", singleRange[0].Item1.ToString("yyyyMMdd"));
            Assert.AreEqual("20200512", singleRange[0].Item2.ToString("yyyyMMdd"));
        }

        [TestMethod]
        public void SplitDateRange_dayChunkIs1AndDateIsOneDay_return1DateRange()
        {
            var dateFrom = new DateTime(2020, 5, 12);
            var dateTo = new DateTime(2020, 5, 12);
            int splitByDays = 1;

            var singleRange = DateTimeHelpers.SplitDateRange(dateFrom, dateTo, dayChunkSize: splitByDays).ToList();
            Assert.IsNotNull(singleRange);
            Assert.AreEqual(1, singleRange.Count);
            Assert.AreEqual("20200512", singleRange[0].Item1.ToString("yyyyMMdd"));
            Assert.AreEqual("20200512", singleRange[0].Item2.ToString("yyyyMMdd"));
        }

        [TestMethod]
        public void SplitDateRange_dayChunkIs2AndDateIsOneDay_return1DateRange()
        {
            var dateFrom = new DateTime(2020, 5, 12);
            var dateTo = new DateTime(2020, 5, 12);
            int splitByDays = 2;

            var singleRange = DateTimeHelpers.SplitDateRange(dateFrom, dateTo, dayChunkSize: splitByDays).ToList();
            Assert.IsNotNull(singleRange);
            Assert.AreEqual(1, singleRange.Count);
            Assert.AreEqual("20200512", singleRange[0].Item1.ToString("yyyyMMdd"));
            Assert.AreEqual("20200512", singleRange[0].Item2.ToString("yyyyMMdd"));
        }
    }
}
