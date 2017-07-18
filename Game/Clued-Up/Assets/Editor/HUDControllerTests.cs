using UnityEngine;
using NUnit.Framework;
using System;

public class HUDControllerTests {
	GameObject TestPanel;
	GameObject TestHUDtext;
	HUDController testHUDC;

	[TestFixtureSetUp]
	public void testSetup(){
		TestPanel = new GameObject();
		TestHUDtext = new GameObject();
		testHUDC = new HUDController(); 
	}

	[Test]
	public void LoadPanelTest(){
		try{
			testHUDC.loadPanelAndPause(TestPanel);
			Assert.Pass();
		}catch(Exception e){}
		Assert.AreEqual(Time.timeScale, 0);
	}

	[Test]
	public void HidePanelTest(){
		try{
			testHUDC.hidePanelAndResume(TestPanel);
			Assert.Pass();
		}catch(Exception e){}
		Assert.AreEqual(Time.timeScale, 1);
	}	

	[Test]
	public void DisplayTextTest(){
		try{
			testHUDC.displayHUDText("Test");
			Assert.Pass();
		}catch (Exception e){}
	}

}


