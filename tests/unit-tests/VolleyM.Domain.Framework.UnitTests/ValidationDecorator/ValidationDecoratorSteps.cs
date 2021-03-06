﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using LanguageExt;
using NSubstitute;
using SimpleInjector;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.IDomainFrameworkTestFixture;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Framework.UnitTests.ValidationDecorator
{
	[Binding]
    [Scope(Feature = "Validation Decorator")]
    public class ValidationDecoratorSteps
    {
        private enum HandlerType
        {
            SampleHandler,
            NoValidationHandler
        }

        private HandlerType _handlerType;
        private bool _validationSuccess;
        private Either<Error, Unit> _actualResult;

        private readonly Container _container;
        private IAuthorizationService _authorizationService;

        public ValidationDecoratorSteps(Container container)
        {
            _container = container;
        }

        [BeforeScenario(Order = Constants.BEFORE_SCENARIO_REGISTER_DEPENDENCIES_ORDER)]
        public void RegisterDependenciesForScenario()
        {
            RegisterHandlers();

            _authorizationService = Substitute.For<IAuthorizationService>();
            _container.RegisterInstance(_authorizationService);
        }

        [Given(@"I have a handler")]
        public void GivenIHaveAHandler()
        {
            // do nothing
        }

        [Given(@"validator is not defined")]
        public void GivenValidatorIsNotDefined()
        {
            _handlerType = HandlerType.NoValidationHandler;
            SetPermissionForHandler();
        }

        [Given(@"single validator defined")]
        public void GivenSingleValidatorDefined()
        {
            _handlerType = HandlerType.SampleHandler;
            SetPermissionForHandler();
        }

        [Given(@"validator passes")]
        public void GivenValidatorPasses()
        {
            _validationSuccess = true;
        }

        [Given(@"validator fails")]
        public void GivenValidatorFails()
        {
            _validationSuccess = false;
        }

        [When(@"I call decorated handler")]
        public async Task WhenICallDecoratedHandler()
        {
            _actualResult = await ResolveAndCallHandler(_handlerType).ToEither();
        }

        [Then(@"handler result should be returned")]
        public void ThenHandlerResultShouldBeReturned()
        {
            _actualResult.ShouldBeEquivalent(Unit.Default);
        }

        [Then(@"validation error should be returned")]
        public void ThenValidationErrorShouldBeReturned()
        {
            _actualResult.ShouldBeError(ErrorType.ValidationFailed);
        }

        private EitherAsync<Error, Unit> ResolveAndCallHandler(HandlerType handlerType)
        {
            return handlerType switch
            {
                HandlerType.NoValidationHandler => ResolveAndCallSpecificHandler<NoValidationHandler.Request>(HandlerParameterFactory),
                HandlerType.SampleHandler => ResolveAndCallSpecificHandler<SampleHandler.Request>(HandlerParameterFactory),
                _ => throw new NotSupportedException()
            };
        }
        private EitherAsync<Error, Unit> ResolveAndCallSpecificHandler<T>(Func<HandlerType, IRequest<Unit>> requestFactory) where T : IRequest<Unit>
        {
            var handler = _container.GetInstance<IRequestHandler<T, Unit>>();

            return handler.Handle((T)requestFactory(_handlerType));
        }

        private void RegisterHandlers()
        {
            FrameworkDomainComponentDependencyRegistrar.RegisterCommonServices(_container,
                new List<Assembly> { Assembly.GetAssembly(GetType()) });
        }

        private IRequest<Unit> HandlerParameterFactory(HandlerType type)
        {
            return type switch
            {
                HandlerType.SampleHandler => (IRequest<Unit>)new SampleHandler.Request { A = _validationSuccess ? 0 : 1 },
                HandlerType.NoValidationHandler => new NoValidationHandler.Request(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private void SetPermissionForHandler()
        {
            _authorizationService
                .CheckAccess(
                    new Permission(nameof(IDomainFrameworkTestFixture), _handlerType.ToString()))
                .Returns(true);
        }

    }
}