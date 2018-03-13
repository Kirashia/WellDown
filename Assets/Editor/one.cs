using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class one {

	[Test]
	public void oneSimplePasses() {
        // Use the Assert class to test conditions.
        Debug.Log("runnin2:29g");
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator oneWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
        Debug.Log("runnin2:29sg");
        // yield to skip a frame2:29
        yield return null;
	}
}
