using System.Linq.Expressions;
using Hangfire;
using Tiktok.API.Domain.Services;

namespace Tiktok.ScheduledJob.Services;

public class ScheduledJobService : IScheduleService
{
    private readonly IBackgroundJobClient _backgroundJob;

    public ScheduledJobService(IBackgroundJobClient backgroundJob)
    {
        _backgroundJob = backgroundJob;
    }

    public string Enqueue(Expression<Action> functionCall)
    {
        return _backgroundJob.Enqueue(functionCall);
    }

    public string Enqueue<T>(Expression<Action<T>> functionCall)
    {
        return _backgroundJob.Enqueue<T>(functionCall);
    }

    public string Schedule(Expression<Action> functionCall, TimeSpan delay)
    {
        return _backgroundJob.Schedule(functionCall, delay);
    }

    public string Schedule<T>(Expression<Action<T>> functionCall, TimeSpan delay)
    {
        return _backgroundJob.Schedule<T>(functionCall, delay);
    }

    public string Schedule(Expression<Action> functionCall, DateTimeOffset enqueueAt)
    {
        return _backgroundJob.Schedule(functionCall, enqueueAt);
    }

    public string ContinueQueueWith(string parentJobId, Expression<Action> functionCall)
    {
        return _backgroundJob.ContinueJobWith(parentJobId, functionCall);
    }

    public bool Delete(string jobId)
    {
        return _backgroundJob.Delete(jobId);
    }

    public bool Requeue(string jobId)
    {
        return _backgroundJob.Requeue(jobId);
    }
}