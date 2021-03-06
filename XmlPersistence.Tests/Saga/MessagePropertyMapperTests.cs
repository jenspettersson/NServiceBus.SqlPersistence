using System;
using System.Linq.Expressions;
using NServiceBus;
using NServiceBus.Persistence.Sql.Xml;
using NUnit.Framework;

[TestFixture]
public class MessagePropertyMapperTests
{

    [Test]
    public void StringCorrelationId()
    {
        Expression<Func<SagaDataWithStringCorrelationId, object>> expression;
        Assert.IsTrue(MessagePropertyMapper<SagaDataWithStringCorrelationId>.TryGetExpression(out expression));
        var instance = new SagaDataWithStringCorrelationId
        {
            CorrelationProperty = "Foo"
        };
        var property = expression.Compile()(instance);
        Assert.AreEqual("Foo", property);
    }

    public class SagaDataWithStringCorrelationId : ContainSagaData
    {
        [CorrelationId]
        public string CorrelationProperty { get; set; }
    }
    [Test]
    public void IntCorrelationId()
    {
        Expression<Func<SagaDataWithIntCorrelationId, object>> expression;
        Assert.IsTrue(MessagePropertyMapper<SagaDataWithIntCorrelationId>.TryGetExpression(out expression));
        var instance = new SagaDataWithIntCorrelationId
        {
            CorrelationProperty = 10
        };
        var property = expression.Compile()(instance);
        Assert.AreEqual(10, property);
    }

    public class SagaDataWithIntCorrelationId : ContainSagaData
    {
        [CorrelationId]
        public int CorrelationProperty { get; set; }
    }
    [Test]
    public void GuidCorrelationId()
    {
        Expression<Func<SagaDataWithGuidCorrelationId, object>> expression;
        Assert.IsTrue(MessagePropertyMapper<SagaDataWithGuidCorrelationId>.TryGetExpression(out expression));
        var guid = Guid.NewGuid();
        var instance = new SagaDataWithGuidCorrelationId
        {
            CorrelationProperty = guid
        };
        var property = expression.Compile()(instance);
        Assert.AreEqual(guid, property);
    }
    public class SagaDataWithGuidCorrelationId : ContainSagaData
    {
        [CorrelationId]
        public Guid CorrelationProperty { get; set; }
    }
    public class MySaga : XmlSaga<SagaData>
    {
        protected override void ConfigureMapping(MessagePropertyMapper<SagaData> mapper)
        {
            mapper.MapMessage<Message>(message => message.CorrelationProperty);
        }
    }

    public class SagaData : ContainSagaData
    {
        [CorrelationId]
        public string CorrelationProperty { get; set; }
    }
    public class Message
    {
        public string CorrelationProperty { get; set; }
    }
}