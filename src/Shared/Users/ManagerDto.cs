using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Svk.Shared.Users;

public class ManagerDto
{
    public class Index
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }

    public class Detail
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Auth0Id { get; set; }

        public string? Email { get; set; }
    }

    public class Mutate
    {
        public string? Name { get; set; }
        public string? Auth0Id { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        public class Validator : AbstractValidator<Mutate>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
                RuleFor(x => x.Auth0Id).NotEmpty().MaximumLength(1_000);
                RuleFor(x => x.Email).NotEmpty().MaximumLength(1_000);
            }
        }
    }
}