using AccomodationSuggestionDomain.Exceptions.CustomExceptions;
using AccomodationSuggestionDomain.Primitives;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestionDomain.Entities
{
    public class Picture : Entity
    {
        public string FileName { get; init; }
        public string Base64 { get; init; }

        public Picture(Guid id): base(id) { } //obrisati kad se kreiraju dtovi
        private Picture(Guid id, string fileName, string base64) : base(id)
        {
            FileName = fileName;
            Base64 = base64;
        }
        public static Picture Create(Guid id, string filename, string base64)
        {
            Picture picture = new Picture(id, filename, base64);
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
            
        }
    }
}
