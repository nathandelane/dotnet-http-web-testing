# The ValidateResponse Event #

ValidateResponse is probably the event that you will use most while testing web sites an web applications. It should be used to validate text or values in the HTTP response body. In order to connect to the ValidateResponse event you must attach a ValidationRule object via its Validate method.

Below is an example of how to accomplish this:
```
// ...
	ValidateFormField rule1 = new ValidateFormField();
	rule1.Name = "q";
	rule1.ExpectedValue = String.Empty;
	request1.ValidateResponse += new WebTestRequest.ValidationEventHandler(rule1.Validate);
// ...
```

Here I showcase the ValidateFormField validation rule. The class ValidationRule itself is abstract and so you must extend it if you are planning on creating your own ValidationRule. The method used to attach the event handler to the event is the standard method used in .NET programming, using delegates. The ValidateResponse event is the second to last event called during the testing process. This may be important to know.

The ValidationRule abstract base class has the following signature:
```
public abstract class ValidationRule
{
	protected _context: Nathandelane.TestingTools.WebTesting.WebTestContext
	protected _message: string

	public RuleDescription: string
	public RuleName: string
	public Document: System.Xml.LinqXDocument
	public Required: bool
	public Outcome: Nathandelane.TestingTools.WebTesting.WebTestOutcome

	ValidationRule(string name, string description)

	public virtual void Validate(Object sender, Nathandelane.TestingTools.WebTesting.Events.ValidationEventArgs e)
}
```