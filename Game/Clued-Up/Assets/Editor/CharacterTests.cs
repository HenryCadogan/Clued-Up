using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class CharacterTests {

	Story testStory;
	Character testCharacter;


	[TestFixtureSetUp]
	public void testSetup(){
		testStory = new Story();
		testStory.setStory();
		testCharacter = testStory.getFullCharacterList()[0].GetComponent<Character>();
	}

	[Test]
	public void InitTest(){
		//testing to see if values have been initialised correctly
		Assert.IsNotNullOrEmpty(testCharacter.description);
		Assert.IsNotNullOrEmpty(testCharacter.longName);
	}

	[Test]
	public void NotVictimAndMurdererTest(){
		Assert.IsFalse(testCharacter.isVictim && testCharacter.isMurderer);
	} 
	[Test]
	public void SpriteTest(){
		Assert.NotNull(testCharacter.image);
	}


}
