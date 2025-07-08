using FluentValidation;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace CodeCraft.NET.Application.CQRS.Base.Features
{
	public abstract class BaseValidator<TEntity> : AbstractValidator<TEntity>
	{
		public readonly string GREATER_THAN_ZERO = "{0} must be greater than 0.";
		public readonly string LESS_THAN_X = "{0} must be less than {1}.";
		public readonly string PROPERTY_REQUIRED = "{0} is required.";
		public readonly string PROPERTY_LENGTH = "{0} length must be between {1} and {2} characters.";
		public readonly string PROPERTY_RANGE = "{0} range must be between {1} and {2}.";
		public readonly string POSTAL_CODE_REQUIRED = "Postal Code is required.";
		public readonly string POSTAL_CODE_FORMAT = "Postal Code must be 5 digits.";
		public readonly string EMAIL_NOT_VALID = "Email is not valid.";
		public readonly string DATETIME_NOTULL_OR_MIN_MAX = "{0} cannot be null or have maximum or minimum values.";

		protected void IdRules(Expression<Func<TEntity, int>> propertyExpression, string propertyName)
		{
			RuleFor(propertyExpression)
				.NotNull()
				.LessThan(0).WithMessage(string.Format(GREATER_THAN_ZERO, propertyName));
		}

		protected void IdRules(Expression<Func<TEntity, int?>> propertyExpression, string propertyName)
		{
			RuleFor(propertyExpression)
				.Must(x => x != null && x < 0).WithMessage(string.Format(GREATER_THAN_ZERO, propertyName));
		}

		protected void StringRules(Expression<Func<TEntity, string>> propertyExpression, string propertyName, int minLength, int maxLength)
		{
			RuleFor(propertyExpression)
				.NotEmpty().WithMessage(string.Format(PROPERTY_REQUIRED, propertyName))
				.Length(minLength, maxLength).WithMessage(string.Format(PROPERTY_LENGTH, propertyName, minLength, maxLength));
		}

		protected void DecimalRules(Expression<Func<TEntity, decimal>> propertyExpression, string propertyName, decimal minValue, decimal maxValue)
		{
			RuleFor(propertyExpression)
				.InclusiveBetween(minValue, maxValue).WithMessage(string.Format(PROPERTY_RANGE, propertyName, minValue, maxValue));
		}

		protected void DoubleRules(Expression<Func<TEntity, double>> propertyExpression, string propertyName, double minValue, double maxValue)
		{
			RuleFor(propertyExpression)
				.InclusiveBetween(minValue, maxValue).WithMessage(string.Format(PROPERTY_RANGE, propertyName, minValue, maxValue));
		}

		protected void IntRules(Expression<Func<TEntity, int>> propertyExpression, string propertyName, int minValue, int maxValue)
		{
			RuleFor(propertyExpression)
				.InclusiveBetween(minValue, maxValue).WithMessage(string.Format(PROPERTY_RANGE, propertyName, minValue, maxValue));
		}

		protected void BoolRules(Expression<Func<TEntity, bool>> propertyExpression, string propertyName)
		{
			RuleFor(propertyExpression)
				.NotEmpty().WithMessage(string.Format(PROPERTY_REQUIRED, propertyName));
		}

		protected void DateTimeRules(Expression<Func<TEntity, DateTime?>> propertyExpression, string propertyName)
		{
			RuleFor(propertyExpression)
				.Must(x => x == null || x >= DateTime.MinValue || x <= DateTime.MaxValue)
				.WithMessage(DATETIME_NOTULL_OR_MIN_MAX);
		}

		protected void PostalCodeRule(Expression<Func<TEntity, string>> propertyExpression)
		{
			RuleFor(propertyExpression)
				.NotEmpty().WithMessage(POSTAL_CODE_REQUIRED)
				.Matches(@"^\d{5}$").WithMessage(POSTAL_CODE_FORMAT);
		}

		protected void EmailRule(Expression<Func<TEntity, string>> propertyExpression)
		{
			RuleFor(propertyExpression)
				.NotEmpty().WithMessage(PROPERTY_REQUIRED)
				.Must(x => Regex.IsMatch(x, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
				.EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
				.WithMessage(EMAIL_NOT_VALID);
		}
	}
}
