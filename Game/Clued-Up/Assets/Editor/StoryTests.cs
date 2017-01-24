using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections;
using System;


public class StoryTests {
	Story testStory ;

	[TestFixtureSetUp]
	public void TestSetup(){
		testStory = new Story();
		testStory.setStory();
	}

	[Test]
	public void TestStoryCreation(){
		//now perform all the checks to see if it has been initialised correctly
		Assert.IsNotNull(testStory);
	}

	[Test]
	public void WeatherInitTest(){
		//has weather been initialised?
		Assert.NotNull(testStory.getWeather());
	}

	[Test]
	public void GetTraitStringTest(){
		//are all values accepted for the traits
		for(int x = 0; x <= 6; x++) {
			Assert.IsNotEmpty(testStory.getTraitString(x));
		}
		//getTraitString should throw an error with an invalid index E.G: -1
		try {
			testStory.getTraitString(-1);
			Assert.Fail();
		} catch(Exception e) {
		}
	}

	[Test]
	public void GetDetectiveTest(){
		//has the detective been initialised
		Assert.NotNull(testStory.getDetective());
	}
	[Test]
	public void CharactersLoadedTest(){
		//have the characters been initialised
		Assert.IsNotEmpty(testStory.getFullCharacterList());
	}

	[Test]
	public void VictimLoadedTest(){
		//is there a victim
		Assert.NotNull(testStory.getVictim());
	}

	[Test]
	public void MurdererLoadedTest(){
		//is there a murderer
		Assert.NotNull(testStory.getMurderer());
	}
	[Test]
	public void GetRandomAliveCharacter(){
		//can get a random character
		Assert.NotNull(testStory.randomAliveCharacter());
	}
	[Test]
	public void IntroLoadingTest(){
		//intro and intro1 text isnt empty for all values
		for(int x = 2; x < 3; x++) {
			Assert.IsNotEmpty(testStory.getIntro(x));				
		}

	}
	[Test]
	public void Intro1LoadingTest(){
		Assert.IsNotEmpty(testStory.getIntro1());
	}

	[Test]
	public void FullCharacterListTest(){
		//getting character information
		foreach(GameObject character in testStory.getFullCharacterList()) {
			Assert.NotNull(testStory.getCharacterInformation(character.GetComponent<Character>().name));
		}
	}

	[Test]
	public void CharacterInfoErrorTest(){
		try {
			testStory.getCharacterInformation("");
			Assert.Fail();
		} catch(Exception e) {
		}
	}

	[Test]
	public void CharacterInRoomTest(){
		try {
			testStory.getCharactersInRoom(-1);
			Assert.Fail();
		}catch (Exception e){
		}
	}

}