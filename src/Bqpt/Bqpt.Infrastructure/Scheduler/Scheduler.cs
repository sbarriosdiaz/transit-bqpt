////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Bqpt.Infrastructure
{
    public class AppScheduler
    {
        protected AppScheduler()
        {
        }

        public static async Task Start()
        {
            var fileValidatorJob = JobBuilder.Create<FileValidation>()
                                               .WithIdentity("fileValidationJob", "groupFileManagement")
                                               .Build();

            var triggerForfileValidatorJob = TriggerBuilder.Create()
                                                            .WithIdentity("triggerFoFileValidationJob", "groupFileManagement")
                                                            .StartNow()
                                                            .WithSimpleSchedule(x => x
                                                                .WithIntervalInMinutes(2)
                                                                .RepeatForever())
                                                            .Build();

            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();
            await scheduler.ScheduleJob(fileValidatorJob, triggerForfileValidatorJob);
        }
    }
}