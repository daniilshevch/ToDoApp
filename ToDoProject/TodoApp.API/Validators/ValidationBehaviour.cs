using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace ToDoApp.API.Validators
{
    public class ValidationBehaviour<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if(_validators.Any())
            {
                ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);
                ValidationResult[] validationResults = await 
                    Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));
                ValidationFailure[] validationFailures = validationResults.SelectMany(result => result.Errors).Where(failure => failure != null).ToArray();
                if(validationFailures.Any())
                {
                    throw new ValidationException(validationFailures);
                }    
            }
            return await next.Invoke();
        }
    }
}
