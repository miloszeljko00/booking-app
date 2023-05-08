using AccomodationDomain.Exceptions.CustomExceptions;
using AccomodationDomain.Primitives;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationDomain.Entities
{
    public class Picture : Entity
    {
        public string FileName { get; init; }
        public string Description { get; init; }

        public Picture(Guid id): base(id) { } //obrisati kad se kreiraju dtovi
        private Picture(Guid id, string fileName, string description) : base(id)
        {
            FileName = fileName;
            Description = description;
        }
        public static Picture Create(Guid id, string filename, string description)
        {
            Picture picture = new Picture(id, filename, description);
            var validationResult = CheckIfPictureIsValid(picture);
            if (validationResult.IsValid)
            {
                return picture;
            }
            else
            {
                throw new InvalidPictureException();
            }
        }

        private static FluentValidation.Results.ValidationResult CheckIfPictureIsValid(Picture picture)
        {
            var pictureValidator = new PictureValidator();
            return pictureValidator.Validate(picture);
        }
    }
    internal class PictureValidator : AbstractValidator<Picture>
    {
        public PictureValidator()
        {
            RuleFor(x => x.FileName).NotEmpty().MaximumLength(100).Must(x => x.EndsWith(".jpg") || x.EndsWith(".png")).WithMessage("File name must end with '.jpg' or '.png'");
            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        }
    }
}
