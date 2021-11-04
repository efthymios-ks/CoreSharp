using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class TaskExtensionsTests
    {
        //Methods
        private static Task<TValue> GetValueAsync<TValue>(TValue value)
            => Task.FromResult(value);

        private static Task GetExceptionAsync(Exception ex)
            => Task.FromException(ex);

        [Test]
        public async Task WithAggregatedException_WhenAllSuccessfull_ReturnResponses()
        {
            //Arrange
            const int value1 = 1;
            const int value2 = 2;
            var task1 = GetValueAsync(value1);
            var task2 = GetValueAsync(value2);

            //Act
            await Task.WhenAll(task1, task2).WithAggregatedException();

            //Assert 
            task1.Result.Should().Be(value1);
            task2.Result.Should().Be(value2);
        }

        [Test]
        public async Task WithAggregatedException_WhenExceptionsOccure_ReturnAggregateException()
        {
            //Arrange 
            var exception1 = new ArgumentException("1");
            var exception2 = new ArgumentException("2");
            var task1 = GetExceptionAsync(exception1);
            var task2 = GetExceptionAsync(exception2);

            //Act
            Func<Task> action = async () => await Task.WhenAll(task1, task2).WithAggregatedException();

            //Assert 
            var assertion = await action.Should().ThrowExactlyAsync<AggregateException>();
            var aggregateException = assertion.Which;
            aggregateException.InnerExceptions.Count.Should().Be(2);
            aggregateException.InnerExceptions.Should().Contain(exception1);
            aggregateException.InnerExceptions.Should().Contain(exception2);
        }
    }
}
