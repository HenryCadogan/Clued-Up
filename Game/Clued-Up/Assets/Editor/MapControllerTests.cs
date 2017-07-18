using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

public class MapControllerTests {
	MapController testMapController;

	[TestFixtureSetUp]
	public void TestSetup(){
		testMapController = new MapController();
	}

	[Test]
	public void updateMapButtonsTest(){
		try{
			testMapController.updateMapButtons();
			Assert.Pass();
		} catch (Exception e){}
		Assert.AreEqual(Time.timeScale, 0);
	
	}

	[Test]
	public void LoadSceneTest(){
		for (int x = 0; x < 7; x ++){
			try{
				Debug.Log(x);
				testMapController.loadScene("Room"+x);
				Assert.Pass();
			}catch(Exception e){}
		}
	}
}
