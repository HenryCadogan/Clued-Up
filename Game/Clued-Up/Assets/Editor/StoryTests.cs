using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections;
using System;


public class StoryTests {
	Story testStory = new Story();

	[Test]
	public void testStoryElements(){
		//initialise a new story
		testStory.setStory();
		//now perform all the checks to see if it has been initialised correctly


		//has weather been initialised?
		Assert.NotNull(testStory.getWeather());
		Debug.Log("Weather getter passed");
		//are all values accepted for the traits
		for (int x = 0; x <=6 ; x ++){
			Assert.IsNotEmpty (testStory.getTraitString(x));
		}
		Debug.Log("Trait string getter passed for all values");
		//getTraitString should throw an error with an invalid index E.G: -1
		try {
			testStory.getTraitString(-1);
			Assert.Fail();
		}catch (Exception e){
		}
		Debug.Log("Trait string error throw passed");

		//has the detective been initialised
		Assert.NotNull(testStory.getDetective());
		Debug.Log("Get detective passed");

		//have the characters been initialised
		Assert.IsNotEmpty(testStory.getFullCharacterList());
		Debug.Log("Character list not empty passed");

		//is there a victim
		Assert.NotNull(testStory.getVictim());
		Debug.Log("Victim created passed");
		//is there a murderer
		Assert.NotNull(testStory.getMurderer());
		Debug.Log("Murderer created passed");
		//can get a random character
		Assert.NotNull(testStory.randomAliveCharacter());
		Debug.Log("Get random character passed");
		//intro and intro1 text isnt empty for all values
		for (int x = 2; x <=4; x++ ){
			Assert.IsNotEmpty(testStory.getIntro(x));				
		}
		Debug.Log("Numbered Intro's loaded passed");
		Assert.IsNotEmpty(testStory.getIntro1());
		Debug.Log("Intro 1 loaded passed");

		//getting character information
		foreach (GameObject character in testStory.getFullCharacterList()){
			Assert.NotNull(testStory.getCharacterInformation(character.GetComponent<Character>().name));
		}
		Debug.Log("Character information loaded passed");

		try {
			testStory.getCharacterInformation("");
			Assert.Fail();
		}catch (Exception e){
		}
		Debug.Log("Character infor error throw passed");



		Debug.Log("All tests Passed in StoryTests");

	}


}
