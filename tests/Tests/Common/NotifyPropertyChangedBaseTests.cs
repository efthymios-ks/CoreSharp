namespace CoreSharp.Common.Tests;

[TestFixture]
public class NotifyPropertyChangedBaseTests
{
    [Test]
    public void OnPropertyChanged_WhenCalled_ShouldInvokePropertyChangedEvent()
    {
        // Arrange
        var instance = new NotifyPropertyChangedClass();
        var eventRaised = false;
        instance.PropertyChanged += (sender, args)
            => eventRaised = true;

        // Act
        instance.Property1 = "NewValue";

        // Assert
        eventRaised.Should().BeTrue();
    }

    [Test]
    public void SetValue_WhenValueDifferent_ShouldSetFieldAndInvokePropertyChangedEvent()
    {
        // Arrange
        var instance = new NotifyPropertyChangedClass();
        var newValue = "Changed";
        var eventRaised = false;
        instance.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(NotifyPropertyChangedClass.Property1))
            {
                eventRaised = true;
            }
        };

        // Act
        instance.Property1 = newValue;

        // Assert
        instance.Property1.Should().Be(newValue);
        eventRaised.Should().BeTrue();
    }

    [Test]
    public void SetValue_WhenValueSame_ShouldNotSetFieldOrInvokePropertyChangedEvent()
    {
        // Arrange
        var instance = new NotifyPropertyChangedClass();
        var initialValue = instance.Property1;
        var newValue = initialValue;
        var eventRaised = false;
        instance.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(NotifyPropertyChangedClass.Property1))
            {
                eventRaised = true;
            }
        };

        // Act
        instance.Property1 = newValue;

        // Assert
        instance.Property1.Should().Be(initialValue);
        eventRaised.Should().BeFalse();
    }

    private sealed class NotifyPropertyChangedClass : NotifyPropertyChangedBase
    {
        private string _property1;

        public string Property1
        {
            get => _property1;
            set => SetValue(ref _property1, value);
        }
    }
}