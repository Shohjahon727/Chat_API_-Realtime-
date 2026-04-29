using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatAPI.Application.Validators
{
	public class ValidationBehavior<TRequest, TResponse>
		: IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IRequest<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators;

		public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
		{
			_validators = validators;
		}

		public async Task<TResponse> Handle(
			TRequest request,
			RequestHandlerDelegate<TResponse> next,
			CancellationToken cancellationToken)
		{
			if (_validators.Any())
			{
				var context = new ValidationContext<TRequest>(request);

				var failures = await Task.WhenAll(
					_validators
						.Select(v => v.ValidateAsync(context, cancellationToken))
						.Select(async v => await v)
				);

				var errorList = failures
					.SelectMany(r => r.Errors)
					.Where(f => f != null)
					.ToList();

				if (errorList.Any())
					throw new ValidationException(errorList);
			}

			return await next();
		}
	}
}
