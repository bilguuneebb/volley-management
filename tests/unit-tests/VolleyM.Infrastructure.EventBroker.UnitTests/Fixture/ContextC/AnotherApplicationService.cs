﻿using System.Threading.Tasks;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextC
{
    public class AnotherApplicationService : IEventHandler<EventG>
    {
        private readonly EventInvocationSpy _eventSpy;

        public AnotherApplicationService(EventInvocationSpy eventSpy)
        {
            _eventSpy = eventSpy;
        }

        public Task Handle(EventG @event)
        {
            return RegisterEvent(@event);
        }

        private Task RegisterEvent(IEvent @event)
        {
            _eventSpy.RegisterInvocation(@event);
            return Task.CompletedTask;
        }
    }
}