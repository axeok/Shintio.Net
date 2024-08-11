namespace Shintio.CodeProcessor.Models
{
	public interface ICombiner
	{
		string Combine(CombineOptions options, params SharpFile[] files);
	}
}