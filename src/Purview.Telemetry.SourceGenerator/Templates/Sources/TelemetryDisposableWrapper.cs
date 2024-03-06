sealed class TelemetryDisposableWrapper : System.IDisposable {
	bool _disposedValue;
	readonly System.IDisposable[] _disposables;

	public TelemetryDisposableWrapper(params System.IDisposable[] disposables) {
		_disposables = disposables ?? System.Array.Empty<System.IDisposable>();
	}

	void Dispose(bool disposing) {
		if (!_disposedValue) {
			if (disposing) {
				for (var i = 0; i < _disposables.Length; i++) {
					try {
						_disposables[i]?.Dispose();
					}
					catch {
						// Ignore
					}
				}
			}

			_disposedValue = true;
		}
	}

	public void Dispose() {
		Dispose(disposing: true);
		System.GC.SuppressFinalize(this);
	}
}
