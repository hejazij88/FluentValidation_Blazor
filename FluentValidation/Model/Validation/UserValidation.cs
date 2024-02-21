using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Model.Validation
{
    public class UserValidation:AbstractValidator<User>
    {
        [Obsolete("Obsolete")]
        public UserValidation()
        {
            CascadeMode=CascadeMode.StopOnFirstFailure;

            RuleFor(user => user.FullName).NotEmpty()
                .NotNull()
                .MaximumLength(50)
                .MinimumLength(3)
                .NotEqual("Test");
            
            RuleFor(user => user.Phone).NotNull()
                .NotEmpty()
                .MinimumLength(11)
                .MaximumLength(11)
                .Matches(@"^(?:\+1\s ?) ?\(? ([0 - 9]{ 3})\)?[-.\s] ? ([0 - 9]{ 3})[-.\s] ? ([0 - 9]{ 4})$");

            RuleFor(user => user.Email).NotNull()
                .NotEmpty()
                .EmailAddress();

            RuleFor(user => user.NatinoalCode).NotNull()
                .NotEmpty()
                .Matches("^[0-9]{10}$"); //Persian Notion Code
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result =
                await ValidateAsync(ValidationContext<User>.CreateWithOptions((User)model,
                    x => x.IncludeProperties(propertyName)));

            return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
        };


    }
}
