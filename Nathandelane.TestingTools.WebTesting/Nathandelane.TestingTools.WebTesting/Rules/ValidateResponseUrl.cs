using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nathandelane.TestingTools.WebTesting.Events;

namespace Nathandelane.TestingTools.WebTesting.Rules
{
	public class ValidateResponseUrl : ValidationRule
	{
		#region Fields

		private string _expectedResponseUrl;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the expected response URL.
		/// </summary>
		public string ExpectedResponseUrl
		{
			get { return _expectedResponseUrl; }
			set { _expectedResponseUrl = value; }
		}

		#endregion

		#region Methods

		public override void Validate(object sender, ValidationEventArgs e)
		{
			base.Validate(sender, e);

			if (((WebTestRequest)e.WebTestItem).ResponseUrl.Equals(_expectedResponseUrl))
			{
				_context.Outcome = WebTestOutcome.Passed;
			}
			else
			{
				_context.Outcome = WebTestOutcome.Failed;
			}
		}

		#endregion
	}
}
