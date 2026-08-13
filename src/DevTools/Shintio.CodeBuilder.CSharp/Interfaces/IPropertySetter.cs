namespace Shintio.CodeBuilder.CSharp.Interfaces;

public interface IPropertySetter : ICodeBlock
{
	public string GetCode(int indent, string modifier);
}