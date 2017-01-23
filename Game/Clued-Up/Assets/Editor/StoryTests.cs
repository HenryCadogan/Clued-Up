using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class StoryTests {
	Story testStory = new Story();

	[Test]
	public void testStoryElements(){
		//initialise a new story
		testStory.setStory();
		//now perform all the checks to see if it has been initialised correctly


		//has weather been initialised?
		Assert.NotNull(testStory.getWeather());
		//are all values accepted for the traits
		for (int x = 0; x <=6 ; x ++){
			Assert.IsNotEmpty (testStory.getTraitString(x));
		}
		//getTraitString should throw an error with an invalid index E.G: -1
		//Assert.Throws<System.IndexOutOfRangeException>(new TestDelegate(testStory.getTraitString(-1)));

		//has the detective been initialised
		Assert.NotNull(testStory.getDetective());

		//have the characters been initialised
		Assert.IsNotEmpty(testStory.getFullCharacterList());

		//is there a victim
		Assert.NotNull(testStory.getVictim());

		//is there a murderer
		Assert.NotNull(testStory.getMurderer());

		//can get a random character
		Assert.NotNull(testStory.randomAliveCharacter());

		//intro and intro1 text isnt empty for all values
		for (int x = 2; x <=4; x++ ){
			Assert.IsNotEmpty(testStory.getIntro(x));				
		}
		Assert.IsNotEmpty(testStory.getIntro1());


		//getting character information
		foreach (GameObject character in testStory.getFullCharacterList()){
			Assert.NotNull(testStory.getCharacterInformation(character.GetComponent<Character>().name));
		}
		//give empty character name to test error throwing
		//Assert.Throws<System.ArgumentException>(new TestDelegate(testStory.getCharacterInformation("")).Method);



	}


}
