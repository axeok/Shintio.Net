using LibGit2Sharp;

namespace Shintio.Git.Services;

public class GitService
{
	private readonly Action<string> _log;

	private int _lastReceivedObjects = -1;

	public GitService(string localPath, string remotePath, Action<string> log)
	{
		LocalPath = localPath;
		RemotePath = remotePath;

		_log = log;
	}

	public string LocalPath { get; }
	public string RemotePath { get; }

	public Repository GetRepository()
	{
		return new Repository(LocalPath);
	}

	public Task Initialize()
	{
		if (!Repository.IsValid(LocalPath))
		{
			var options = new CloneOptions()
			{
				FetchOptions =
				{
					OnProgress = output =>
					{
						_log(output);

						return true;
					},
					OnTransferProgress = progress =>
					{
						if (_lastReceivedObjects == progress.ReceivedObjects)
						{
							return true;
						}

						_log($"Received {progress.ReceivedObjects}/{progress.TotalObjects} objects");
						_lastReceivedObjects = progress.ReceivedObjects;

						return true;
					},
				},
				OnCheckoutProgress = (path, steps, totalSteps) =>
				{
					_log($"Cloning Tesseract data({steps}/{totalSteps}): {path}");
				}
			};

			_log("Cloning Tesseract data...");
			Repository.Clone(RemotePath, LocalPath, options);
		}

		return Task.CompletedTask;
	}
}