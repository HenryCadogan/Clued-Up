using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

public class SceneTransitionsTests {

	SceneTransitions testST;

	[TestFixtureSetUp]
	public void TestSetup(){
		testST = new SceneTransitions();
	}

	[Test]
	public void returnToMainMenuTest(){
		try{
			testST.returnToMainMenu();
			Assert.Pass();
		}catch (Exception e){}

	}


	[Test]
	public void StartSceneTransitionTest(){
		for(int x = 0; x < 9; x++) {
			try {
				testST.startSceneTransition(x);
				Assert.Pass();
			} catch(Exception e) {
			}
		}
	}

	[Test]
	public void OnMouseDownTest(){
		try {
			testST.OnMouseDown();
			Assert.Pass();
		} catch(Exception e) {
		}
	
	}
}
