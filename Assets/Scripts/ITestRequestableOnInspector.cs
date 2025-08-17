using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[assembly: InternalsVisibleTo("Assembly-CSharp-Editor")]
internal interface ITestRequestableOnInspector {
	public UniTask SendAnyRequest();
}