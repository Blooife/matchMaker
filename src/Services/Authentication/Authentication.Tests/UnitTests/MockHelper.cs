using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Language.Flow;

namespace Authentication.Tests.UnitTests;

public static class MockHelper
{
    public static ISetup<ILogger<T>> MockLog<T>(this Mock<ILogger<T>> logger, LogLevel level)
    {
        return logger.Setup(x => x.Log(
            level, 
            It.IsAny<EventId>(), 
            It.IsAny<It.IsAnyType>(), 
            It.IsAny<Exception>(), 
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()));
    }

    private static Expression<Action<ILogger<T>>> VerifyExpression<T>(LogLevel level)
    {
        return x => x.Log(
            level, 
            It.IsAny<EventId>(), 
            It.IsAny<It.IsAnyType>(), 
            It.IsAny<Exception>(), 
            It.IsAny<Func<It.IsAnyType, Exception?, string>>());
    }

    public static void VerifyLog<T>(this Mock<ILogger<T>> logger, LogLevel level, Times times)
    {
        logger.Verify(VerifyExpression<T>(level), times);
    }
}
