using UnityEngine;
using System.Collections;

[System.Serializable]
public class Pair<X, Y> {

	public X fst;
	public Y snd;

	public Pair() {
		fst = default(X);
		snd = default(Y);
	}

	public Pair(X arg1, Y arg2) {
		fst = arg1;
		snd = arg2;
	}
}
