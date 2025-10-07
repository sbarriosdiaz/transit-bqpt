////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System;
using Bqpt.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ET.Identity.Tests
{
    [TestClass()]
    public class DateServicesTests
    {
        [TestMethod()]
        public void GetNumberOfWorkingDaysTest()
        {
            var date1 = DateTime.Now;
            var date2 = date1.AddDays(3);

            var workingDays = DateServices.GetNumberOfWorkingDays(date1, date2);

            Assert.IsTrue(workingDays > 1);
        }

        [TestMethod()]
        public void IsFederalHolidayTest()
        {
            var date = new DateTime(2020, 12, 1);
            var date2 = new DateTime(2020, 12, 25);
            var dtNow = DateServices.DtNow;

            Assert.IsTrue(dtNow == DateTime.Now);

            // assert failure prove the work on a false return of the function
            Assert.IsTrue(!DateServices.IsFederalHoliday(date));

            // assert is true that date is a federal holiday
            Assert.IsTrue(DateServices.IsFederalHoliday(date2));
        }
    }
}