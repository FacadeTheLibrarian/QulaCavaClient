using System.Collections.Generic;
using UnityEngine;

internal struct LoginChallenge {
	public string message;
	public string data;
	public LoginChallenge(string message, string data) {
		this.message = message;
		this.data = data;
	}
}