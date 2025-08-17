using System.Collections.Generic;
using System.Threading;
using UnityEngine;

internal sealed class Root : MonoBehaviour {
	[SerializeField] private WebTest _login = default;

    private CancellationToken _token = default;
    private void Awake() {
        _token = this.destroyCancellationToken;
        _login.SetUp(_token);
    }
}