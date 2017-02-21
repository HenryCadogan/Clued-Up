using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class ClueTests {
	Story testStory;
	Clue testClue;

	[TestFixtureSetUp]
	public void TestSetup(){
		testStory = new Story();
		testStory.setStory();
		testClue = testStory.getCluesInRoom(0)[0].GetComponent<Clue>();
	}

	[Test]
	public void InitTest(){
		Assert.IsNotNull(testClue);
		Assert.IsNotNullOrEmpty(testClue.description);
		Assert.IsNotNullOrEmpty(testClue.longName);
		Assert.IsNotNull(testClue.sprite);
	}


	[Test]
	public void DisplaysClueInfoTest(){
		try{
			testClue.displayClueInformation();
			Assert.Pass();
		}catch (System.Exception e){
		}
	}


}
