﻿Feature: EventBroker
	In order to be able to communicate assynchronously
	As a developer I need event publisher
	Which will deliver events to registered event handlers

@ab:1099
Scenario Outline: Publish event
	Given I have <HandlerCount> <HandlerType> event handlers for <Event>
	When I publish <Event>
	Then handler result should be returned
	And handler should receive event

	Examples:
		| Event                                         | HandlerCount | HandlerType |
		| SingleSubscriberEvent                         | 1            | Internal    |
		| NoSubscribersEvent                            | 0            | Internal    |
		| SeveralSubscribersEvent                       | 2            | Internal    |
		| PublicSingleSubscriberEvent                   | 1            | Public      |
		| PublicNoSubscribersEvent                      | 0            | Public      |
		| PublicSeveralSubscribersEvent                 | 2            | Public      |
		| PublicSeveralDifferentContextSubscribersEvent | 3            | Public      |

@ab:1099
Scenario: Event published twice
	Given I have 1 internal event handler for SingleSubscriberEvent
	And SingleSubscriberEvent was published once
	When I publish SingleSubscriberEvent
	Then handler result should be returned
	And handler should receive all events

@ab:1099
Scenario: Event has internal and external subscribers
	Given I have internal and public event handler for InternalAndPublicEvent
	When I publish InternalAndPublicEvent
	Then handler result should be returned
	And handler should receive event
	
@ab:1099
Scenario: Consumer can have less properties on the event
	Given I have internal and public event handler for IgnorePropertyEvent
	When I publish IgnorePropertyEvent
	Then handler result should be returned
	And handler should receive event

# quite unexplicable case - everything is done according to the documentation of simple injector but it still does not work
@ab:1099 @ignore
Scenario: Each event handler is invoked in separate scope
	Given I have 2 event handlers for SeparateScopesEvent
	When I publish SeparateScopesEvent
	Then handler result should be returned
	And events received by handlers should have following data:
		| RequestScope | EventHandlerScope |
		| rootScope    | eventScope1       |
		| rootScope    | eventScope2       |
