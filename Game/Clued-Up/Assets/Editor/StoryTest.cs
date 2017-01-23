using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.IO;

public class StoryTest {
	Story testStory = new Story();

	[Test]
	/// <summary>
	/// Tests the loading in of the characters
	/// </summary>
	public void CharacterTest(){
		//setup a new story to perform tests upon
		testStory.setStory ();
		testStory.setStory ();
		//testing that the charcaters have been loaded in correctly
//		Assert.IsNotNullOrEmpty(testStory.getFullCharacterList());
	}

	[Test]
	/// <summary>
	/// Tests the exception catching/throwing of the clue loading
	/// </summary>
	public void ClueTest(){
		//setup a new story
		testStory.setStory();
		//call the clue info with an invalid name to prove it throws the exception
//		Assert.Throws<System.ArgumentException>(testStory.getClueInformation(""));
	}

	[Test]
	public void getIntro1Test(){
		testStory.setStory ();
		//check the weather is assigned correctly
//		Assert.IsNotNullOrEmpty (testStory.getWeather ());
		//change weather index to invalid value
//		testStory.weather = -1;

	}


}
