using Colegio.Application.DTOs;
using FluentValidation;

namespace Colegio.Application.Validators;

public class TeacherRequestValidator : AbstractValidator<TeacherRequestDTO>
{
    public TeacherRequestValidator()
    {
        RuleFor(x => x.DocumentNumber).NotEmpty().MaximumLength(30);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(80);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(80);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(120);
        RuleFor(x => x.Status).NotEmpty().Must(s => s is "Active" or "Inactive");
    }
}
