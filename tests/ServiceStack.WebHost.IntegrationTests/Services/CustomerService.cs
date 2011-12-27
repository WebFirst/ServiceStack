using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using ServiceStack.Common.Web;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace ServiceStack.WebHost.IntegrationTests.Services
{
	[RestService("/customers")]
	[RestService("/customers/{Id}")]
	public class Customers
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Company { get; set; }
		public decimal Discount { get; set; }
		public string Address { get; set; }
		public string Postcode { get; set; }
		public bool HasDiscount { get; set; }
	}

	public class CustomersValidator : AbstractValidator<Customers>
	{
		public CustomersValidator()
		{
			RuleSet(ApplyTo.Post | ApplyTo.Put, () => {
				RuleFor(x => x.LastName).NotEmpty();
				RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please specify a first name");
				RuleFor(x => x.Company).NotNull();
				RuleFor(x => x.Discount).NotEqual(0).When(x => x.HasDiscount);
				RuleFor(x => x.Address).Length(20, 250);
				RuleFor(x => x.Postcode).Must(BeAValidPostcode).WithMessage("Please specify a valid postcode");
			});
		}

		static readonly Regex UsPostCodeRegEx = new Regex(@"^\d{5}(-\d{4})?$", RegexOptions.Compiled);

		private bool BeAValidPostcode(string postcode)
		{
			return UsPostCodeRegEx.IsMatch(postcode);
		}
	}

	public class CustomersResponse
	{
		public Customers Result { get; set; }
	}

	public class CustomerService : RestServiceBase<Customers>
	{
		public override object OnGet(Customers request)
		{
			return new CustomersResponse { Result = request };
		}

		public override object OnPost(Customers request)
		{
			return new CustomersResponse { Result = request };
		}

		public override object OnPut(Customers request)
		{
			return new CustomersResponse { Result = request };
		}

		public override object OnDelete(Customers request)
		{
			return new CustomersResponse { Result = request };
		}
	}

}